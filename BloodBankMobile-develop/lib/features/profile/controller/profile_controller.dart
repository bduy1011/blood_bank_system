import 'dart:io';

import 'package:blood_donation/app/app_util/enum.dart';
import 'package:path_provider/path_provider.dart';
import 'package:blood_donation/base/base_view/base_view.dart';
import 'package:blood_donation/utils/extension/getx_extension.dart';
import 'package:blood_donation/utils/secure_token_service.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:image_picker/image_picker.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../app/app_page/controller/app_page_controller.dart';
import '../../../app/config/routes.dart';
import '../../../core/localization/app_locale.dart';
import '../../../models/blood_donor.dart';
import '../../../utils/app_utils.dart';
import '../../../utils/phone_number_formater.dart';
import '../../home/controller/home_controller.dart';
import '../../scan_qr_code/scan_qr_code_screen.dart';

class ProfileController extends BaseModelStateful {
  final TextEditingController fullnameController = TextEditingController();
  final TextEditingController birthYearController = TextEditingController();
  final TextEditingController idCardController = TextEditingController();
  final TextEditingController phoneNumberController = TextEditingController();
  final TextEditingController passwordNumberController =
      TextEditingController();
  final SecureTokenService _tokenService = SecureTokenService();

  String? get note => getNote();
  String? get avatarUrl => appCenter.authentication?.avatarUrl;

  /// URL đầy đủ để tải ảnh avatar (server có thể trả relative path "avatars/xxx/avatar.heif").
  String? get avatarDisplayUrl {
    final u = avatarUrl;
    if (u == null || u.isEmpty) return null;
    if (u.startsWith('http://') || u.startsWith('https://')) return u;
    final base = backendProvider.url.toString().replaceAll(RegExp(r'/$'), '');
    return '$base/api/system-user/avatar';
  }

  /// Ảnh đã chọn nhưng chưa bấm "Cập nhật thông tin" — chỉ upload khi update profile.
  File? _pendingAvatarFile;
  File? get pendingAvatarFile => _pendingAvatarFile;
  bool get hasPendingAvatar => _pendingAvatarFile != null;

  @override
  void onBack() {
    // TODO: implement onBack
  }

  @override
  void onTapRightMenu() {
    // TODO: implement onTapRightMenu
  }

  @override
  Future<void> onInit() async {
    ///

    super.onInit();
    try {
      fullnameController.text = appCenter.authentication?.name ?? "";
      phoneNumberController.text = PhoneNumberFormatter.formatString(
          (appCenter.authentication?.phoneNumber ?? "").replaceAll(" ", ""));
      idCardController.text = appCenter.authentication?.cmnd ?? "";
    } catch (e) {
      // TODO
      // print(e);
    }
  }

  String? getNote() {
    return null;
  }

