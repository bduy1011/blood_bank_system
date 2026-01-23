import 'dart:developer';

import 'package:blood_donation/app/theme/colors.dart';
import 'package:blood_donation/app/theme/icons.dart';
import 'package:blood_donation/base/base_view/base_view_stateful.dart';
import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/features/question_answer/presentation/question_answer_page.dart';
import 'package:blood_donation/utils/biometric_auth_service.dart';
import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:blood_donation/utils/widget/spacer_widget.dart';
import 'package:flutter/material.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:get/get.dart';

import '../../about/presentation/contact_page.dart';
import '../controller/login_controller.dart';
import 'forgot_password.dart';
import 'register_page.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends BaseViewStateful<LoginPage, LoginController> {
  @override
  LoginController dependencyController() {
    // TODO: implement dependencyController
    return LoginController();
  }

  bool _showPassword = false;
  bool _rememberPassword = true;
  bool _biometricAvailable = false;
  bool _hasStoredTokens = false;
  String _biometricTypeName = 'Biometric';
  final BiometricAuthService _biometricAuthService = BiometricAuthService();

  @override
  void initState() {
    super.initState();
    _checkBiometricAvailability();
    _checkStoredTokens();
    _initRememberPassword();
  }

  Future<void> _initRememberPassword() async {
    // Controller onInit là async, nên đợi 1 nhịp rồi lấy state hiện tại
    await Future.delayed(const Duration(milliseconds: 10));
    if (!mounted) return;
    setState(() {
      _rememberPassword = controller.rememberPassword;
    });
  }

  Future<void> _checkBiometricAvailability() async {
    final isAvailable = await _biometricAuthService.isAvailable();
    final biometricTypeName =
        await _biometricAuthService.getBiometricTypeName();
    log("[LoginPage] Biometric availability check - isAvailable: $isAvailable, type: $biometricTypeName");
    setState(() {
      _biometricAvailable = isAvailable;
      _biometricTypeName = biometricTypeName;
    });
  }

  /// Kiểm tra xem có tokens đã lưu trong secure storage không
  /// Chỉ hiển thị button biometric nếu có token đã lưu
  Future<void> _checkStoredTokens() async {
    final hasTokens = await controller.hasStoredTokens();
    log("[LoginPage] Stored tokens check - hasTokens: $hasTokens");
    log("[LoginPage] Button biometric will show: ${_biometricAvailable && hasTokens}");
    setState(() {
      _hasStoredTokens = hasTokens;
    });
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        FocusScope.of(context).requestFocus(FocusNode());
      },
      child: Scaffold(
        resizeToAvoidBottomInset: true,
        body: Stack(
          alignment: Alignment.bottomCenter,
          children: [
            Positioned.fill(
              child: Center(
                child: SingleChildScrollView(
                  padding: const EdgeInsets.symmetric(horizontal: 30),
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      const SizedBox(
                        height: 100,
                      ),
                      Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          buildIcon(),
                          const VSpacing(
                            spacing: 30,
                          ),
                          buildUserName(context),
                          const SizedBox(
                            height: 20,
                          ),
                          buildPassword(context),
                          const SizedBox(height: 6),
                          buildRememberPassword(context),
                          const VSpacing(
                            spacing: 20,
                          ),
                          buildButtonLogin(context),
                        ],
                      ),
                      const SizedBox(
                        height: 80,
                      ),
                    ],
                  ),
                ),
              ),
            ),
            buildBottomSection(context),
          ],
        ),
      ),
    );
  }

  Positioned buildBottomSection(BuildContext context) {
    var bottom = MediaQuery.of(context).viewInsets.bottom;
    return Positioned(
      bottom: -20,
      left: 0,
      right: 0,
      child: bottom > 0
          ? const SizedBox.shrink()
          : Image.asset(
              AppImages.imgFooter,
              height: 100,
              fit: BoxFit.cover,
            ),
    );
  }

  Widget buildAction(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.start,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Row(
          children: [
            buildActionRegister(context),
            const Spacer(),
            buildActionQA(context),
            const Spacer(),
            buildActionContact(context),
          ],
        ),
        buildActionForgotPassword(context),
      ],
    );
  }

  TextButton buildActionForgotPassword(BuildContext context) {
    return TextButton(
      onPressed: () {
        Get.to(const ForgotPasswordPage());
        // Get.to(() => const RegisterPagePhoneNumber());
      },
      style: TextButton.styleFrom(
        minimumSize: Size.zero,
        // padding: EdgeInsets.zero,
        // backgroundColor: Colors.red,
      ),
      child: Text(
        AppLocale.forgotPassword.translate(context),
        style: context.myTheme.textThemeT1.title
            .copyWith(color: AppColor.grey, fontSize: 16),
      ),
    );
  }

  TextButton buildActionRegister(BuildContext context) {
    return TextButton(
      onPressed: () async {
        var rs = await Get.to(() => const RegisterPage());
        if (rs == true) {
          await controller.initUserName();
          // Sau khi đăng ký thành công: clear password field để tránh hiển thị mật khẩu cũ
          controller.passwordController.clear();
          // Sau khi register xong: username có thể thay đổi → thử nạp password đã lưu (nếu bật ghi nhớ)
          await controller
              .onUsernameChanged(controller.usernameController.text);
        }
      },
      style: TextButton.styleFrom(
        minimumSize: Size.zero,
        // padding: EdgeInsets.zero,
        // backgroundColor: Colors.red,
      ),
      child: Text(
        AppLocale.signUp.translate(context),
        style: context.myTheme.textThemeT1.title
            .copyWith(color: AppColor.mainColor, fontSize: 16),
      ),
    );
  }

  TextButton buildActionQA(BuildContext context) {
    return TextButton(
      onPressed: () {
        Get.to(() => const QuestionAnswerPage());
        // Get.to(() => const RegisterPagePhoneNumber());
      },
      style: TextButton.styleFrom(
        minimumSize: Size.zero,
        // padding: EdgeInsets.zero,
        // backgroundColor: Colors.red,
      ),
      child: Text(
        AppLocale.questionAndAnswer.translate(context),
        style: context.myTheme.textThemeT1.title
            .copyWith(color: AppColor.mainColor, fontSize: 16),
      ),
    );
  }

  TextButton buildActionContact(BuildContext context) {
    return TextButton(
      onPressed: () {
        Get.to(() => const ContactPage());
      },
      style: TextButton.styleFrom(
        minimumSize: Size.zero,
        // padding: EdgeInsets.zero,
        // backgroundColor: Colors.red,
      ),
      child: Text(
        AppLocale.contact.translate(context),
        style: context.myTheme.textThemeT1.title
            .copyWith(color: AppColor.mainColor, fontSize: 16),
      ),
    );
  }

  Widget buildButtonLogin(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.start,
      mainAxisSize: MainAxisSize.min,
      children: [
        Row(
          children: [
            Expanded(
              child: ElevatedButton(
                  onPressed: () {
                    controller.login(
                        username: controller.usernameController.text,
                        password: controller.passwordController.text,
                        context: context);
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color.fromARGB(255, 229, 59, 59),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(50),
                    ),
                    padding: const EdgeInsets.symmetric(
                      vertical: 15,
                      horizontal: 40,
                    ),
                  ),
                  child: Text(
                    AppLocale.login.translate(context),
                    style: context.myTheme.textThemeT1.title
                        .copyWith(color: Colors.white),
                  )),
            ),
            if (_biometricAvailable && _hasStoredTokens) ...[
              const SizedBox(width: 12),
              buildBiometricLoginButton(context),
            ],
          ],
        ),
        buildAction(context),
      ],
    );
  }

  Widget buildBiometricLoginButton(BuildContext context) {
    // Xác định icon dựa trên loại biometric
    IconData biometricIcon = Icons.fingerprint;
    if (_biometricTypeName.contains('Face ID') ||
        _biometricTypeName.contains('Face')) {
      biometricIcon = Icons.face;
    } else if (_biometricTypeName.contains('Vân tay') ||
        _biometricTypeName.contains('Fingerprint')) {
      biometricIcon = Icons.fingerprint;
    }

    return InkWell(
      onTap: () {
        log("[LoginPage] Biometric login button tapped");
        controller.loginWithBiometric(context);
      },
      borderRadius: BorderRadius.circular(26),
      child: Container(
        width: 52,
        height: 52,
        alignment: Alignment.center,
        decoration: BoxDecoration(
          shape: BoxShape.circle,
          color: Colors.white,
          border: Border.all(color: Colors.black.withOpacity(0.06)),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.06),
              blurRadius: 10,
              offset: const Offset(0, 4),
            ),
          ],
        ),
        child: Icon(
          biometricIcon,
          size: 34,
          color: const Color.fromARGB(255, 229, 59, 59),
        ),
      ),
    );
  }

  TextFormField buildPassword(BuildContext context) {
    return TextFormField(
      controller: controller.passwordController,
      obscureText: !_showPassword,
      obscuringCharacter: "*",
      decoration: InputDecoration(
        focusedBorder: const UnderlineInputBorder(),
        labelText: AppLocale.password.translate(context),
        floatingLabelStyle: context.myTheme.textThemeT1.title
            .copyWith(fontWeight: FontWeight.normal),
        labelStyle: context.myTheme.textThemeT1.title
            .copyWith(fontWeight: FontWeight.normal),
        suffixIcon: IconButton(
          onPressed: () {
            setState(() {
              _showPassword = !_showPassword;
            });
          },
          icon: Icon(
            !_showPassword ? Icons.visibility_off : Icons.visibility,
          ),
        ),
      ),
      style: context.myTheme.textThemeT1.body.copyWith(fontSize: 16),
      autovalidateMode: AutovalidateMode.onUserInteraction,
      textInputAction: TextInputAction.done,
      onFieldSubmitted: (v) {
        controller.login(
            username: controller.usernameController.text,
            password: controller.passwordController.text,
            context: context);
      },
      validator: FormBuilderValidators.compose(
        [
          FormBuilderValidators.required(
              errorText:
                  AppLocale.formRequiredPasswordError.translate(context)),
        ],
      ),
    );
  }

  Widget buildRememberPassword(BuildContext context) {
    return Row(
      children: [
        Checkbox(
          value: _rememberPassword,
          activeColor: const Color.fromARGB(255, 229, 59, 59),
          materialTapTargetSize: MaterialTapTargetSize.shrinkWrap,
          visualDensity: const VisualDensity(horizontal: -4, vertical: -4),
          onChanged: (v) async {
            final next = v ?? false;
            setState(() {
              _rememberPassword = next;
            });
            await controller.setRememberPassword(next);
          },
        ),
        const SizedBox(width: 6),
        Expanded(
          child: GestureDetector(
            onTap: () async {
              final next = !_rememberPassword;
              setState(() {
                _rememberPassword = next;
              });
              await controller.setRememberPassword(next);
            },
            child: Text(
              AppLocale.rememberPassword.translate(context),
              style: context.myTheme.textThemeT1.body.copyWith(
                fontSize: 14,
                color: AppColor.grey,
              ),
            ),
          ),
        ),
      ],
    );
  }

  TextFormField buildUserName(BuildContext context) {
    return TextFormField(
      controller: controller.usernameController,
      decoration: InputDecoration(
        focusedBorder: const UnderlineInputBorder(),
        labelText: AppLocale.username.translate(context),
        floatingLabelStyle: context.myTheme.textThemeT1.title
            .copyWith(fontWeight: FontWeight.normal),
        labelStyle: context.myTheme.textThemeT1.title.copyWith(
          fontWeight: FontWeight.normal,
        ),
      ),
      style: context.myTheme.textThemeT1.body.copyWith(fontSize: 16),
      autovalidateMode: AutovalidateMode.onUserInteraction,
      textInputAction: TextInputAction.next,
      onChanged: (v) {
        controller.onUsernameChanged(v);
      },
      validator: FormBuilderValidators.compose(
        [
          FormBuilderValidators.required(
              errorText:
                  AppLocale.formRequiredUsernameError.translate(context)),
        ],
      ),
    );
  }

  Row buildIcon() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Image.asset(
          "assets/icons/app_cr_icon.png",
          height: 120,
          fit: BoxFit.contain,
        ),
        // const SizedBox(
        //   width: 20,
        // ),
        // Image.asset(
        //   "assets/icons/app_icon_oval.png",
        //   height: 120,
        //   fit: BoxFit.contain,
        // ),
      ],
    );
  }
}

// class SocialAuthWidget extends StatelessWidget {
//   const SocialAuthWidget({super.key, required this.cubit});
//   final LoginCubit cubit;

//   @override
//   Widget build(BuildContext context) {
//     return Column(
//       mainAxisSize: MainAxisSize.min,
//       mainAxisAlignment: MainAxisAlignment.center,
//       children: [
//         GestureDetector(
//           onTap: () {
//             cubit.loginWithGoogle();
//           },
//           child: Container(
//             height: 50,
//             padding: const EdgeInsets.symmetric(horizontal: 20),
//             decoration: BoxDecoration(
//                 border: Border.all(width: 0.5, color: const Color(0xFF979696)),
//                 borderRadius: const BorderRadius.all(Radius.circular(100))),
//             child: Row(
//               mainAxisAlignment: MainAxisAlignment.center,
//               mainAxisSize: MainAxisSize.min,
//               children: [
//                 Image.asset(
//                   AppImages.google,
//                   height: 30,
//                   width: 30,
//                 ),
//                 const HSpacing(),
//                 Text(
//                   'Đăng nhập với Google',
//                   style: context.myTheme.textThemeT1.body,
//                 )
//               ],
//             ),
//           ),
//         ),
//       ],
//     );
//   }
// }
