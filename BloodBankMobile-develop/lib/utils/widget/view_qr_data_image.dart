import 'dart:convert';
import 'dart:typed_data';

import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/features/donor_signature/presentation/donor_signature_page.dart';
import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:blood_donation/utils/extension/datetime_extension.dart';
import 'package:blood_donation/utils/app_utils.dart';
import 'package:blood_donation/utils/printer/printer_settings.dart';
import 'package:blood_donation/utils/printer/thermal_printer_service.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:get_it/get_it.dart';
import 'package:photo_view/photo_view.dart';
import 'package:print_bluetooth_thermal/print_bluetooth_thermal.dart';
import 'package:qr_flutter/qr_flutter.dart';
import 'package:screen_brightness/screen_brightness.dart';

import '../../app/app_util/app_center.dart';

class ViewQrImageData extends StatefulWidget {
  const ViewQrImageData({
    super.key,
    required this.data,
    required this.nameBloodDonation,
    required this.timeBloodDonation,
    required this.idBloodDonation,
    required this.idRegister,
  });
  final String data;
  final String nameBloodDonation;
  final DateTime timeBloodDonation;
  final String idBloodDonation;
  final String idRegister;

  @override
  State<ViewQrImageData> createState() => _ViewQrImageDataState();
}

class _ViewQrImageDataState extends State<ViewQrImageData> {
  // double _previousBrightness = 0.5;
  // final double _previousApplicationBrightness = 0.5;
  final appCenter = GetIt.instance<AppCenter>();

  bool _isLoadingSignature = false;
  bool _isSigned = false;
  DateTime? _signedAt;
  DonorSignatureResult? _signatureResult;

  bool _autoPrintTriggered = false;

