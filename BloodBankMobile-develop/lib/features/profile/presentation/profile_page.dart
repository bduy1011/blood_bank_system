import 'package:blood_donation/app/theme/colors.dart';
import 'package:blood_donation/base/base_view/base_view_stateful.dart';
import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:blood_donation/utils/widget/spacer_widget.dart';
import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:get_it/get_it.dart';
import 'package:get/route_manager.dart';

import '../../../app/app_util/app_center.dart';
import '../../../utils/phone_number_formater.dart';
import '../controller/profile_controller.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState
    extends BaseViewStateful<ProfilePage, ProfileController> {
  @override
  ProfileController dependencyController() {
    // TODO: implement dependencyController
    return ProfileController();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        FocusScope.of(context).requestFocus(FocusNode());
      },
      child: Container(
        decoration: const BoxDecoration(
          gradient: LinearGradient(
            colors: [
              Color(0xFFB22C2D),
              Color.fromARGB(255, 240, 88, 88),
            ],
            begin: Alignment.topLeft,
            end: Alignment.centerRight,
          ),
        ),
        child: Scaffold(
          backgroundColor: Colors.transparent,
          resizeToAvoidBottomInset: true,
          appBar: AppBar(
            backgroundColor: Colors.transparent,
            centerTitle: true,
            leading: IconButton(
                onPressed: () {
                  Get.back();
                },
                icon: const Icon(
                  Icons.arrow_back_rounded,
                  color: Colors.white,
                )),
            title: Text(
              AppLocale.accountInformation.translate(context),
              style: context.myTheme.textThemeT1.title
                  .copyWith(color: Colors.white),
            ),
          ),
          body: Container(
            alignment: Alignment.topLeft,
            decoration: const BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.only(
                topLeft: Radius.circular(30),
                topRight: Radius.circular(30),
              ),
            ),
            child: Stack(
              children: [
                Positioned.fill(
                  left: 0,
                  right: 0,
                  top: 0,
                  bottom: 0,
                  child: SingleChildScrollView(
                    physics: const AlwaysScrollableScrollPhysics(),
                    child: Padding(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 16, vertical: 10),
                      child: Column(
                        children: [
                          const VSpacing(
                            spacing: 40,
                          ),

                          GestureDetector(
                            onTap: () => controller.pickAvatar(context),
                            child: _buildAvatar(context),
                          ),
                          if (controller.note != null) ...[
                            const VSpacing(
                              spacing: 30,
                            ),
                            Text(
                              controller.note ?? "",
                              style: context.myTheme.textThemeT1.title
                                  .copyWith(color: Colors.black87),
                              textAlign: TextAlign.center,
                            ),
                          ],
                          const VSpacing(
                            spacing: 30,
                          ),
                          _buildTextField(
                              controller: controller.fullnameController,
                              labelText: AppLocale.fullname.translate(context),
                              enabled: true),
                          const VSpacing(spacing: 16),

                          // _buildYearPicker(
                          //     controller: controller.birthYearController,
                          //     labelText: AppLocale.birthYear.translate(context)),
                          // const VSpacing(spacing: 16),
                          _buildTextField(
                              controller: controller.idCardController,
                              labelText: AppLocale.idCard.translate(context),
                              enabled: true,
                              keyboardType: TextInputType.number,
                              inputFormatters: [
                                // IdCardFormatter(),
                                LengthLimitingTextInputFormatter(12),
                              ]),
                          const VSpacing(spacing: 16),
                          _buildTextField(
                              controller: controller.phoneNumberController,
                              labelText:
                                  AppLocale.phoneNumber.translate(context),
                              enabled: true,
                              keyboardType: TextInputType.number,
                              inputFormatters: [
                                PhoneNumberFormatter(),
                                LengthLimitingTextInputFormatter(12),
                              ]),

                          const VSpacing(spacing: 30),
                          buildButton(context),
                        ],
                      ),
                    ),
                  ),
                ),
                buildQrScanButton(context),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget buildQrScanButton(BuildContext context) {
    var bottom = MediaQuery.of(context).viewInsets.bottom;

    ///
    return Positioned(
      bottom: 0,
      left: 0,
      right: 0,
      child: bottom > 0
          ? const SizedBox.shrink()
          : GestureDetector(
              onTap: () {
                ///
                controller.scanQRCode();
              },
              child: Column(
                children: [
                  SizedBox(
                    height: 50,
                    width: 50,
                    child: Stack(
                      children: [
                        SizedBox(
                          height: 50,
                          width: 50,
                          child: Image.asset(
                            'assets/icons/ic_qrcode.png',
                            fit: BoxFit.cover,
                          ),
                        ),
                        Align(
                          alignment: Alignment.center,
                          child: SizedBox(
                            height: 25,
                            width: 25,
                            child: Image.asset(
                              "assets/icons/app_icon_circle.png",
                              fit: BoxFit.cover,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(
                    height: 10,
                  ),
                  Text(
                    AppLocale.scanIdCardToGetInfo.translate(context),
                    style: context.myTheme.textThemeT1.title.copyWith(
                      fontWeight: FontWeight.w700,
                      color: AppColor.mainColor,
                      fontSize: 18,
                      decoration: TextDecoration.underline,
                      decorationColor: AppColor.mainColor,
                    ),
                  ),
                  SizedBox(
                    height: Get.mediaQuery.padding.bottom > 0
                        ? Get.mediaQuery.padding.bottom
                        : 30,
                  )
                ],
              ),
            ),
    );
  }

  Widget buildButton(BuildContext context) {
    return ElevatedButton(
      onPressed: () {
        controller.updateProfile(context);
      },
      style: ElevatedButton.styleFrom(
        backgroundColor: const Color.fromARGB(255, 229, 59, 59),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(50),
        ),
        overlayColor: Colors.transparent,
        padding: const EdgeInsets.symmetric(
          vertical: 15,
          horizontal: 40,
        ),
      ),
      child: Text(
        AppLocale.updateInformation.translate(context),
        style: context.myTheme.textThemeT1.title.copyWith(color: Colors.white),
      ),
    );
  }

  Widget _buildYearPicker({
    required TextEditingController controller,
    required String labelText,
    bool isRequired = false,
  }) {
    return Container(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(8),
        boxShadow: [
          BoxShadow(
            blurRadius: 5,
            color: Colors.grey.withOpacity(0.3),
            blurStyle: BlurStyle.outer,
          )
        ],
      ),
      child: TextFormField(
        controller: controller,
        readOnly: true,
        decoration: InputDecoration(
          labelText: isRequired ? '$labelText *' : labelText,
          labelStyle: TextStyle(
            color: Colors.grey,
            fontWeight: isRequired ? FontWeight.bold : FontWeight.normal,
          ),
          filled: true,
          fillColor: Colors.white,
          border: OutlineInputBorder(
            borderSide: BorderSide.none,
            borderRadius: BorderRadius.circular(8.0),
          ),
        ),
        onTap: () async {
          final DateTime? pickedDate = await showDatePicker(
            context: context,
            initialDate: DateTime.now(),
            firstDate: DateTime(1900),
            lastDate: DateTime.now(),
            initialDatePickerMode: DatePickerMode.year,
          );
          if (pickedDate != null) {
            controller.text = pickedDate.year.toString();
          }
        },
        validator: (value) {
          if (isRequired && (value == null || value.trim().isEmpty)) {
            return AppLocale.fieldRequired.translate(context);
          }
          return null;
        },
      ),
    );
  }

  Widget _buildAvatar(BuildContext context) {
    const size = 120.0;
    if (controller.hasPendingAvatar && controller.pendingAvatarFile != null) {
      return ClipOval(
        child: Image.file(
          controller.pendingAvatarFile!,
          width: size,
          height: size,
          fit: BoxFit.cover,
        ),
      );
    }
    final url = controller.avatarDisplayUrl ?? controller.avatarUrl;
    final token = GetIt.instance<AppCenter>().authentication?.accessToken;
    if (url != null && url.isNotEmpty) {
      return ClipOval(
        child: CachedNetworkImage(
          imageUrl: url,
          width: size,
          height: size,
          fit: BoxFit.cover,
          httpHeaders: token != null && token.isNotEmpty
              ? {'Authorization': 'Bearer $token'}
              : null,
          placeholder: (_, __) => const SizedBox(
            width: size,
            height: size,
            child: Center(child: CircularProgressIndicator()),
          ),
          errorWidget: (_, __, ___) => Image.asset(
            "assets/icons/app_icon_oval.png",
            width: size,
            height: size,
            fit: BoxFit.contain,
          ),
        ),
      );
    }
    return Image.asset(
      "assets/icons/app_icon_oval.png",
      width: size,
    );
  }

  Widget _buildTextField(
      {required TextEditingController controller,
      required String labelText,
      bool isRequired = false,
      bool password = false,
      bool enabled = true,
      TextInputType? keyboardType,
      List<TextInputFormatter>? inputFormatters}) {
    return Container(
      decoration:
          BoxDecoration(borderRadius: BorderRadius.circular(8), boxShadow: [
        BoxShadow(
            blurRadius: 5,
            color: Colors.grey.withOpacity(0.3),
            blurStyle: BlurStyle.outer)
      ]),
      child: TextFormField(
        controller: controller,
        obscureText: password,
        enabled: enabled,
        keyboardType: keyboardType,
        maxLengthEnforcement: MaxLengthEnforcement.none,
        decoration: InputDecoration(
          labelText: isRequired ? '$labelText*' : labelText,
          labelStyle: context.myTheme.textThemeT1.body.copyWith(
            color: Colors.grey,
            fontWeight: isRequired ? FontWeight.bold : FontWeight.normal,
          ),
          hintStyle: const TextStyle(color: Colors.grey),
          filled: true,
          fillColor: Colors.white,
          border: OutlineInputBorder(
            borderSide: BorderSide.none,
            borderRadius: BorderRadius.circular(8.0),
          ),
        ),
        inputFormatters: inputFormatters,
      ),
    );
  }
}
