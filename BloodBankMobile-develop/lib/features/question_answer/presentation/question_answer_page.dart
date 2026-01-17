import 'package:blood_donation/base/base_view/base_view_stateful.dart';
import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/features/question_answer/models/personality_group.dart';
import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:blood_donation/utils/widget/spacer_widget.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:get/route_manager.dart';
import 'package:photo_view/photo_view.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../../app/theme/icons.dart';
import '../../home/models/home_category.dart';
import '../controller/question_answer_controller.dart';

class QuestionAnswerPage extends StatefulWidget {
  const QuestionAnswerPage({super.key});

  @override
  State<QuestionAnswerPage> createState() => _QuestionAnswerPageState();
}

class _QuestionAnswerPageState
    extends BaseViewStateful<QuestionAnswerPage, QuestionAnswerController> {
  @override
  QuestionAnswerController dependencyController() {
    // TODO: implement dependencyController
    return QuestionAnswerController();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
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
          title: Hero(
            tag: HomeCategory.questionAndAnswer.name,
            child: Text(
              AppLocale.questionAndAnswer.translate(context),
              style: context.myTheme.textThemeT1.title
                  .copyWith(color: Colors.white),
            ),
          ),
        ),
        body: Container(
          height: context.screenHeight,
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 20),
          decoration: const BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(30), topRight: Radius.circular(30))),
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 10),
                child: Image.asset(
                  AppImages.imageQA,
                  height: 120,
                  width: Get.width,
                  fit: BoxFit.fitWidth,
                  alignment: Alignment.topCenter,
                ),
              ),
              const VSpacing(
                spacing: 10,
              ),
              Expanded(
                child: ListView.separated(
                    itemCount: PersonalityGroup.values.length,
                    itemBuilder: (context, index) {
                      return buildItem(index, context);
                    },
                    separatorBuilder: (context, index) {
                      return const VSpacing(
                        spacing: 10,
                      );
                    }),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget buildItem(int index, BuildContext context) {
    return InkWell(
      onTap: () => setState(() {
        controller.isExpandedList[index] = !controller.isExpandedList[index];
      }),
      child: Container(
        padding: const EdgeInsets.all(10),
        decoration: BoxDecoration(
          border: Border.all(
              width: 1, color: const Color.fromARGB(255, 194, 194, 194)),
          borderRadius: BorderRadius.circular(10),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              children: [
                Icon(
                  PersonalityGroup.values[index].icon,
                  color: Colors.red,
                ),
                const SizedBox(
                  width: 10,
                ),
                Expanded(
                  child: Text(
                    PersonalityGroup.values[index].title(context).toUpperCase(),
                    style: context.myTheme.textThemeT1.title.copyWith(
                        color: controller.isExpandedList[index]
                            ? const Color.fromARGB(255, 117, 13, 13)
                            : const Color.fromARGB(255, 0, 0, 0)),
                  ),
                ),
                Icon(
                  controller.isExpandedList[index]
                      ? Icons.keyboard_arrow_down_rounded
                      : Icons.keyboard_arrow_up_rounded,
                  color: Colors.black,
                )
              ],
            ),
            controller.isExpandedList[index]
                ? buildContentItem(context, PersonalityGroup.values[index])
                : const SizedBox()
          ],
        ),
      ),
    );
  }

  Widget buildContentItem(BuildContext context, PersonalityGroup value) {
    switch (value) {
      case PersonalityGroup.location:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: AppLocale.bloodTransfusionCenter.translate(context),
              style: context.myTheme.textThemeT1.title
                  .copyWith(height: 1.2, fontWeight: FontWeight.w700),
              children: [
                TextSpan(
                  text: "\n\n${AppLocale.address.translate(context)} ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text: AppLocale.bloodTransfusionCenterAddress.translate(context),
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n\n${AppLocale.phone.translate(context)} ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                    text: "",
                    style: context.myTheme.textThemeT1.body.copyWith(
                      height: 1.2,
                      fontSize: 16,
                      fontWeight: FontWeight.w400,
                    ),
                    children: [
                      TextSpan(
                        text: "028.38554137",
                        style: context.myTheme.textThemeT1.title.copyWith(
                          height: 1.2,
                          fontSize: 16,
                          fontWeight: FontWeight.w500,
                          color: const Color.fromARGB(255, 103, 27, 27),
                        ),
                        recognizer: TapGestureRecognizer()
                          ..onTap = () {
                            //callPhone("028.38554137");
                            launchUrl(Uri.parse("tel:02838554137"),
                                mode: LaunchMode.externalApplication);
                          },
                      ),
                      TextSpan(
                        text: " ${AppLocale.extension.translate(context)} 1162 ${AppLocale.or.translate(context)} ",
                        style: context.myTheme.textThemeT1.body.copyWith(
                          height: 1.2,
                          fontSize: 16,
                          fontWeight: FontWeight.w400,
                        ),
                      ),
                      TextSpan(
                        text: "028.39555885",
                        style: context.myTheme.textThemeT1.title.copyWith(
                          height: 1.2,
                          fontSize: 16,
                          fontWeight: FontWeight.w500,
                          color: const Color.fromARGB(255, 103, 27, 27),
                        ),
                        recognizer: TapGestureRecognizer()
                          ..onTap = () {
                            //callPhone("028.39555885");
                            launchUrl(Uri.parse("tel:02839555885"),
                                mode: LaunchMode.externalApplication);
                          },
                      ),
                      TextSpan(
                        text: " ${AppLocale.contactDuringBusinessHours.translate(context)}",
                        style: context.myTheme.textThemeT1.body.copyWith(
                          height: 1.2,
                          fontSize: 16,
                          fontWeight: FontWeight.w400,
                        ),
                      ),
                    ])
              ],
            ),
          ),
        );
      case PersonalityGroup.time:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: "${AppLocale.qaDonateBlood.translate(context)}: ",
              style: context.myTheme.textThemeT1.title.copyWith(
                height: 1.2,
                fontWeight: FontWeight.w700,
                color: const Color.fromARGB(255, 0, 38, 255),
                fontSize: 16,
              ),
              children: [
                TextSpan(
                  text: "${AppLocale.qaFromMondayToFriday.translate(context)}.",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n •  ${AppLocale.qaMorning.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                TextSpan(
                  text: "   7:00 - 10:30",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n •  ${AppLocale.qaAfternoon.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                TextSpan(
                  text: "13:00 - 15:30",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n\n${AppLocale.qaDonatePlatelets.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontWeight: FontWeight.w700,
                    color: const Color.fromARGB(255, 0, 38, 255),
                    fontSize: 16,
                  ),
                ),
                TextSpan(
                  text: "${AppLocale.qaFromMondayToFriday.translate(context)}.",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n •  ${AppLocale.qaMorning.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                TextSpan(
                  text: "7:00 - 9:00 ${AppLocale.qaMayStopEarly.translate(context)}",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n\n${AppLocale.qaWeekendDonation.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontWeight: FontWeight.w700,
                    color: const Color.fromARGB(255, 0, 38, 255),
                    fontSize: 16,
                  ),
                ),
                TextSpan(
                  text: "${AppLocale.qaSecondSunday.translate(context)}.",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n •  ${AppLocale.qaMorning.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                TextSpan(
                  text: "   7:00 - 10:30",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n •  ${AppLocale.qaAfternoon.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                TextSpan(
                  text: "13:00 - 15:30",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
              ],
            ),
          ),
        );
      case PersonalityGroup.condition:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: "• ${AppLocale.qaAge.translate(context)}: ",
              style: context.myTheme.textThemeT1.title.copyWith(
                height: 1.2,
                fontSize: 16,
                fontWeight: FontWeight.w400,
              ),
              children: [
                TextSpan(
                  text: "18-60",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text: "\n•  ${AppLocale.qaWeight.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "trên 45kg",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text: "\n•  ${AppLocale.qaDistanceBetweenDonations.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "12 tuần",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text: "\n•  ${AppLocale.qaDistanceBetweenPlatelets.translate(context)}: ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "14 ngày",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text:
                      "\n•  ${AppLocale.qaDiseases.translate(context)}: Không mắc các bệnh lây nhiễm qua đường truyền máu như ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "HIV/AIDS, viêm gan siêu vi B, C, Sốt rét, Giang mai. ",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text:
                      "Không mắc các bệnh tim, gan, thận, phổi, ung thư, tiểu đường, cao huyết áp, các bệnh về máu, v.v",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n•  ${AppLocale.qaMenstrualCycle.translate(context)} ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "7 ngày ",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
                TextSpan(
                  text: "${AppLocale.qaForFemale.translate(context)}",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "\n•  ${AppLocale.qaEatNormally.translate(context)} ",
                  style: context.myTheme.textThemeT1.title.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
                TextSpan(
                  text: "không thức khuya đêm trước hiến máu.",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w700,
                  ),
                ),
              ],
            ),
          ),
        );
      case PersonalityGroup.note:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: AppLocale.qaBringIdCard.translate(context),
              style: context.myTheme.textThemeT1.body.copyWith(
                height: 1.2,
                fontSize: 16,
                fontWeight: FontWeight.w400,
              ),
              children: [
                TextSpan(
                  text: "\n•  ${AppLocale.qaParkingLocation.translate(context)}",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                )
              ],
            ),
          ),
        );
      case PersonalityGroup.image:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: Column(
            children: [
              GestureDetector(
                onTap: () {
                  ///
                  Get.to(
                    () => Stack(
                      children: [
                        PhotoView(
                          imageProvider: const AssetImage(AppImages.toRoi1),
                          errorBuilder: (context, url, error) =>
                              const Icon(Icons.error),
                        ),
                        Positioned(
                          top: Get.mediaQuery.padding.top + 5,
                          left: 5,
                          child: IconButton(
                            onPressed: () {
                              Get.back();
                            },
                            icon: const Icon(
                              Icons.arrow_back_rounded,
                              color: Colors.white,
                            ),
                          ),
                        ),
                      ],
                    ),
                  );
                },
                child: Image.asset(
                  AppImages.toRoi1,
                  width: Get.width,
                  fit: BoxFit.contain,
                ),
              ),
              GestureDetector(
                onTap: () {
                  ///
                  Get.to(() => Stack(
                        children: [
                          PhotoView(
                            imageProvider: const AssetImage(AppImages.toRoi2),
                            errorBuilder: (context, url, error) =>
                                const Icon(Icons.error),
                          ),
                          Positioned(
                            top: Get.mediaQuery.padding.top + 5,
                            left: 5,
                            child: IconButton(
                              onPressed: () {
                                Get.back();
                              },
                              icon: const Icon(
                                Icons.arrow_back_rounded,
                                color: Colors.white,
                              ),
                            ),
                          ),
                        ],
                      ));
                },
                child: Image.asset(
                  AppImages.toRoi2,
                  width: Get.width,
                  fit: BoxFit.contain,
                ),
              ),
            ],
          ),
        );
      case PersonalityGroup.benefit:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: AppLocale.qaBenefitsContent.translate(context),
              style: context.myTheme.textThemeT1.body.copyWith(
                height: 1.2,
                fontSize: 16,
                fontWeight: FontWeight.w400,
              ),
              children: [],
            ),
          ),
        );
      case PersonalityGroup.testing:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: AppLocale.qaTestingContent.translate(context),
              style: context.myTheme.textThemeT1.body.copyWith(
                height: 1.2,
                fontSize: 16,
                fontWeight: FontWeight.w400,
              ),
              children: [
                TextSpan(
                  text: "\n•  ${AppLocale.qaTestingResult.translate(context)}",
                  style: context.myTheme.textThemeT1.body.copyWith(
                    height: 1.2,
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                )
              ],
            ),
          ),
        );
      case PersonalityGroup.cmnd:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaIdCardReason.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      case PersonalityGroup.contagious:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaCannotGetInfected.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      case PersonalityGroup.unwell:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaUnwellSymptoms.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      case PersonalityGroup.signs:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaSwellingInstructions.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      case PersonalityGroup.preparedonate:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaPrepareInstructions.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      case PersonalityGroup.abnormal:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaAbnormalInstructions.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      case PersonalityGroup.needblood:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: AppLocale.qaWhyNeedBloodReason.translate(context),
              style: context.myTheme.textThemeT1.body.copyWith(
                height: 1.2,
                fontSize: 16,
                fontWeight: FontWeight.w400,
              ),
              children: [],
            ),
          ),
        );
      case PersonalityGroup.health:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
              text: AppLocale.qaHealthContent.translate(context),
              style: context.myTheme.textThemeT1.body.copyWith(
                height: 1.2,
                fontSize: 16,
                fontWeight: FontWeight.w400,
              ),
              children: [],
            ),
          ),
        );
      case PersonalityGroup.delay:
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
          child: RichText(
            text: TextSpan(
                text: AppLocale.qaPostponeContent.translate(context),
                style: context.myTheme.textThemeT1.body.copyWith(
                  height: 1.2,
                  fontSize: 16,
                  fontWeight: FontWeight.w400,
                )),
          ),
        );
      default:
    }
    return const SizedBox();
  }
}