  void updateProfile(BuildContext context) async {
    // backendProvider.logout();
    // print(appCenter.authentication?.toJson());
    FocusScope.of(context).requestFocus(FocusNode());
    var cccd = idCardController.text.trim().replaceAll(" ", "");
    var phoneNumber = phoneNumberController.text.trim().replaceAll(" ", "");

    ///
    if (!cccd.isNum || !(cccd.length == 12 || cccd.length == 9)) {
      AppUtils.instance
          .showToast(AppLocale.profileIDCardInvalidFormat.translate(context));
      return;
    }
    if (!phoneNumber.isNum || phoneNumber.length != 10) {
      AppUtils.instance
          .showToast(AppLocale.profilePhoneInvalidFormat.translate(context));
      return;
    }

    ///
    try {
      showLoading();
      if (hasPendingAvatar) {
        final ok = await _uploadPendingAvatar(context);
        if (!ok) {
          hideLoading();
          return;
        }
      }
      final oldUserCode = appCenter.authentication?.userCode?.trim();
      var body = {
        "userCode": appCenter.authentication?.userCode,
        "name": fullnameController.text.trim(),
        "phoneNumber": phoneNumber,
        "password": "",
        "idCardNr": cccd,
        "appRole": appCenter.authentication?.appRole,
        "active": true,
      };
      var isModIdCard = idCardController.text != appCenter.authentication?.cmnd;
      var response = await backendProvider.updateAccount(
        body: body,
        code: appCenter.authentication?.userCode ?? "",
        isModIdCard: isModIdCard,
      );
      if (response.status == 200) {
        final newUserCode = response.data?.userCode?.trim();
        final shouldUpdateUserCode = newUserCode != null &&
            newUserCode.isNotEmpty &&
            newUserCode != oldUserCode;

        var dmNguoiHienMau =
            isModIdCard ? null : appCenter.authentication?.dmNguoiHienMau;
        if (response.data?.dmNguoiHienMau != null) {
          // dmNguoiHienMau = await getDMNguoiHienMau(
          //     idCardController.text, phoneNumberController.text);
          dmNguoiHienMau = response.data?.dmNguoiHienMau;
        }
        appCenter.authentication?.dmNguoiHienMau = dmNguoiHienMau;
        appCenter.authentication?.phoneNumber = phoneNumber;
        appCenter.authentication?.name = fullnameController.text;
        appCenter.authentication?.cmnd = cccd;
        if (shouldUpdateUserCode) {
          appCenter.authentication?.userCode = newUserCode;
        }
        appCenter.authentication?.accessToken = isModIdCard
            ? response.data?.accessToken
            : appCenter.authentication?.accessToken;
        appCenter.authentication?.ngayHienMauGanNhat =
            response.data?.ngayHienMauGanNhat;
        appCenter.authentication?.soLanHienMau = response.data?.soLanHienMau;
        appCenter.authentication?.duongTinhGanNhat =
            response.data?.duongTinhGanNhat;
        // Nhận avatarUrl từ server. Server có thể trả relative path — lưu thành URL đầy đủ. Thêm ?v=timestamp để tránh cache ảnh cũ.
        final raw = (response.data?.avatarUrl)?.toString().trim();
        if (raw != null && raw.isNotEmpty) {
          final base = raw.startsWith('http://') || raw.startsWith('https://')
              ? raw
              : '${backendProvider.url.toString().replaceAll(RegExp(r'/$'), '')}/api/system-user/avatar';
          final sep = base.contains('?') ? '&' : '?';
          appCenter.authentication?.avatarUrl =
              '$base${sep}v=${DateTime.now().millisecondsSinceEpoch}';
          await backendProvider.evictAvatarCache();
        }

        await backendProvider.saveAuthentication(appCenter.authentication!);

        // Cập nhật secure storage để mọi nơi (home, drawer, v.v.) đọc được avatar mới
        try {
          await _tokenService.saveAuthentication(appCenter.authentication!);
        } catch (_) {}

        // Đảm bảo: logout ra login page sẽ show CCCD/userCode mới (vì login lấy prefs["userName"])
        // và biometric/token cũng "đi theo CCCD mới" (update secure storage Authentication).
        if (shouldUpdateUserCode) {
          try {
            final prefs = await SharedPreferences.getInstance();
            await prefs.setString("userName", newUserCode);
          } catch (_) {}

          // Kiểm tra xem đã có authentication của user khác trong secure storage chưa
          // Nếu có user khác, không ghi đè để tránh mất vân tay của user đó
          try {
            final existingAuth = await _tokenService.getAuthentication();
            if (existingAuth?.userCode != null &&
                appCenter.authentication?.userCode != null) {
              final existingUserCode = existingAuth!.userCode!.trim();
              final newUserCode = appCenter.authentication!.userCode!.trim();
              final oldUserCodeTrimmed = oldUserCode?.trim();

              // Chỉ cập nhật nếu:
              // 1. existingAuth.userCode == newUserCode (cùng user, cập nhật thông tin)
              // 2. existingAuth.userCode == oldUserCode (cùng user, userCode thay đổi)
              // Nếu không phải cả hai trường hợp trên → đây là user khác, không ghi đè
              if (existingUserCode == newUserCode ||
                  (oldUserCodeTrimmed != null &&
                      existingUserCode == oldUserCodeTrimmed)) {
                // Cùng user → cập nhật authentication
                await _tokenService
                    .saveAuthentication(appCenter.authentication!);
              } else {
                // User khác → không ghi đè, giữ nguyên authentication của user đó
                // Log để debug
                print(
                    "[ProfileController] Biometric already linked to another account: ${existingAuth.userCode}. Skipping save to preserve biometric login.");
              }
            } else {
              // Không có authentication trong secure storage → lưu bình thường
              await _tokenService.saveAuthentication(appCenter.authentication!);
            }
          } catch (_) {}
        }

        AppUtils.instance
            .showToast(AppLocale.updateAccountSuccess.translate(context));
        hideLoading();
        refresh();
        Get.findOrNull<HomeController>()?.onRefresh();

        ///
        if (dmNguoiHienMau != null) {
          backToHome();
        }

        return;
      }
      AppUtils.instance.showToast(
          "${AppLocale.updateAccountFailed.translate(context)}\n${response.message ?? ""}");
    } catch (e, t) {
      print(e);
      print(t);
      // TODO
      AppUtils.instance
          .showToast(AppLocale.updateAccountFailed.translate(context));
    }
    hideLoading();
  }

