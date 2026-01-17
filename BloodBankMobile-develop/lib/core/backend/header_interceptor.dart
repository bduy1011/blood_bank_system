import 'dart:convert';
import 'dart:developer' as developer;
import 'dart:io';

import 'package:blood_donation/app/app_util/app_center.dart';
import 'package:blood_donation/utils/firebase_manager.dart';
import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';

import '../../app/app_util/app_state.dart';
import '../../utils/app_utils.dart';
import '../storage/local_storage.dart';
import 'backend_provider.dart';

class HeaderInterceptor extends InterceptorsWrapper {
  // Flag để tránh vòng lặp khi refresh token
  bool _isRefreshing = false;

  @override
  Future onRequest(
    RequestOptions options,
    RequestInterceptorHandler handler,
  ) async {
    options.baseUrl = BackendProvider().url;
    var localStorage = LocalStorage();

    ///
    if (localStorage.authentication?.accessToken?.isNotEmpty == true &&
        options.headers["Authorization"] == null) {
      options.headers["Authorization"] =
          "Bearer ${localStorage.authentication!.accessToken}";
    }

    ///app info
    options.headers["FireBaseToken"] = FireBaseManager.firebaseToken;
    options.headers["AppVersion"] = AppUtils.instance.appVersion;
    options.headers["DeviceId"] = AppUtils.instance.deviceId;

    ///log curl command
    var curlCommand = generateCurlCommand(options);
    developer.log("===============================================");
    developer.log(curlCommand);

    ///
    super.onRequest(options, handler);
  }

  @override
  Future onError(
    DioException err,
    ErrorInterceptorHandler handler,
  ) async {
    developer.log("onError : $err");
    if (err.response?.statusCode == HttpStatus.unauthorized) {
      // Kiểm tra xem request hiện tại có phải là refresh-token endpoint không
      final isRefreshTokenEndpoint = err.requestOptions.uri.path
          .contains('refresh-token');
      
      // Nếu đang refresh token và gặp 401, hoặc đang trong quá trình refresh
      // thì không nên retry, mà phải clear auth và logout ngay
      if (isRefreshTokenEndpoint || _isRefreshing) {
        developer.log("Refresh token failed or already refreshing. Clearing authentication...");
        _isRefreshing = false;
        GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
        await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
        BackendProvider().notifyAuthentication(isAuthenticated: false);
        super.onError(err, handler);
        return;
      }

      // Nếu chưa đang refresh, thử refresh token
      if (!_isRefreshing) {
        _isRefreshing = true;
        try {
          var token = await BackendProvider().refreshToken();
          if (token?.isNotEmpty == true) {
            GetIt.instance<AppCenter>().authentication?.accessToken = token;
            GetIt.instance<AppCenter>().localStorage.saveAuthentication(
                authentication: GetIt.instance<AppCenter>().authentication!);
            _isRefreshing = false;
          } else {
            // Refresh token thất bại, clear auth
            developer.log("Token refresh failed. Clearing authentication...");
            _isRefreshing = false;
            GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
            await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
            BackendProvider().notifyAuthentication(isAuthenticated: false);
          }
        } catch (e) {
          // Lỗi khi refresh token, clear auth
          developer.log("Error refreshing token: $e");
          _isRefreshing = false;
          GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
          await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
          BackendProvider().notifyAuthentication(isAuthenticated: false);
        }
      }
    }
    super.onError(err, handler);
  }

  @override
  void onResponse(Response response, ResponseInterceptorHandler handler) {
    // Log response
    developer.log("===============================================");
    developer.log("API Response:");
    developer.log("Status Code: ${response.statusCode}");
    developer.log("URL: ${response.requestOptions.uri}");
    developer.log("Method: ${response.requestOptions.method}");
    
    // Format response data
    String responseData = "";
    try {
      if (response.data != null) {
        if (response.data is Map || response.data is List) {
          responseData = const JsonEncoder.withIndent('  ').convert(response.data);
        } else {
          responseData = response.data.toString();
        }
      }
    } catch (e) {
      responseData = response.data?.toString() ?? "Unable to parse response";
    }
    
    developer.log("Response Body:");
    developer.log(responseData);
    developer.log("===============================================");

    super.onResponse(response, handler);
  }
}

String generateCurlCommand(RequestOptions options) {
  final method = options.method.toUpperCase();
  final url = options.uri.toString();
  final headers = options.headers;
  final data = options.data;
  try {
    final buffer = StringBuffer();

    buffer.write('curl -X $method');
    buffer.write(' \'$url\'');
    // Add headers
    headers.forEach((key, value) {
      buffer.write(' -H "$key: $value"');
    });
    // Add data
    if (data != null) {
      if (data is Map) {
        final jsonData = data.map((key, value) => MapEntry(key, value));
        buffer.write(' -d \'${json.encode(jsonData)}\'');
      } else if (data is FormData) {
        final formDataMap =
            data.fields.map((e) => '${e.key}=${e.value}').join('&');
        buffer.write(' -d \'$formDataMap\'');
      } else {
        buffer.write(' -d \'$data\'');
      }
    }
    return buffer.toString();
  } catch (e) {
    // TODO
    print("generateCurlCommand $e");
  }

  return "";
}