  @override
  void initState() {
    super.initState();
    // setSystemBrightness(1.0); // Set brightness to 100%
    setApplicationBrightness(1.0);
    _loadSignatureStatus();

    // Kiot: auto print QR right after opening this screen.
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _tryAutoPrintForKiosk();
    });
  }

  bool get _isKiosk => appCenter.kioskMode == 1;

  Future<void> _tryAutoPrintForKiosk() async {
    if (!_isKiosk) return;
    if (_autoPrintTriggered) return;
    _autoPrintTriggered = true;

    try {
      EasyLoading.show(status: 'Đang kết nối máy in...');

      final okPerm = await ThermalPrinterService.instance.ensureBluetoothPermissions();
      if (!okPerm) {
        EasyLoading.dismiss();
        AppUtils.instance.showToast('Chưa cấp quyền Bluetooth để in.');
        await _showSelectPrinterBottomSheet(autoPrintAfterSelect: true);
        return;
      }

      final paperMm =
          (appCenter.kioskPrinterPaperMm == 80 || appCenter.kioskPrinterPaperMm == 58)
              ? appCenter.kioskPrinterPaperMm
              : await PrinterSettings.getPaperMm(defaultValue: 58);

      final macFromServer = appCenter.kioskPrinterMac.trim();
      final macFromLocal = (await PrinterSettings.getMacAddress()) ?? '';
      final printerMac = macFromServer.isNotEmpty ? macFromServer : macFromLocal;

      if (printerMac.isEmpty) {
        EasyLoading.dismiss();
        await _showSelectPrinterBottomSheet(autoPrintAfterSelect: true);
        return;
      }

      final printed = await ThermalPrinterService.instance.printRegistrationQr(
        printerMac: printerMac,
        paperMm: paperMm,
        qrData: widget.data,
        nameBloodDonation: widget.nameBloodDonation,
        timeBloodDonation: widget.timeBloodDonation,
        idBloodDonation: widget.idBloodDonation,
        idRegister: widget.idRegister,
      );

      EasyLoading.dismiss();
      if (!printed) {
        AppUtils.instance.showToast('In thất bại. Vui lòng kiểm tra máy in.');
        await _showSelectPrinterBottomSheet(autoPrintAfterSelect: true);
      } else {
        AppUtils.instance.showToast('Đã in QR.');
      }
    } catch (_) {
      EasyLoading.dismiss();
      AppUtils.instance.showToast('In thất bại. Vui lòng thử lại.');
    }
  }

  Future<void> _showSelectPrinterBottomSheet({required bool autoPrintAfterSelect}) async {
    if (!mounted) return;

    final enabled = await PrintBluetoothThermal.bluetoothEnabled;
    if (!enabled) {
      AppUtils.instance.showToast('Vui lòng bật Bluetooth để kết nối máy in.');
      return;
    }

    EasyLoading.show(status: 'Đang tìm máy in đã ghép nối...');
    final devices = await ThermalPrinterService.instance.getPairedPrinters();
    EasyLoading.dismiss();

    if (devices.isEmpty) {
      AppUtils.instance.showToast('Không tìm thấy máy in Bluetooth đã ghép nối.');
      return;
    }

    await showModalBottomSheet(
      context: context,
      useSafeArea: true,
      showDragHandle: true,
      builder: (_) {
        return ListView.separated(
          padding: const EdgeInsets.symmetric(vertical: 8),
          itemCount: devices.length,
          separatorBuilder: (_, __) => const Divider(height: 1),
          itemBuilder: (_, i) {
            final d = devices[i];
            return ListTile(
              leading: const Icon(Icons.print),
              title: Text(d.name),
              subtitle: Text(d.macAdress),
              onTap: () async {
                await PrinterSettings.setMacAddress(d.macAdress);
                if (mounted) Get.back();
                if (autoPrintAfterSelect) {
                  _autoPrintTriggered = false; // allow retry after selecting
                  await _tryAutoPrintForKiosk();
                }
              },
            );
          },
        );
      },
    );
  }

  Future<void> _loadSignatureStatus() async {
    setState(() => _isLoadingSignature = true);
    try {
      final response = await appCenter.backendProvider.getDonorSignatureInfo(
        registerId: widget.idRegister,
        includeImage: false,
      );
      if (response?.status == 200 && response?.data != null) {
        setState(() {
          _isSigned = response!.data!.isSigned;
          _signedAt = response.data!.signedAt;
        });
      }
    } catch (_) {
      // ignore
    } finally {
      if (mounted) setState(() => _isLoadingSignature = false);
    }
  }

  Future<void> _viewSignatureIfAny() async {
    try {
      EasyLoading.show(status: 'Đang tải chữ ký...');
      final response = await appCenter.backendProvider.getDonorSignatureInfo(
        registerId: widget.idRegister,
        includeImage: true,
      );
      EasyLoading.dismiss();

      if (response?.status == 200 && response?.data != null) {
        final base64Str = response!.data!.signatureBase64;
        if (base64Str != null && base64Str.isNotEmpty) {
          final bytes = base64Decode(base64Str);
          setState(() {
            _signatureResult = DonorSignatureResult(bytes);
            _signedAt = response.data!.signedAt;
            _isSigned = true;
          });
          _openSignatureDialog(bytes);
        } else {
          AppUtils.instance.showToast('Chưa có ảnh chữ ký.');
        }
      } else {
        AppUtils.instance.showToast(response?.message ?? 'Không tải được chữ ký');
      }
    } catch (e) {
      EasyLoading.dismiss();
      AppUtils.instance.showToast('Không tải được chữ ký');
    }
  }

  void _openSignatureDialog(Uint8List bytes) {
    Get.dialog(
      Dialog(
        insetPadding: const EdgeInsets.all(16),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        child: SizedBox(
          height: 360,
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                child: Row(
                  children: [
                    const Icon(Icons.draw, color: Color(0xff5c0101)),
                    const SizedBox(width: 8),
                    Expanded(
                      child: Text(
                        'Chữ ký của bạn',
                        style: Get.context?.myTheme.textThemeT1.title
                            .copyWith(fontWeight: FontWeight.w700),
                      ),
                    ),
                    IconButton(
                      onPressed: () => Get.back(),
                      icon: const Icon(Icons.close),
                    ),
                  ],
                ),
              ),
              const Divider(height: 1),
              Expanded(
                child: ClipRRect(
                  borderRadius: const BorderRadius.only(
                    bottomLeft: Radius.circular(16),
                    bottomRight: Radius.circular(16),
                  ),
                  child: PhotoView(
                    imageProvider: MemoryImage(bytes),
                    backgroundDecoration: const BoxDecoration(color: Colors.white),
                    minScale: PhotoViewComputedScale.contained,
                    maxScale: PhotoViewComputedScale.covered * 3.0,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> _signNow() async {
    if (_isSigned) return;

    final result = await Get.to(
      () => const DonorSignaturePage(title: 'Ký xác nhận tiếp nhận'),
      fullscreenDialog: true,
    );

    if (result is! DonorSignatureResult) return;

    try {
      EasyLoading.show(status: 'Đang lưu chữ ký...');
      final response = await appCenter.backendProvider.saveDonorSignature(
        registerId: widget.idRegister,
        signatureBase64Png: result.base64Png,
        updateStatusToDaTiepNhan: true,
      );
      EasyLoading.dismiss();

      if (response?.status == 200 && response?.data != null) {
        setState(() {
          _isSigned = true;
          _signedAt = response!.data!.signedAt ?? DateTime.now();
          _signatureResult = result;
        });
        AppUtils.instance.showToast('Đã lưu chữ ký.');
      } else {
        AppUtils.instance.showToast(response?.message ?? 'Lưu chữ ký thất bại');
      }
    } catch (e) {
      EasyLoading.dismiss();
      AppUtils.instance.showToast('Lưu chữ ký thất bại');
    }
  }

  Future<double> get systemBrightness async {
    try {
      return await ScreenBrightness.instance.system;
    } catch (e) {
      print(e);
      print('Failed to get system brightness');
    }
    return 0.5;
  }

  Future<void> setSystemBrightness(double brightness) async {
    try {
      await ScreenBrightness.instance.setSystemScreenBrightness(brightness);
    } catch (e) {
      debugPrint(e.toString());
    }
  }

  Future<double> get applicationBrightness async {
    try {
      return await ScreenBrightness.instance.application;
    } catch (e) {
      print(e);
      print('Failed to get application brightness');
    }
    return 0.5;
  }

  Future<void> setApplicationBrightness(double brightness) async {
    try {
      await ScreenBrightness.instance
          .setApplicationScreenBrightness(brightness);
    } catch (e) {
      debugPrint(e.toString());
      print('Failed to set application brightness');
    }
  }

  Future<void> resetApplicationBrightness() async {
    try {
      await ScreenBrightness.instance.resetApplicationScreenBrightness();
    } catch (e) {
      debugPrint(e.toString());
      throw 'Failed to reset application brightness';
    }
  }

  Future<void> _restoreBrightness() async {
    //
    // await setSystemBrightness(_previousBrightness);
    await resetApplicationBrightness();
  }

  @override
  void dispose() {
    _restoreBrightness(); // Restore brightness when exiting
    super.dispose();
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
          title: Text(
            AppLocale.qrCodeRegistration.translate(context),
            style: context.myTheme.textThemeT1.title.copyWith(
              color: Colors.white,
            ),
          ),
          leading: IconButton(
              onPressed: () {
                Get.back();
              },
              icon: const Icon(
                Icons.arrow_back_rounded,
                color: Colors.white,
              )),
          centerTitle: true,
          backgroundColor: Colors.transparent,
          elevation: 0,
          automaticallyImplyLeading: false,
        ),
        body: Container(
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.only(
              topLeft: Radius.circular(30),
              topRight: Radius.circular(30),
            ),
          ),
          child: SafeArea(
            top: false,
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(15),
              child: Column(
                children: [
                  buildInfo(),
                  const SizedBox(height: 8),
                  QrImageView(
                    data: widget.data,
                    version: QrVersions.auto,
                    size: 320,
                    gapless: false,
                    embeddedImageStyle: const QrEmbeddedImageStyle(
                      size: Size(80, 80),
                    ),
                  ),
                  if (_isKiosk) ...[
                    const SizedBox(height: 12),
                    Row(
                      children: [
                        Expanded(
                          child: ElevatedButton.icon(
                            onPressed: () async {
                              _autoPrintTriggered = false;
                              await _tryAutoPrintForKiosk();
                            },
                            icon: const Icon(Icons.print),
                            label: const Text('In QR'),
                          ),
                        ),
                        const SizedBox(width: 10),
                        OutlinedButton(
                          onPressed: () async {
                            await _showSelectPrinterBottomSheet(
                              autoPrintAfterSelect: false,
                            );
                          },
                          child: const Text('Chọn máy in'),
                        ),
                      ],
                    ),
                  ],
                  const SizedBox(height: 14),
                  _buildSignatureSection(context),
                  SizedBox(height: MediaQuery.of(context).padding.bottom + 8),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildSignatureSection(BuildContext context) {
    final signedAtText =
        _signedAt != null ? ' • ${_signedAt!.ddmmyyyy} ${_signedAt!.timeHourString}' : '';

    return Container(
      width: double.infinity,
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: const Color.fromARGB(255, 248, 243, 243),
        borderRadius: BorderRadius.circular(14),
        border: Border.all(color: Colors.black12),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              const Icon(Icons.edit, size: 18, color: Color(0xff5c0101)),
              const SizedBox(width: 8),
              Expanded(
                child: Text(
                  _isSigned ? 'Đã ký xác nhận$signedAtText' : 'Chưa ký xác nhận',
                  style: context.myTheme.textThemeT1.body.copyWith(
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
              if (_isLoadingSignature)
                const SizedBox(
                  width: 18,
                  height: 18,
                  child: CircularProgressIndicator(strokeWidth: 2),
                ),
            ],
          ),
          const SizedBox(height: 10),
          ClipRRect(
            borderRadius: BorderRadius.circular(12),
            child: Container(
              width: double.infinity,
              color: Colors.white,
              padding: const EdgeInsets.all(10),
              child: AspectRatio(
                aspectRatio: 3.2, // giống khung ký thực tế
                child: _signatureResult?.pngBytes != null
                    ? InkWell(
                        onTap: () => _openSignatureDialog(_signatureResult!.pngBytes),
                        child: Image.memory(
                          _signatureResult!.pngBytes,
                          fit: BoxFit.contain,
                        ),
                      )
                    : Center(
                        child: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Icon(
                              _isSigned ? Icons.image_outlined : Icons.border_color,
                              color: Colors.black38,
                            ),
                            const SizedBox(width: 8),
                            Text(
                              _isSigned
                                  ? 'Chưa tải ảnh chữ ký (bấm Xem)'
                                  : 'Ký vào đây để xác nhận',
                              style: context.myTheme.textThemeT1.body.copyWith(
                                color: Colors.black54,
                              ),
                            ),
                          ],
                        ),
                      ),
              ),
            ),
          ),
          const SizedBox(height: 10),
          Row(
            children: [
              Expanded(
                child: ElevatedButton(
                  onPressed: _isSigned ? null : _signNow,
                  child: Text(_isSigned ? 'Đã ký' : 'Ký xác nhận tiếp nhận'),
                ),
              ),
              const SizedBox(width: 10),
              OutlinedButton(
                onPressed: _isSigned
                    ? () async {
                        // Nếu đã có ảnh trong bộ nhớ thì mở luôn,
                        // nếu chưa có thì tải từ server rồi mở.
                        final bytes = _signatureResult?.pngBytes;
                        if (bytes != null) {
                          _openSignatureDialog(bytes);
                          return;
                        }
                        await _viewSignatureIfAny();
                      }
                    : null,
                child: Text(_signatureResult?.pngBytes != null ? 'Xem lớn' : 'Xem'),
              ),
            ],
          ),
          const SizedBox(height: 6),
          Text(
            'Lưu ý: chữ ký này là chữ ký tay của người hiến để xác nhận tiếp nhận.',
            style: context.myTheme.textThemeT1.body.copyWith(
              color: Colors.black54,
              fontSize: 12,
            ),
          ),
        ],
      ),
    );
  }

  Widget buildInfo() {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 20),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              const Icon(Icons.bloodtype,
                  color: Colors.red), // Icon cho tên đợt hiến máu
              const SizedBox(width: 8),
              Expanded(
                child: Text.rich(
                  TextSpan(
                    children: [
                      TextSpan(
                        text: widget.nameBloodDonation.toUpperCase(),
                        style: const TextStyle(fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                  style: const TextStyle(fontSize: 16),
                ),
              ),
            ],
          ),
          const SizedBox(height: 8),
          Row(
            children: [
              const Icon(Icons.calendar_today,
                  color: Colors.blue), // Icon cho ngày tháng
              const SizedBox(width: 8),
              Expanded(
                child: Text.rich(
                  TextSpan(
                    children: [
                      TextSpan(
                        text: '${AppLocale.time.translate(context)}: ',
                        style: const TextStyle(fontWeight: FontWeight.bold),
                      ),
                      TextSpan(
                        text:
                            '${widget.timeBloodDonation.timeHourString} ngày ${widget.timeBloodDonation.ddmmyyyy} (${widget.timeBloodDonation.dayInWeek})', // Giá trị ví dụ
                        style: const TextStyle(fontWeight: FontWeight.normal),
                      ),
                    ],
                  ),
                  style: const TextStyle(fontSize: 16),
                ),
              ),
            ],
          ),
          const SizedBox(height: 8),
          Row(
            children: [
              const Icon(Icons.tag, color: Colors.green), // Icon cho Id đợt
              const SizedBox(width: 8),
              Expanded(
                child: Text.rich(
                  TextSpan(
                    children: [
                      TextSpan(
                        text: '${AppLocale.eventId.translate(context)}: ',
                        style: const TextStyle(fontWeight: FontWeight.bold),
                      ),
                      TextSpan(
                        text: widget.idBloodDonation, // Giá trị ví dụ
                        style: const TextStyle(fontWeight: FontWeight.normal),
                      ),
                    ],
                  ),
                  style: const TextStyle(fontSize: 16),
                ),
              ),
            ],
          ),
          const SizedBox(height: 8),
          Row(
            children: [
              const Icon(Icons.tag, color: Colors.green), // Icon cho Id đăng ký
              const SizedBox(width: 8),
              Expanded(
                child: Text.rich(
                  TextSpan(
                    children: [
                      TextSpan(
                        text: '${AppLocale.registrationId.translate(context)}: ',
                        style: const TextStyle(fontWeight: FontWeight.bold),
                      ),
                      TextSpan(
                        text: widget.idRegister, // Giá trị ví dụ
                        style: const TextStyle(fontWeight: FontWeight.normal),
                      ),
                    ],
                  ),
                  style: const TextStyle(fontSize: 16),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
