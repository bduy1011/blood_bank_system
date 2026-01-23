import 'dart:io';

import 'package:esc_pos_utils_plus/esc_pos_utils_plus.dart';
import 'package:permission_handler/permission_handler.dart';
import 'package:print_bluetooth_thermal/print_bluetooth_thermal.dart';

class ThermalPrinterService {
  static final ThermalPrinterService instance = ThermalPrinterService._internal();
  ThermalPrinterService._internal();

  Future<bool> ensureBluetoothPermissions() async {
    if (!Platform.isAndroid) {
      // iOS permissions are handled via Info.plist and system prompt.
      return true;
    }

    // Android 12+ requires runtime BLUETOOTH_CONNECT/SCAN.
    final statuses = await [
      Permission.bluetoothConnect,
      Permission.bluetoothScan,
    ].request();

    final okConnect = statuses[Permission.bluetoothConnect]?.isGranted ?? true;
    final okScan = statuses[Permission.bluetoothScan]?.isGranted ?? true;
    return okConnect && okScan;
  }

  Future<List<BluetoothInfo>> getPairedPrinters() async {
    return await PrintBluetoothThermal.pairedBluetooths;
  }

  Future<bool> connect(String macAddress) async {
    final mac = macAddress.trim();
    if (mac.isEmpty) return false;

    final status = await PrintBluetoothThermal.connectionStatus;
    if (status) return true;

    return await PrintBluetoothThermal.connect(macPrinterAddress: mac);
  }

  Future<bool> printRegistrationQr({
    required String printerMac,
    required int paperMm,
    required String qrData,
    required String nameBloodDonation,
    required DateTime timeBloodDonation,
    required String idBloodDonation,
    required String idRegister,
  }) async {
    final connected = await connect(printerMac);
    if (!connected) return false;

    final profile = await CapabilityProfile.load();
    final paper =
        paperMm == 80 ? PaperSize.mm80 : PaperSize.mm58;
    final generator = Generator(paper, profile);

    final bytes = <int>[];
    bytes.addAll(generator.reset());

    bytes.addAll(generator.text(
      'MÃ QR ĐĂNG KÝ HIẾN MÁU',
      styles: const PosStyles(
        align: PosAlign.center,
        bold: true,
        height: PosTextSize.size2,
        width: PosTextSize.size2,
      ),
      linesAfter: 1,
    ));

    bytes.addAll(generator.text(
      nameBloodDonation,
      styles: const PosStyles(align: PosAlign.center, bold: true),
      linesAfter: 1,
    ));

    bytes.addAll(generator.text(
      'Thời gian: ${_formatDateTime(timeBloodDonation)}',
      styles: const PosStyles(align: PosAlign.center),
    ));
    bytes.addAll(generator.text(
      'Id đợt: $idBloodDonation',
      styles: const PosStyles(align: PosAlign.center),
    ));
    bytes.addAll(generator.text(
      'Id đăng ký: $idRegister',
      styles: const PosStyles(align: PosAlign.center),
      linesAfter: 1,
    ));

    bytes.addAll(generator.qrcode(
      qrData,
      size: const QRSize(6),
      cor: QRCorrection.H,
    ));
    bytes.addAll(generator.feed(2));

    bytes.addAll(generator.text(
      'Vui lòng mang mã này đến quầy tiếp nhận.',
      styles: const PosStyles(align: PosAlign.center),
    ));
    bytes.addAll(generator.feed(2));

    try {
      bytes.addAll(generator.cut());
    } catch (_) {
      // Some printers don't support cut.
    }

    return await PrintBluetoothThermal.writeBytes(bytes);
  }

  static String _formatDateTime(DateTime dt) {
    final dd = dt.day.toString().padLeft(2, '0');
    final mm = dt.month.toString().padLeft(2, '0');
    final yyyy = dt.year.toString();
    final hh = dt.hour.toString().padLeft(2, '0');
    final min = dt.minute.toString().padLeft(2, '0');
    return '$hh:$min  $dd/$mm/$yyyy';
  }

  Future<void> disconnect() async {
    try {
      await PrintBluetoothThermal.disconnect;
    } catch (_) {
      // ignore
    }
  }
}