  void backToHome() {
    try {
      Get.findOrNull<AppPageController>()?.onChangeHomeTab();
    } catch (e) {
      print(e);
    }

    ///
    Get.until((route) {
      var currentRoute = route.settings.name;
      debugPrint("Get.currentRoute --- $currentRoute");
      if (currentRoute == Routes.appPage) {
        return true;
      } else {
        return false;
      }
    });
  }

  Future<BloodDonor?> getDMNguoiHienMau(
      String idCard, String phoneNumber) async {
    ///
    try {
      var dataResponse = await backendProvider.getDMNguoiHienMauByIdCard(
          idCard: idCard, phoneNumber: phoneNumber);
      if (dataResponse.status == 200) {
        //
        return dataResponse.data;
      }
    } catch (e) {
      // TODO
    }
    return null;
  }

// 074202000733|281290246|Lê Nguyễn Anh Vũ|16112002|Nam|Tổ 6, Khu Phố 1,, Uyên Hưng, Tân Uyên, Bình Dương|13042021
  /// Chọn ảnh từ thư viện/máy ảnh — chỉ lưu tạm, chưa upload. Upload khi bấm "Cập nhật thông tin".
  Future<void> pickAvatar(BuildContext context) async {
    try {
      final picker = ImagePicker();
      final source = await showModalBottomSheet<ImageSource>(
        context: context,
        builder: (ctx) => SafeArea(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              ListTile(
                leading: const Icon(Icons.photo_library),
                title: Text(AppLocale.selectImageFromGallery.translate(ctx)),
                onTap: () => Navigator.pop(ctx, ImageSource.gallery),
              ),
              ListTile(
                leading: const Icon(Icons.camera_alt),
                title: Text(AppLocale.takePhoto.translate(ctx)),
                onTap: () => Navigator.pop(ctx, ImageSource.camera),
              ),
            ],
          ),
        ),
      );
      if (source == null) return;
      final xFile = await picker.pickImage(
          source: source, maxWidth: 1024, imageQuality: 85);
      if (xFile == null) return;
      File file = File(xFile.path);
      const allowedExt = [
        '.jpg',
        '.jpeg',
        '.png',
        '.gif',
        '.webp',
        '.heic',
        '.heif'
      ];
      final fname = xFile.path.split(RegExp(r'[/\\]')).last;
      final ext =
          fname.contains('.') ? '.${fname.split('.').last.toLowerCase()}' : '';
      if (!allowedExt.contains(ext)) {
        final dir = await getTemporaryDirectory();
        final newPath =
            '${dir.path}/avatar_upload_${DateTime.now().millisecondsSinceEpoch}.jpg';
        file = await file.copy(newPath);
      }
      _pendingAvatarFile = file;
      refresh();
    } catch (e, t) {
      print(e);
      print(t);
      AppUtils.instance
          .showToast(AppLocale.updateAccountFailed.translate(context));
    }
  }

  /// Gửi ảnh pending lên server (gọi trong updateProfile khi hasPendingAvatar).
  Future<bool> _uploadPendingAvatar(BuildContext context) async {
    if (_pendingAvatarFile == null) return true;
    final file = _pendingAvatarFile!;
    final response = await backendProvider.uploadAvatar(file);
    if (response.status != 200 || response.data == null) {
      AppUtils.instance.showToast(
          response.message ?? AppLocale.updateAccountFailed.translate(context));
      return false;
    }
    final url =
        (response.data is Map ? (response.data as Map)['avatarUrl'] : null)
            ?.toString();
    if (url != null && url.isNotEmpty) {
      final sep = url.contains('?') ? '&' : '?';
      appCenter.authentication?.avatarUrl =
          '$url${sep}v=${DateTime.now().millisecondsSinceEpoch}';
      _pendingAvatarFile = null;
      await backendProvider.evictAvatarCache();
    }
    return true;
  }

  Future<bool> scanQRCode() async {
    var rs = await Get.to(
      () => ScanQrCodeScreen(
        title: AppLocale.scanQRCCCD.translate(Get.context!),
        onScan: (code) async {
          //
          var ls = code.split("|");
          idCardController.text = ls[0];
          fullnameController.text = ls[2];
          return true;
        },
      ),
    );
    if (rs == "ok") {
      return true;
    }
    if (rs == "cancel") {
      return false;
    }
    return false;
  }

  @override
  Future<void> onClose() async {
    ///
    fullnameController.dispose();
    birthYearController.dispose();
    idCardController.dispose();
    phoneNumberController.dispose();
    passwordNumberController.dispose();
    super.onClose();
  }
}
