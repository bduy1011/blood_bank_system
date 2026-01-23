import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../app/config/routes.dart';
import '../../../base/base_view/base_view.dart';
import '../../../core/localization/app_locale.dart';
import '../../../utils/app_utils.dart';
import '../../../utils/secure_token_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ChangePasswordController extends BaseModelStateful {
  final TextEditingController oldPasswordController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  final TextEditingController confirmPasswordController =
      TextEditingController();
  final SecureTokenService _tokenService = SecureTokenService();

  @override
  void onBack() {
    // TODO: implement onBack
  }

  @override
  void onTapRightMenu() {
    // TODO: implement onTapRightMenu
  }
  @override
  Future<void> onInit() {
    // TODO: implement onInit

    return super.onInit();
  }

  @override
  Future<void> onClose() async {
    // Implement your hide dispose indicator logic here

    confirmPasswordController.dispose();
    passwordController.dispose();
    oldPasswordController.dispose();

    super.onClose();
  }

  void autoGotoHomePage() {
    if (appCenter.backendProvider.isAuthenticated) {
      try {
        Get.offAllNamed(Routes.appPage);
      } catch (e, s) {
        // TODO
        log("autoGotoHomePage()", error: e, stackTrace: s);
      }
    }
  }

  Future<void> changePassword() async {
    var password = passwordController.text;
    var confirmPassword = confirmPasswordController.text;
    var oldPassword = oldPasswordController.text;

    if (oldPassword.trim().isEmpty) {
      AppUtils.instance.showMessage(AppLocale.changePasswordOldPasswordRequired.translate(Get.context!), context: Get.context);
      return;
    }
    if (password.trim().isEmpty) {
      AppUtils.instance.showMessage(AppLocale.registerPasswordRequired.translate(Get.context!), context: Get.context);
      return;
    }
    if (password.trim() != confirmPassword.trim()) {
      AppUtils.instance.showMessage(AppLocale.registerPasswordNotMatch.translate(Get.context!), context: Get.context);
      return;
    }

    ///
    try {
      var body = {
        "userCode": appCenter.authentication?.userCode,
        "oldPassword": oldPassword,
        "newPassword": password,
        "confirmedNewPassword": password,
      };
      showLoading();
      var response = await backendProvider.changePassword(
        body: body,
      );
      hideLoading();
      if (response.status == 200) {
        // await appCenter.backendProvider.saveAuthentication(authenticated!);
        // autoGotoHomePage();

        // Lưu lại mật khẩu mới (secure storage) sau khi đổi mật khẩu thành công
        // Chỉ lưu nếu đang bật "Ghi nhớ mật khẩu"
        try {
          final prefs = await SharedPreferences.getInstance();
          final remember = prefs.getBool("rememberPassword") ?? true;
          if (remember) {
            final username = appCenter.authentication?.userCode;
            if (username != null && username.trim().isNotEmpty) {
              await _tokenService.saveLoginCredentials(
                username: username,
                password: password,
              );
            }
          }
        } catch (e) {
          log("[ChangePasswordController] saveLoginCredentials error: $e");
        }

        await AppUtils.instance.showMessage(AppLocale.changePasswordSuccess.translate(Get.context!), context: Get.context);
        Get.back();
        return;
      }
      AppUtils.instance.showMessage(
        "${AppLocale.changePasswordFailed.translate(Get.context!)}\n${response.message}",
        context: Get.context,
      );
    } catch (e, t) {
      log("setPassword()", error: e, stackTrace: t);
      // TODO
      AppUtils.instance.showMessage(AppLocale.changePasswordFailed.translate(Get.context!), context: Get.context);
    }
    hideLoading();
  }
}
