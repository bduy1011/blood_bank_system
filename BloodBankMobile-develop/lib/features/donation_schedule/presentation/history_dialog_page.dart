import 'package:blood_donation/base/base_view/base_view_stateful.dart';
import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:blood_donation/utils/extension/datetime_extension.dart';
import 'package:blood_donation/utils/widget/spacer_widget.dart';
import 'package:dotted_border/dotted_border.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';

import '../../../app/app_util/enum.dart';
import '../../../models/blood_donation_event.dart';
import '../../../utils/app_utils.dart';
import '../../../utils/redirect_to_google_utils.dart';
import '../../components/blood_search_bar.dart';
import '../controller/donation_schedule_controller.dart';

class HistoryDialogPage extends StatefulWidget {
  const HistoryDialogPage({super.key});

  @override
  State<HistoryDialogPage> createState() => _HistoryDialogPageState();
}

class _HistoryDialogPageState
    extends BaseViewStateful<HistoryDialogPage, DonationScheduleController> {
  @override
  DonationScheduleController dependencyController() {
    // TODO: implement dependencyController
    return DonationScheduleController();
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
          automaticallyImplyLeading: false,
          leading: IconButton(
              onPressed: () {
                Get.back();
              },
              icon: const Icon(
                Icons.arrow_back_rounded,
                color: Colors.white,
              )),
          centerTitle: true,
          title: Text(
            AppLocale.selectBloodDonationSchedule.translate(context),
            style:
                context.myTheme.textThemeT1.title.copyWith(color: Colors.white),
          ),
          backgroundColor: Colors.transparent,
          surfaceTintColor: context.myTheme.colorScheme.scaffoldBackgroundColor,
          actions: [
            IconButton(
                onPressed: () async {
                  controller.onFilter(context);
                },
                icon: SvgPicture.asset(
                  "assets/icons/ic_location_filter.svg",
                  width: 20,
                  color: Colors.white,
                ))
          ],
        ),
        body: Container(
          decoration: const BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(30), topRight: Radius.circular(30))),
          child: Column(
            children: [
              const SizedBox(height: 15),
              BloodSearchBar(
                hintText: "${AppLocale.search.translate(context)}...",
                onChange: (text) {
                  ///
                  controller.onSearch(text ?? "");
                },
              ),
              Expanded(
                  child: controller.bloodDonationEventsSearch.isEmpty
                      ? buildEmptyLayout()
                      : ListView.separated(
                          itemCount:
                              controller.bloodDonationEventsSearch.length,
                          padding: const EdgeInsets.symmetric(
                                  vertical: 16, horizontal: 16)
                              .copyWith(bottom: Get.mediaQuery.padding.bottom),
                          itemBuilder: (context, index) {
                            final item =
                                controller.bloodDonationEventsSearch[index];
                            return buildItemHistory(item, context);
                          },
                          separatorBuilder: (context, index) {
                            return const VSpacing(
                              spacing: 20,
                            );
                          },
                        ))
            ],
          ),
        ),
      ),
    );
  }

  Widget buildItemHistory(BloodDonationEvent item, BuildContext context) {
    // Determine background color based on loaiMau
    Color backgroundColor;
    if (!item.isDuocDangKy!) {
      // Disabled/inactive items - gray
      backgroundColor = Colors.grey[200]!;
    } else if (item.loaiMau == LoaiMau.TieuCau.value) {
      // Platelet donation - orange/beige color
      backgroundColor = const Color(0xFFFFF4E6); // Light orange/beige
    } else {
      // Blood donation - pink color
      backgroundColor = const Color(0xFFFCE4EC); // Light pink
    }

    return GestureDetector(
      onTap: () {
        controller.backToDonationBlood(item);
      },
      child: Container(
        decoration: BoxDecoration(
            color: backgroundColor,
            borderRadius: BorderRadius.circular(14),
            boxShadow: [
              BoxShadow(
                  color: const Color.fromARGB(255, 90, 90, 90).withOpacity(0.6),
                  blurRadius: 3,
                  blurStyle: BlurStyle.outer)
            ]),
        child: buildItem(context, item),
      ),
    );
  }

  Widget buildItem(BuildContext context, BloodDonationEvent item) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        buildTextContent(item, context),
        const VSpacing(
          spacing: 5,
        ),
        buildItemTime(item, context),
        const VSpacing(
          spacing: 10,
        ),
        buildItemLocation(item, context),
      ],
    );
  }

  Widget buildTextContent(BloodDonationEvent item, BuildContext context) {
    // Determine text color based on loaiMau
    Color titleColor;
    Color bodyColor;

    if (!item.isDuocDangKy!) {
      // Disabled/inactive items
      titleColor = Colors.black38;
      bodyColor = Colors.black38;
    } else if (item.loaiMau == LoaiMau.TieuCau.value) {
      // Platelet donation - orange/brown color
      titleColor = const Color(0xFFD97706); // Orange/brown
      bodyColor = Colors.black87;
    } else {
      // Blood donation - dark red color
      titleColor = const Color(0xff5c0101); // Dark red
      bodyColor = Colors.black87;
    }

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            item.ten ?? "",
            style: context.myTheme.textThemeT1.title.copyWith(
              color: titleColor,
            ),
          ),
          Text(
            item.diaDiemToChuc ?? '',
            style: context.myTheme.textThemeT1.title.copyWith(
              color: titleColor,
            ),
          ),
          const VSpacing(
            spacing: 5,
          ),
          Text(
            "Địa chỉ: ${(item.tenXa?.trim().isNotEmpty == true) ? "${item.tenXa}, " : ""}${item.tenHuyen}, ${item.tenTinh}",
            style: context.myTheme.textThemeT1.body.copyWith(
              color: bodyColor,
            ),
          ),
        ],
      ),
    );
  }

  Path customPath = Path()
    ..moveTo((Get.width - 30) / 2, 0)
    ..lineTo((Get.width - 30) / 2, 40)
    ..moveTo(0, 0)
    ..lineTo(Get.width - 30, 0);

  Widget buildItemLocation(BloodDonationEvent item, BuildContext context) {
    return DottedBorder(
      customPath: (size) => customPath,
      color: Colors.grey,
      child: Row(
        children: [
          Flexible(
            child: Container(
              padding: const EdgeInsets.only(top: 5, bottom: 10),
              color: Colors.transparent,
              alignment: Alignment.center,
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Icon(
                    Icons.person,
                    color: Color(0xff0020aa),
                    size: 20,
                  ),
                  const HSpacing(
                    spacing: 4,
                  ),
                  Text(
                    AppLocale.registeredCount
                        .translate(context)
                        .replaceAll(
                            '{registered}', '${item.soLuongDangKyHienMau ?? 0}')
                        .replaceAll('{total}', '${item.soLuongDuKien}'),
                    style: context.myTheme.textThemeT1.body.copyWith(
                      color: const Color(0xff0020aa),
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                ],
              ),
            ),
          ),
          Flexible(
            child: GestureDetector(
              onTap: () {
                if (item.googleMapLink?.isNotEmpty == true) {
                  ProcessWebviewDialog.instance
                      .openGoogleMapRoadToUrlAddress(item.googleMapLink ?? "");
                } else {
                  AppUtils.instance
                      .showToast(AppLocale.routeNotFound.translate(context));
                }
              },
              child: Container(
                padding: const EdgeInsets.only(top: 5, bottom: 10),
                color: Colors.transparent,
                alignment: Alignment.center,
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Icon(
                      Icons.location_on,
                      color: Color(0xff0020aa),
                      size: 20,
                    ),
                    const HSpacing(
                      spacing: 4,
                    ),
                    Text(
                      AppLocale.road.translate(context),
                      style: context.myTheme.textThemeT1.body.copyWith(
                        color: const Color(0xff0020aa),
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget buildItemTime(BloodDonationEvent item, BuildContext context) {
    Color timeBgColor;
    Color iconColor;

    if (item.loaiMau == LoaiMau.TieuCau.value) {
      timeBgColor = const Color(0xFFFFDAB9); // Saturated orange/beige
      iconColor = const Color(0xFFD97706); // Orange/brown
    } else {
      timeBgColor = const Color.fromARGB(255, 248, 209, 209); // Saturated pink
      iconColor = const Color(0xff5c0101); // Dark red
    }

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5)
          .copyWith(top: 3),
      decoration: BoxDecoration(
        color: timeBgColor,
        borderRadius: const BorderRadius.only(
            topRight: Radius.circular(6), bottomRight: Radius.circular(6)),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(
                Icons.access_time,
                color: iconColor,
                size: 20,
              ),
              const HSpacing(
                spacing: 4,
              ),
              Text(
                item.ngayGio?.timeHourString ?? "",
                style: context.myTheme.textThemeT1.body
                    .copyWith(color: Colors.black87),
              ),
              Text(
                " ・ ${item.ngayGio?.ddmmyyyy}",
                style: context.myTheme.textThemeT1.body
                    .copyWith(color: Colors.black87),
              ),
            ],
          ),
          Text(
            " ・ ${item.ngayGio?.dayInWeek}",
            style: context.myTheme.textThemeT1.body
                .copyWith(color: Colors.black87),
          ),
        ],
      ),
    );
  }

  @override
  Widget buildEmptyLayout() {
    return Center(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 30),
        child: Text(
          AppLocale.noDataFromDateToDate
              .translate(context)
              .replaceAll('{fromDate}', controller.startDate?.ddmmyyyy ?? '')
              .replaceAll('{toDate}', controller.endDate?.ddmmyyyy ?? ''),
          style: Get.context?.myTheme.textThemeT1.body,
          textAlign: TextAlign.center,
        ),
      ),
    );
  }
}
