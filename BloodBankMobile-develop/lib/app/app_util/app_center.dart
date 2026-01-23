import 'dart:developer';

import 'package:blood_donation/app/config/routes.dart';
import 'package:blood_donation/models/authentication.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:injectable/injectable.dart';

import '../../core/backend/backend_provider.dart';
import '../../core/storage/local_storage.dart';
import '../config/app_router.dart';
import '../theme/themes.dart';
import 'app_state.dart';

@singleton
class AppCenter {
// extends BaseCubit<AppState> {
  // AppCubit()
  //     : super(AppState(
  //           status: PageStatus.loading, appTheme: AppThemeData.light()));
  AppState state =
      AppState(status: PageStatus.loading, appTheme: AppThemeData.light());
  final AppRouter appRouter = AppRouter();
  Function()? _onAuthentication;
  LocalStorage get localStorage => LocalStorage();
  BackendProvider get backendProvider => BackendProvider();

  Authentication? authentication;

  ///system config
  int maxTuNgayDenNgayLichLayMau = 30;
  int phanTramSoLuongLuuDong = 10;
  int phanTramSoLuongTaiCho = 15;
  int soNgayChoHienMauLai = 90;
  int soNgayChoHienTieuCauLai = 14;
  int soNgayHienThiLichLayMau = 14;
  int showQRCodeOnlyDangKyId = 0;

  /// Kiot (kiosk) config
  /// - `KioskMode`: 1 = enable kiot mode
  /// - `KioskPrinterMac`: Bluetooth MAC address (Android)
  /// - `KioskPrinterPaperMm`: 58 or 80 (default 58)
  int kioskMode = 0;
  String kioskPrinterMac = "";
  int kioskPrinterPaperMm = 58;

  void checkAuthentication({required BuildContext context}) async {
    log("[AppCenter] checkAuthentication() - START");
    log("[AppCenter] backendProvider.isAuthenticated: ${backendProvider.isAuthenticated}");
    log("[AppCenter] localStorage.authentication?.accessToken exists: ${localStorage.authentication?.accessToken?.isNotEmpty == true}");
    log("[AppCenter] this.authentication?.accessToken exists: ${authentication?.accessToken?.isNotEmpty == true}");
    
    if (backendProvider.isAuthenticated) {
      log("[AppCenter] ✓ isAuthenticated = true, setting status to idle");
      state.copyWith(status: PageStatus.idle);
    } else {
      log("[AppCenter] ⚠️ isAuthenticated = false, checking for refresh token...");
      ///refresh Token
      if (authentication?.accessToken?.isNotEmpty == true) {
        log("[AppCenter] Attempting to refresh token...");
        var token = await BackendProvider().refreshToken();
        if (token?.isNotEmpty == true) {
          log("[AppCenter] ✓ Token refreshed successfully");
          authentication?.accessToken = token;
          localStorage.saveAuthentication(authentication: authentication!);
          backendProvider.notifyAuthentication(isAuthenticated: true);
          log("[AppCenter] ✓ Authentication restored after refresh");
          return;
        } else {
          log("[AppCenter] ❌ Refresh token failed or returned empty");
        }
      } else {
        log("[AppCenter] ❌ No authentication token found");
      }

      ///
      log("[AppCenter] Clearing authentication and redirecting to login...");
      await localStorage.clearAuthentication();
      Future.delayed(const Duration(milliseconds: 500), () {
        {
          Get.offAllNamed(Routes.login);
        }
      });
    }
    log("[AppCenter] checkAuthentication() - END");
  }

  @disposeMethod
  void dispose() {
    // logic to dispose instance
    if (_onAuthentication != null) {
      backendProvider.isAuthenticatedNotifier
          .removeListener(_onAuthentication!);
    }
  }

  void init({required BuildContext context}) async {
    authentication = localStorage.authentication;
    _onAuthentication = () {
      checkAuthentication(context: context);
    };
    backendProvider.isAuthenticatedNotifier.addListener(_onAuthentication!);
    checkAuthentication(context: context);
  }

  void setAuthentication(Authentication? authen) {
    authentication = authen;
  }
}
