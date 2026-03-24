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
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:get/get.dart';
import 'package:get_it/get_it.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../app/config/routes.dart';

class LoginController extends BaseModelStateful {
  void changeLanguage(AppLanguage language) {
    localization.translate(language.languageCode);
  }

  ///
  final TextEditingController usernameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  SharedPreferences? prefs;
  bool rememberPassword = true;

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
    await initRememberPassword();
    await initUserName();
    if (rememberPassword) {
      await initSavedPassword();
    }

    ///
    super.onInit();
  }

  Future<void> initRememberPassword() async {
    rememberPassword = prefs?.getBool("rememberPassword") ?? true;
  }

  Future<void> setRememberPassword(bool value) async {
    rememberPassword = value;
    await prefs?.setBool("rememberPassword", value);
    if (!value) {
      // Nếu tắt ghi nhớ → xóa toàn bộ mật khẩu đã lưu + clear field password
      try {
        await _tokenService.clearSavedPasswords();
      } catch (e) {
        log("clearSavedPasswords() error: $e");
      }
      // Không clear field ngay để user vẫn có thể bấm Login tiếp với mật khẩu đang nhập.
      // (Chỉ đảm bảo là mật khẩu không còn được lưu trong secure storage)
    } else {
      // Nếu bật lại → thử nạp password theo username hiện tại
      await initSavedPassword();
    }
  }

  Future<void> onUsernameChanged(String username) async {
    try {
      // Nếu không bật ghi nhớ -> username đổi thì clear password để tránh dùng nhầm
      if (!rememberPassword) {
        if (passwordController.text.isNotEmpty) {
          passwordController.text = "";
        }
        return;
      }

      final saved = await _tokenService.getLoginPassword(username: username);
      if (saved != null && saved.isNotEmpty) {
        passwordController.text = saved;
      } else if (passwordController.text.isNotEmpty) {
        // Nếu user đổi sang account khác mà không có password lưu, clear để tránh dùng nhầm
        passwordController.text = "";
      }
    } catch (e) {
      log("onUsernameChanged() error: $e");
    }
  }

  Future<void> initUserName() async {
    ///
    var userName = prefs?.getString("userName");
    if (userName?.isNotEmpty == true) {
      usernameController.text = userName ?? "";
    }
  }

  Future<void> initSavedPassword() async {
    try {
      final saved = await _tokenService.getLoginPassword(
        username: usernameController.text,
      );
      if (saved != null && saved.isNotEmpty) {
        passwordController.text = saved;
      }
    } catch (e) {
      // Không block UI nếu không đọc được secure storage
      log("initSavedPassword() error: $e");
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
  /// Returns true nếu lưu thành công, false nếu bị chặn (đã có account khác).
  /// Lưu tokens và toàn bộ Authentication vào secure storage sau khi login thành công.
  /// Nếu [forceSave] là true, sẽ ghi đè không cần hỏi (dùng sau khi user xác nhận).
  Future<bool> saveTokensToSecureStorage({
    required Authentication authentication,
    bool forceSave = false,
  }) async {
    try {
      if (!forceSave) {
        // Kiểm tra xem biometric đã gắn với account khác chưa
        final existingUserCode = await _tokenService.getUserCode();
        if (existingUserCode != null &&
            authentication.userCode != null &&
            existingUserCode != authentication.userCode) {
          log("Biometric already linked to another account: $existingUserCode. Need user confirmation.");
          return false; // Trả về false để UI biết cần hiện Dialog xác nhận
        }
      }

      if (authentication.accessToken != null &&
          authentication.accessToken!.isNotEmpty) {
        log("Saving authentication to secure storage: userCode=${authentication.userCode}");
        await _tokenService.saveAuthentication(authentication);
        return true;
      }
      return false;
    } catch (e) {
      log("saveTokensToSecureStorage error: $e");
      rethrow;
    }
  }

  /// Lấy thông tin User đang được liên kết Biometric
  Future<Map<String, String?>> getLinkedUserInfo() async {
    return await _tokenService.getUserInfo();
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
    log("═══════════════════════════════════════════════════════");
    log("[LoginController] loginWithBiometric() - START");
    log("═══════════════════════════════════════════════════════");
    try {
      final biometricService = BiometricAuthService();

      // Bước 1: Kiểm tra xem có tokens đã lưu không
      log("[LoginController] Bước 1: Kiểm tra tokens đã lưu...");
      final hasTokens = await hasStoredTokens();
      log("[LoginController] hasStoredTokens: $hasTokens");
      if (!hasTokens) {
        log("[LoginController] ❌ No tokens found - Cannot proceed with biometric login");
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
      log("[LoginController] Bước 2: Kiểm tra token có hết hạn...");
      final isExpired = await _tokenService.isAccessTokenExpired();
      log("[LoginController] isAccessTokenExpired: $isExpired");
      if (isExpired) {
        log("[LoginController] ❌ Token expired. Cannot use biometric login. User must login with username/password.");
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
      log("[LoginController] Bước 3: Lấy thông tin user đã lưu...");
      final userInfo = await _tokenService.getUserInfo();
      final savedUserCode = userInfo['userCode'];
      final savedUserName = userInfo['userName'];
      log("[LoginController] ✓ Saved user info - UserCode: $savedUserCode, UserName: $savedUserName");

      // Bước 4: Xác thực bằng biometric (chỉ khi token chưa hết hạn)
      log("[LoginController] Bước 4: Xác thực bằng biometric...");
      String reason = AppLocale.biometricAuthReason.translate(context);
      if (savedUserName != null || savedUserCode != null) {
        final userDisplay = savedUserName != null 
            ? (savedUserCode != null ? "$savedUserName ($savedUserCode)" : savedUserName)
            : savedUserCode ?? '';
        reason = AppLocale.biometricLoginWithAccount.translate(context)
            .replaceAll('{account}', userDisplay)
            .replaceAll('{reason}', reason);
      }
      log("[LoginController] Biometric reason: $reason");
      
      log("[LoginController] Calling biometricService.authenticate()...");
      final didAuthenticate = await biometricService.authenticate(
        reason: reason,
        context: context,
      );
      log("[LoginController] didAuthenticate result: $didAuthenticate");

      if (!didAuthenticate) {
        log("[LoginController] ❌ Biometric authentication failed or cancelled by user");
        return; // User cancel hoặc fail
      }

      log("[LoginController] ✓ Biometric authentication successful");
      AppUtils.instance.showLoading();

      // Bước 5: Lấy Authentication object từ secure storage (đã được khóa bằng biometric)
      log("[LoginController] Bước 5: Lấy Authentication từ secure storage...");
      log("[LoginController] Getting authentication from secure storage (unlocked by biometric)...");
      Authentication? savedAuth = await _tokenService.getAuthentication();
      
      if (savedAuth == null) {
        log("[LoginController] ❌ No authentication found in secure storage");
        AppUtils.instance.hideLoading();
        if (context.mounted) {
          AppUtils.instance.showToast(
              AppLocale.biometricAuthNotFound.translate(context));
        }
        return;
      }
      
      log("[LoginController] ✓ Authentication retrieved: userCode=${savedAuth.userCode}, name=${savedAuth.name}");

      // Bước 6: Kiểm tra lại token (đảm bảo token vẫn còn hợp lệ)
      log("[LoginController] Bước 6: Kiểm tra lại access token...");
      final accessToken = savedAuth.accessToken ?? '';
      log("[LoginController] accessToken length: ${accessToken.length}");
      if (accessToken.isEmpty) {
        log("[LoginController] ❌ Access token is empty");
        AppUtils.instance.hideLoading();
        if (context.mounted) {
          AppUtils.instance.showToast(
              AppLocale.biometricTokenInvalid.translate(context));
        }
        await clearStoredTokens();
        return;
      }
      
      // Kiểm tra token có hết hạn không (double check để đảm bảo)
      try {
        final isExpired = JwtDecoder.isExpired(accessToken);
        log("[LoginController] Token expiration check (double check): isExpired=$isExpired");
        if (isExpired) {
          log("[LoginController] ❌ Token has expired (JWT expiration check). Cannot use biometric login.");
          await clearStoredTokens();
          AppUtils.instance.hideLoading();
          if (context.mounted) {
            await AppUtils.instance.showMessage(
              AppLocale.biometricSessionExpired.translate(context),
              context: context,
            );
          }
          return;
        }
      } catch (e) {
        log("[LoginController] ⚠️ Error checking token expiration (JWT): $e - Will continue with token");
        // Nếu không parse được JWT, vẫn tiếp tục (có thể token không phải JWT)
      }

      // Bước 7: Set authentication vào localStorage và BackendProvider
      // KHÔNG refresh token ở đây vì:
      // 1. Token đã được kiểm tra ở bước 2 là chưa hết hạn
      // 2. Refresh token có thể fail (401) và HeaderInterceptor sẽ xóa authentication
      // 3. Token từ secure storage đã đủ để sử dụng (chưa hết hạn)
      // Refresh token sẽ được xử lý tự động bởi HeaderInterceptor khi token thực sự hết hạn
      log("[LoginController] Bước 7: Set authentication vào localStorage...");
      await _appCenter.localStorage
          .saveAuthentication(authentication: savedAuth);
      _appCenter.setAuthentication(savedAuth);
      _backendProvider.notifyAuthentication(isAuthenticated: true);
      log("[LoginController] ✓ Authentication set in localStorage and BackendProvider");
      
      // Bước 8: Đảm bảo BackendProvider được notify và isAuthenticated được set đúng
      log("[LoginController] Bước 8: Đảm bảo BackendProvider authentication state...");
      // Kiểm tra lại isAuthenticated để đảm bảo state đúng
      final isAuth = _backendProvider.isAuthenticated;
      log("[LoginController] backendProvider.isAuthenticated: $isAuth");
      if (!isAuth) {
        // Nếu vẫn chưa authenticated, thử set lại
        log("[LoginController] ⚠️ isAuthenticated is false, setting again...");
        _backendProvider.notifyAuthentication(isAuthenticated: true);
      }
      
      // Đảm bảo token đã được set trong localStorage
      final hasLocalStorageAuth = _appCenter.localStorage.authentication?.accessToken?.isNotEmpty == true;
      log("[LoginController] localStorage.hasAuthentication: $hasLocalStorageAuth");
      
      if (!hasLocalStorageAuth) {
        log("[LoginController] ⚠️ localStorage authentication missing, re-saving...");
        await _appCenter.localStorage
            .saveAuthentication(authentication: savedAuth);
      }
      
      log("[LoginController] ✓ Authentication set successfully from secure storage");
      
      if (!context.mounted) {
        log("Context is not mounted, cannot show UI");
        return;
      }
      
      AppUtils.instance.hideLoading();
      AppUtils.instance
          .showToast(AppLocale.biometricAuthSuccess.translate(context));

      // Vào app - dùng offAllNamed để xóa login page khỏi stack
      log("[LoginController] Navigating to home page...");
      // Đợi một chút để đảm bảo state được update đầy đủ
      await Future.delayed(const Duration(milliseconds: 100));
      autoGotoHomePage(context);
      log("═══════════════════════════════════════════════════════");
      log("[LoginController] loginWithBiometric() - SUCCESS ✓");
      log("═══════════════════════════════════════════════════════");
    } catch (e, t) {
      log("═══════════════════════════════════════════════════════");
      log("[LoginController] ❌ loginWithBiometric() ERROR");
      log("═══════════════════════════════════════════════════════");
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
        // 1. Ẩn loading trước khi xử lý các Dialog (nếu có) hoặc chuyển trang
        AppUtils.instance.hideLoading();

        // 2. Lưu mật khẩu (secure storage) sau khi login thành công (chỉ khi bật "Ghi nhớ mật khẩu")
        if (rememberPassword) {
          try {
            await _tokenService.saveLoginCredentials(
              username: username,
              password: password,
            );
            // Nếu hệ thống dùng userCode nội bộ (khác username nhập), lưu thêm theo userCode
            final userCode = isAuthenticated.userCode;
            if (userCode != null &&
                userCode.trim().isNotEmpty &&
                userCode.trim() != username.trim()) {
              await _tokenService.saveLoginCredentials(
                username: userCode,
                password: password,
              );
            }
          } catch (e) {
            log("[LoginController] saveLoginCredentials error: $e");
          }
        }

        // 3. Lưu tokens vào secure storage để dùng cho biometric login
        try {
          log("[LoginController] Checking biometric binding...");
          final linkedInfo = await getLinkedUserInfo();
          final linkedUserCode = linkedInfo['userCode'];

          if (linkedUserCode != null &&
              linkedUserCode != isAuthenticated.userCode) {
            // Trường hợp: Tài khoản login khác với tài khoản đang giữ Vân tay
            log("[LoginController] Identity mismatch: $linkedUserCode vs ${isAuthenticated.userCode}");
            if (context.mounted) {
              final confirm = await _showChangeBiometricDialog(
                context,
                oldUser: linkedInfo['userName'] ?? linkedUserCode,
                newUser: isAuthenticated.name ?? isAuthenticated.userCode ?? '',
              );

              if (confirm == true) {
                await saveTokensToSecureStorage(
                  authentication: isAuthenticated,
                  forceSave: true,
                );
                if (context.mounted) {
                  AppUtils.instance.showToast(
                      AppLocale.biometricAuthSuccess.translate(context));
                }
              }
            }
          } else {
            // Trường hợp: Trùng user hoặc máy chưa có liên kết nào -> Tự động cập nhật/lưu
            await saveTokensToSecureStorage(
              authentication: isAuthenticated,
              forceSave: true,
            );
          }
        } catch (e) {
          log("[LoginController] ❌ Error handling biometric binding: $e");
        }
        
        // 4. Các bước dọn dẹp cuối cùng và vào app
        setUserName();
        // Clear password field after successful login if rememberPassword is false
        if (!rememberPassword) {
          passwordController.clear();
        }
        autoGotoHomePage(context);
      } else {
        AppUtils.instance.hideLoading();
        AppUtils.instance.showError(AppLocale.loginFail.translate(context));
      }
    } catch (e, t) {
      AppUtils.instance.hideLoading();
      log("login()", error: e, stackTrace: t);
      AppUtils.instance.showError("$e");
    }
  }

  /// Dialog xác nhận chuyển đổi liên kết vân tay giữa 2 tài khoản
  Future<bool?> _showChangeBiometricDialog(
    BuildContext context, {
    required String oldUser,
    required String newUser,
  }) async {
    return showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: Text(AppLocale.biometricAuth.translate(context)),
        content: Text(
          "Thiết bị này đang liên kết vân tay với tài khoản '$oldUser'. "
          "Bạn có muốn chuyển liên kết sang tài khoản '$newUser' này không?",
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: Text(AppLocale.cancel.translate(context)),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text("Chuyển đổi", style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );
  }
}
