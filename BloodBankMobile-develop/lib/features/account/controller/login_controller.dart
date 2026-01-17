import 'dart:developer';

import 'package:blood_donation/app/app_util/app_center.dart';
import 'package:blood_donation/base/base_view/base_view.dart';
import 'package:blood_donation/core/backend/backend_provider.dart';
import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/models/authentication.dart';
import 'package:blood_donation/utils/app_utils.dart';
import 'package:blood_donation/utils/biometric_auth_service.dart';
import 'package:blood_donation/utils/secure_token_service.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:get_it/get_it.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../app/config/routes.dart';

class LoginController extends BaseModelStateful {
  ///
  final TextEditingController usernameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  SharedPreferences? prefs;

  ///

  @override
  Future<void> onClose() async {
    // Implement your hide dispose indicator

    ///
    usernameController.dispose();
    passwordController.dispose();

    super.onClose();
  }

  @override
  Future<void> onInit() async {
    prefs = await SharedPreferences.getInstance();
    initUserName();

    ///
    super.onInit();
  }

  Future<void> initUserName() async {
    ///
    var userName = prefs?.getString("userName");
    if (userName?.isNotEmpty == true) {
      usernameController.text = userName ?? "";
    }
  }

  Future<void> setUserName() async {
    ///
    prefs?.setString("userName", usernameController.text);
  }

  // ========== BIOMETRIC LOGIN (Tách biệt, không ảnh hưởng login bằng text) ==========
  final SecureTokenService _tokenService = SecureTokenService();
  final BackendProvider _backendProvider = BackendProvider();
  final AppCenter _appCenter = GetIt.instance<AppCenter>();

  /// Lưu tokens và toàn bộ Authentication vào secure storage sau khi login thành công
  /// Token được khóa bằng Face ID/vân tay cho đúng account (UserCode)
  Future<void> saveTokensToSecureStorage({
    required Authentication authentication,
    String? refreshToken,
  }) async {
    try {
      if (authentication.accessToken != null &&
          authentication.accessToken!.isNotEmpty) {
        log("Saving authentication to secure storage (locked by biometric): userCode=${authentication.userCode}, userName=${authentication.name}");
        await _tokenService.saveAuthentication(authentication);
        log("Authentication and tokens saved to secure storage successfully (locked by biometric)");
      } else {
        log("Warning: accessToken is null or empty, cannot save to secure storage");
      }
    } catch (e) {
      log("saveTokensToSecureStorage error: $e");
      rethrow;
    }
  }

  /// Kiểm tra xem có tokens đã lưu trong secure storage không
  Future<bool> hasStoredTokens() async {
    return await _tokenService.hasTokens();
  }

  /// Xóa tokens khỏi secure storage (khi logout)
  Future<void> clearStoredTokens() async {
    await _tokenService.clearTokens();
  }

  /// Đăng nhập bằng biometric (FaceID/Fingerprint)
  /// Flow: Kiểm tra token → Xác thực biometric → Lấy token từ secure storage → Set auth → Vào app
  Future<void> loginWithBiometric(BuildContext context) async {
    try {
      final biometricService = BiometricAuthService();

      // Bước 1: Kiểm tra xem có tokens đã lưu không
      final hasTokens = await hasStoredTokens();
      if (!hasTokens) {
        log("No tokens found");
        if (context.mounted) {
          await AppUtils.instance.showMessage(
            AppLocale.biometricNotLoggedIn.translate(context),
            context: context,
          );
        }
        return;
      }

      // Bước 2: Kiểm tra token có hết hạn không TRƯỚC KHI xác thực biometric
      // Nếu token hết hạn, server sẽ không cho refresh → yêu cầu đăng nhập lại
      final isExpired = await _tokenService.isAccessTokenExpired();
      if (isExpired) {
        log("Token expired. Cannot use biometric login. User must login with username/password.");
        await clearStoredTokens();
        if (context.mounted) {
          await AppUtils.instance.showMessage(
            AppLocale.biometricSessionExpired.translate(context),
            context: context,
          );
        }
        return;
      }

      // Bước 3: Lấy thông tin user đã lưu để hiển thị
      final userInfo = await _tokenService.getUserInfo();
      final savedUserCode = userInfo['userCode'];
      final savedUserName = userInfo['userName'];
      log("Saved user info - UserCode: $savedUserCode, UserName: $savedUserName");

      // Bước 4: Xác thực bằng biometric (chỉ khi token chưa hết hạn)
      String reason = AppLocale.biometricAuthReason.translate(context);
      if (savedUserName != null || savedUserCode != null) {
        final userDisplay = savedUserName != null 
            ? (savedUserCode != null ? "$savedUserName ($savedUserCode)" : savedUserName)
            : savedUserCode ?? '';
        reason = AppLocale.biometricLoginWithAccount.translate(context)
            .replaceAll('{account}', userDisplay)
            .replaceAll('{reason}', reason);
      }
      
      final didAuthenticate = await biometricService.authenticate(
        reason: reason,
        context: context,
      );

      if (!didAuthenticate) {
        return; // User cancel hoặc fail
      }

      log("Biometric authentication successful");
      AppUtils.instance.showLoading();

      // Bước 5: Lấy Authentication object từ secure storage (đã được khóa bằng biometric)
      log("Getting authentication from secure storage (unlocked by biometric)...");
      Authentication? savedAuth = await _tokenService.getAuthentication();
      
      if (savedAuth == null) {
        log("No authentication found in secure storage");
        AppUtils.instance.hideLoading();
        if (context.mounted) {
          AppUtils.instance.showToast(
              AppLocale.biometricAuthNotFound.translate(context));
        }
        return;
      }
      
      log("Authentication retrieved: userCode=${savedAuth.userCode}, name=${savedAuth.name}");

      // Bước 6: Kiểm tra lại token (đảm bảo token vẫn còn hợp lệ)
      final accessToken = savedAuth.accessToken ?? '';
      if (accessToken.isEmpty) {
        log("Access token is empty");
        AppUtils.instance.hideLoading();
        if (context.mounted) {
          AppUtils.instance.showToast(
              AppLocale.biometricTokenInvalid.translate(context));
        }
        await clearStoredTokens();
        return;
      }

      // Token đã được kiểm tra ở bước 2, nên không cần refresh nữa

      // Bước 7: Set authentication và vào app (KHÔNG cần gọi API - chỉ mở khóa token)
      log("Setting authentication from secure storage (no API call needed)");
      
      // Lưu vào localStorage để tương thích với code cũ
      await _appCenter.localStorage
          .saveAuthentication(authentication: savedAuth);
      
      // Set authentication
      _appCenter.setAuthentication(savedAuth);
      _backendProvider.notifyAuthentication(isAuthenticated: true);

      log("Authentication set successfully from secure storage");
      
      if (!context.mounted) {
        log("Context is not mounted, cannot show UI");
        return;
      }
      
      AppUtils.instance.hideLoading();
      AppUtils.instance
          .showToast(AppLocale.biometricAuthSuccess.translate(context));

      // Vào app
      autoGotoHomePage(context);
    } catch (e, t) {
      log("loginWithBiometric() error", error: e, stackTrace: t);
      AppUtils.instance.hideLoading();
      if (context.mounted) {
        final errorMessage = e.toString();
        log("Error details: $errorMessage");
        final displayError = errorMessage.length > 50 ? "${errorMessage.substring(0, 50)}..." : errorMessage;
        AppUtils.instance.showToast(
          AppLocale.biometricError.translate(context).replaceAll('{error}', displayError),
        );
      }
    }
  }
  // ========== END BIOMETRIC LOGIN ==========

