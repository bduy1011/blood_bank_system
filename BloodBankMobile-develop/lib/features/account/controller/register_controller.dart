import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../base/base_view/base_view.dart';
import '../../../core/localization/app_locale.dart';
import '../../../models/citizen.dart';
import '../../../utils/app_utils.dart';
import '../../scan_qr_code/scan_qr_code_screen.dart';
import '../presentation/confirm_otp.dart';

class RegisterController extends BaseModelStateful {
  final TextEditingController phoneController = TextEditingController();
  //
  final TextEditingController fullNameRegisterController =
      TextEditingController();
  final TextEditingController usernameRegisterController =
      TextEditingController();
  final TextEditingController passwordRegisterController =
      TextEditingController();
  final TextEditingController confirmPasswordRegisterController =
      TextEditingController();
  SharedPreferences? prefs;
  @override
  Future<void> onClose() async {
    // Implement your hide dispose indicator logic here
    fullNameRegisterController.dispose();
    usernameRegisterController.dispose();
    confirmPasswordRegisterController.dispose();
    passwordRegisterController.dispose();

    phoneController.dispose();
    super.onClose();
  }

  @override
  Future<void> onInit() async {
    ///
    prefs = await SharedPreferences.getInstance();
    super.onInit();
  }

  Future<void> register(BuildContext context) async {
    ///
    try {
      FocusScope.of(context).requestFocus(FocusNode());
      var fullName = fullNameRegisterController.text.toUpperCase().trim();
      var userName = usernameRegisterController.text.trim().replaceAll(" ", "");
      var password = passwordRegisterController.text.trim();
      var confirmPassword = confirmPasswordRegisterController.text;

      if (fullName.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.registerFullNameRequired.translate(context));
        return;
      }
      if (fullName.trim().length < 6) {
        AppUtils.instance.showToast(AppLocale.registerFullNameInvalid.translate(context));
        return;
      }
      if (userName.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.registerUsernameRequired.translate(context));
        return;
      }
      if (!userName.isNum || !(userName.length == 12 || userName.length == 9)) {
        AppUtils.instance.showToast(AppLocale.invalidUsername.translate(context));
        return;
      }
      if (password.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.registerPasswordRequired.translate(context));
        return;
      }
      if (password.trim().length < 6) {
        AppUtils.instance.showToast(AppLocale.registerPasswordMinLength.translate(context));
        return;
      }
      if (password.trim() != confirmPassword.trim()) {
        AppUtils.instance.showToast(AppLocale.registerPasswordNotMatch.translate(context));
        return;
      }

      AppUtils.instance.showLoading();
      final isAuthenticated = await backendProvider.register(
          fullName: fullName, username: userName, password: password);
      if (isAuthenticated?.isEmpty == null) {
        // emit(state.copyWith(isAuthenticated: true));
        AppUtils.instance.hideLoading();
        await AppUtils.instance.showMessage(
          AppLocale.registerSuccess.translate(context),
          context: Get.context,
        );
        await setUserName(userName);
        Get.back(result: true);
      } else {
        // emit(state.copyWith(isAuthenticated: false));
        AppUtils.instance.showToast(
          "${AppLocale.registerFailed.translate(context)}\n$isAuthenticated"
        );
      }
    } catch (e, t) {
      log("register()", error: e, stackTrace: t);
      AppUtils.instance.showToast(AppLocale.registerFailed.translate(context));
    }
    AppUtils.instance.hideLoading();
  }

  Future<void> setUserName(String userName) async {
    ///
    try {
      await prefs?.setString("userName", userName);
    } catch (e) {
      // TODO
      print(e);
    }
  }

  Future<void> registerByPhoneNumber(BuildContext context) async {
    // Get.to(() => const ConfirmOtp());

    ///
    try {
      FocusScope.of(context).requestFocus(FocusNode());
      var fullName = fullNameRegisterController.text;
      var phoneNumber = phoneController.text.replaceAll(" ", "");

      if (fullName.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.registerFullNameAccountRequired.translate(context));
        return;
      }
      if (phoneNumber.trim().isEmpty) {
        AppUtils.instance.showToast(AppLocale.registerPhoneRequired.translate(context));
        return;
      }
      if (phoneNumber.trim().length != 10) {
        AppUtils.instance.showToast(AppLocale.registerPhoneInvalid.translate(context));
        return;
      }

      AppUtils.instance.showLoading();
      final isAuthenticated = await backendProvider.registerByPhoneNumber(
          fullName: fullName, phoneNumber: phoneNumber);
      AppUtils.instance.hideLoading();
      if (isAuthenticated == null) {
        // emit(state.copyWith(isAuthenticated: true));
        Get.to(() => const ConfirmOtp());
      } else {
        // emit(state.copyWith(isAuthenticated: false));
        AppUtils.instance.showToast(
          "${AppLocale.registerFailed.translate(context)}\n$isAuthenticated"
        );
      }
    } catch (e, t) {
      log("registerByPhoneNumber()", error: e, stackTrace: t);
      AppUtils.instance.showToast(AppLocale.registerFailed.translate(context));
    }
    AppUtils.instance.hideLoading();
  }

  Future<bool> scanQRCodeForRegistration(BuildContext context) async {
    try {
      var rs = await Get.to(
        () => ScanQrCodeScreen(
          title: AppLocale.scanQRCCCD.translate(context),
          onScan: (code) async {
            try {
              // Parse QR code thành Citizen model
              final citizen = Citizen.fromQRCode(code);

              // Validate thông tin
              if (!citizen.isValid()) {
                final errors = citizen.getValidationErrors();
                AppUtils.instance.showMessage(
                  errors.join("\n"),
                  context: Get.context,
                );
                return false;
              }

              usernameRegisterController.text = citizen.idCard;
              fullNameRegisterController.text = citizen.fullName;

              AppUtils.instance.showToast(AppLocale.scanQRSuccess.translate(context));

              return true;
            } catch (e) {
              log("parseQRCode()", error: e);
              AppUtils.instance.showToast(AppLocale.scanQRError.translate(context));
              return false;
            }
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
    } catch (e, t) {
      log("scanQRCodeForRegistration()", error: e, stackTrace: t);
      AppUtils.instance.showToast(AppLocale.scanQRError.translate(context));
      return false;
    }
  }
}
