import 'dart:developer';
import 'dart:io';
import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:device_info_plus/device_info_plus.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:local_auth/local_auth.dart';

class BiometricAuthService {
  static final BiometricAuthService _instance =
      BiometricAuthService._internal();
  factory BiometricAuthService() => _instance;
  BiometricAuthService._internal();

  final LocalAuthentication _localAuth = LocalAuthentication();
  bool? _isEmulator;

  /// Kiểm tra xem có đang chạy trên emulator không
  Future<bool> isRunningOnEmulator() async {
    if (_isEmulator != null) return _isEmulator!;

    try {
      if (Platform.isAndroid) {
        final deviceInfo = DeviceInfoPlugin();
        final androidInfo = await deviceInfo.androidInfo;
        _isEmulator = androidInfo.isPhysicalDevice == false;
      } else if (Platform.isIOS) {
        final deviceInfo = DeviceInfoPlugin();
        final iosInfo = await deviceInfo.iosInfo;
        _isEmulator = iosInfo.model.toLowerCase().contains('simulator') ||
            !iosInfo.isPhysicalDevice;
      } else {
        _isEmulator = false;
      }
      return _isEmulator ?? false;
    } catch (e) {
      log("isRunningOnEmulator error: $e");
      return false;
    }
  }

  /// Kiểm tra xem thiết bị có hỗ trợ biometric không
  Future<bool> isDeviceSupported() async {
    try {
      return await _localAuth.isDeviceSupported();
    } catch (e) {
      log("isDeviceSupported error: $e");
      return false;
    }
  }

  /// Kiểm tra xem có biometric nào được đăng ký không
  Future<bool> canCheckBiometrics() async {
    try {
      return await _localAuth.canCheckBiometrics;
    } catch (e) {
      log("canCheckBiometrics error: $e");
      return false;
    }
  }

  /// Lấy danh sách các loại biometric có sẵn
  Future<List<BiometricType>> getAvailableBiometrics() async {
    try {
      return await _localAuth.getAvailableBiometrics();
    } catch (e) {
      log("getAvailableBiometrics error: $e");
      return [];
    }
  }

  /// Xác thực bằng biometric
  Future<bool> authenticate({
    String? reason,
    BuildContext? context,
  }) async {
    try {
      // Kiểm tra nếu đang chạy trên emulator
      final isEmulator = await isRunningOnEmulator();
      if (isEmulator && context != null && context.mounted) {
        // Sử dụng mock dialog cho emulator
        return await _authenticateMock(context, reason);
      }

      // Trên thiết bị thật, để local_auth tự xử lý và hiển thị dialog
      final bool didAuthenticate = await _localAuth.authenticate(
        localizedReason: reason ?? (context != null ? AppLocale.biometricAuthReason.translate(context) : 'Please authenticate to login'),
      );
      return didAuthenticate;
    } on PlatformException catch (e) {
      // Xử lý các lỗi từ local_auth
      log("PlatformException in authenticate: ${e.code} - ${e.message}");
      if (e.code == 'NotAvailable' || e.code == 'NotEnrolled') {
        return false;
      }
      return false;
    } catch (e) {
      // Lỗi không mong đợi
      log("authenticate error: $e");
      return false;
    }
  }

  /// Mock authentication dialog cho emulator
  Future<bool> _authenticateMock(BuildContext context, String? reason) async {
    try {
      final result = await showDialog<bool>(
        context: context,
        barrierDismissible: false,
        builder: (BuildContext dialogContext) {
          return AlertDialog(
            title: Row(
              children: [
                const Icon(Icons.fingerprint, color: Colors.blue, size: 28),
                const SizedBox(width: 10),
                Text(AppLocale.biometricAuth.translate(context)),
              ],
            ),
            content: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  reason ?? AppLocale.biometricAuthReason.translate(context),
                  style: const TextStyle(fontSize: 16),
                ),
                const SizedBox(height: 20),
                Container(
                  padding: const EdgeInsets.all(12),
                  decoration: BoxDecoration(
                    color: Colors.blue.shade50,
                    borderRadius: BorderRadius.circular(8),
                    border: Border.all(color: Colors.blue.shade200),
                  ),
                  child: Row(
                    children: [
                      const Icon(Icons.info_outline, color: Colors.blue, size: 20),
                      const SizedBox(width: 8),
                      Expanded(
                        child: Text(
                          AppLocale.emulatorMode.translate(context),
                          style: const TextStyle(
                            fontSize: 12,
                            fontStyle: FontStyle.italic,
                            color: Colors.blue,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
            actions: [
              TextButton(
                onPressed: () {
                  Navigator.of(dialogContext).pop(false);
                },
                child: Text(AppLocale.cancel.translate(context)),
              ),
              ElevatedButton(
                onPressed: () {
                  Navigator.of(dialogContext).pop(true);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.blue,
                  foregroundColor: Colors.white,
                ),
                child: Text(AppLocale.biometricAuthSuccess.translate(context)),
              ),
              TextButton(
                onPressed: () {
                  Navigator.of(dialogContext).pop(false);
                },
                child: Text(
                  AppLocale.biometricAuthFailed.translate(context),
                  style: const TextStyle(color: Colors.red),
                ),
              ),
            ],
          );
        },
      );

      // Đảm bảo dialog đã đóng hoàn toàn
      await Future.delayed(const Duration(milliseconds: 100));

      return result ?? false;
    } catch (e) {
      log("_authenticateMock error: $e");
      return false;
    }
  }

  /// Kiểm tra xem có thể xác thực không (có biometric và đã đăng ký hoặc đang trên emulator)
  Future<bool> isAvailable() async {
    try {
      // Nếu đang trên emulator, luôn return true để có thể test
      final isEmulator = await isRunningOnEmulator();
      if (isEmulator) {
        return true;
      }

      final bool canCheckBiometrics = await _localAuth.canCheckBiometrics;
      final bool isSupported = await _localAuth.isDeviceSupported();
      return canCheckBiometrics || isSupported;
    } catch (e) {
      log("isAvailable error: $e");
      return false;
    }
  }

  /// Lấy tên loại biometric (vân tay, Face ID, etc.)
  Future<String> getBiometricTypeName() async {
    try {
      final availableBiometrics = await getAvailableBiometrics();
      if (availableBiometrics.isEmpty) {
        return 'Biometric';
      }

      // Note: This method returns a simple string, not localized
      // The caller should handle localization if needed
      if (availableBiometrics.contains(BiometricType.face)) {
        return 'Face ID';
      } else if (availableBiometrics.contains(BiometricType.fingerprint)) {
        return 'Fingerprint';
      } else if (availableBiometrics.contains(BiometricType.iris)) {
        return 'Iris';
      } else if (availableBiometrics.contains(BiometricType.strong)) {
        return 'Strong';
      } else if (availableBiometrics.contains(BiometricType.weak)) {
        return 'Weak';
      }
      return 'Biometric';
    } catch (e) {
      log("getBiometricTypeName error: $e");
      return 'Biometric';
    }
  }
}
