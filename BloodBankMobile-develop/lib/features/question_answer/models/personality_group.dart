import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:flutter/material.dart';

enum PersonalityGroup {
  location,
  time,
  condition,
  note,
  image,
  benefit,
  testing,
  cmnd,
  contagious,
  unwell,
  signs,
  preparedonate,
  abnormal,
  needblood,
  health,
  delay;

  String title(BuildContext context) {
    switch (this) {
      case PersonalityGroup.location:
        return AppLocale.qaDonationLocation.translate(context);
      case PersonalityGroup.time:
        return AppLocale.qaDonationTime.translate(context);
      case PersonalityGroup.condition:
        return AppLocale.qaDonationConditions.translate(context);
      case PersonalityGroup.note:
        return AppLocale.qaDonationNotes.translate(context);
      case PersonalityGroup.image:
        return AppLocale.qaDonationHandbook.translate(context);
      case PersonalityGroup.benefit:
        return AppLocale.qaDonationBenefits.translate(context);
      case PersonalityGroup.testing:
        return AppLocale.qaBloodTesting.translate(context);
      case PersonalityGroup.cmnd:
        return AppLocale.qaWhyNeedIdCard.translate(context);
      case PersonalityGroup.contagious:
        return AppLocale.qaCanGetInfected.translate(context);
      case PersonalityGroup.unwell:
        return AppLocale.qaFeelUnwell.translate(context);
      case PersonalityGroup.signs:
        return AppLocale.qaSwellingSigns.translate(context);
      case PersonalityGroup.preparedonate:
        return AppLocale.qaPrepareForDonation.translate(context);
      case PersonalityGroup.abnormal:
        return AppLocale.qaAbnormalSituation.translate(context);
      case PersonalityGroup.needblood:
        return AppLocale.qaWhyNeedBlood.translate(context);
      case PersonalityGroup.health:
        return AppLocale.qaIsDonationHarmful.translate(context);
      case PersonalityGroup.delay:
        return AppLocale.qaPostponeDonation.translate(context);
    }
  }

  IconData? get icon {
    switch (this) {
      case PersonalityGroup.location:
        return Icons.location_on_outlined;
      case PersonalityGroup.time:
        return Icons.access_time_rounded;
      case PersonalityGroup.condition:
        return Icons.add_task_rounded;
      case PersonalityGroup.note:
        return Icons.not_listed_location_outlined;
      case PersonalityGroup.image:
        return Icons.more_outlined;
      case PersonalityGroup.benefit:
        return Icons.volunteer_activism_outlined;
      case PersonalityGroup.testing:
        return Icons.candlestick_chart_outlined;
      case PersonalityGroup.cmnd:
        return Icons.photo_camera_front_outlined;
      case PersonalityGroup.contagious:
        return Icons.ac_unit_sharp;
      case PersonalityGroup.unwell:
        return Icons.announcement_outlined;
      case PersonalityGroup.signs:
        return Icons.personal_injury_outlined;
      case PersonalityGroup.preparedonate:
        return Icons.offline_bolt_outlined;
      case PersonalityGroup.abnormal:
        return Icons.warning_amber;
      case PersonalityGroup.needblood:
        return Icons.model_training_sharp;
      case PersonalityGroup.health:
        return Icons.health_and_safety_outlined;
      case PersonalityGroup.delay:
        return Icons.back_hand_outlined;
    }
  }
}