  @override
  Future<void> onReady() {
    // TODO: implement onReady
    // test();
    return super.onReady();
  }

  // test() async {
  //   Future.delayed(const Duration(seconds: 2), () {
  //     ///
  //     var json = jsonDecode(
  //         '''{ "id": 22, "ngay": "2024-11-24T07:30:00.000", "nguoiHienMauId": 340227, "hoVaTen": "TRẦN VĂN NỔI", "ngaySinh": null, "namSinh": null, "cmnd": "077080006127", "gioiTinh": null, "maXa": null, "tenXa": null, "maHuyen": "238", "tenHuyen": "Châu Đức", "maTinh": "77", "tenTinh": "BR Vũng Tàu", "diaChiLienLac": null, "soDT": "0367645612", "tinhTrang": 1, "maDonViCapMau": null, "traLoiCauHoiId": 68, "dotLayMauId": 27661, "surveyQuestions": [ { "id": 1, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.848037", "notes": null }, { "id": 2, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849346", "notes": null }, { "id": 9, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849360", "notes": null }, { "id": 3, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849365", "notes": null }, { "id": 10, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849366", "notes": null }, { "id": 4, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849367", "notes": null }, { "id": 11, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849368", "notes": null }, { "id": 12, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849369", "notes": null }, { "id": 5, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849370", "notes": null }, { "id": 13, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849371", "notes": null }, { "id": 6, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849372", "notes": null }, { "id": 14, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849373", "notes": null }, { "id": 15, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849374", "notes": null }, { "id": 7, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849375", "notes": null }, { "id": 8, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849375", "notes": null }, { "id": 16, "surveyQuestionId": null, "yes": null, "no": true, "onDate": "2024-11-24T17:12:37.849376", "notes": null } ] }''');
  //     var item = RegisterDonationBloodHistoryResponse.fromJson(json);
  //     var data = jsonEncode(item.toMapQrCode());
  //     log(data);
  //     AppUtils.instance.showQrCodeImage(data);
  //   });
  // }

  void autoGotoHomePage(BuildContext context) {
    if (appCenter.backendProvider.isAuthenticated) {
      try {
        Get.offAllNamed(Routes.appPage);
      } catch (e, s) {
        // TODO
        log("autoGotoHomePage()", error: e, stackTrace: s);
      }
    }
  }

  ///00000000-5783-1f4a-0000-00004578bd37
  Future<void> login(
      {required String username,
      required String password,
      required BuildContext context}) async {
    try {
      FocusScope.of(context).requestFocus(FocusNode());
      if (username.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.loginUsernameRequired.translate(context));
        return;
      }
      if (password.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.loginPasswordRequired.translate(context));
        return;
      }
      AppUtils.instance.showLoading();
      final isAuthenticated =
          await backendProvider.login(username: username, password: password);
      if (isAuthenticated != null) {
        // Lưu tokens vào secure storage để dùng cho biometric login (tách biệt, không ảnh hưởng flow hiện tại)
        try {
          await saveTokensToSecureStorage(
            authentication: isAuthenticated,
            refreshToken: null,
          );
        } catch (e) {
          log("Error saving tokens to secure storage: $e");
          // Không throw error, vì đây là tính năng bổ sung, không ảnh hưởng login bằng text
        }
        
        // emit(state.copyWith(isAuthenticated: true));
        setUserName();
        autoGotoHomePage(context);
      } else {
        // emit(state.copyWith(isAuthenticated: false));
        AppUtils.instance.showError(AppLocale.loginFail.translate(context));
      }
    } catch (e, t) {
      log("login()", error: e, stackTrace: t);
      AppUtils.instance.showError("$e");
    }
    AppUtils.instance.hideLoading();
  }
}
