import 'dart:convert';
import 'dart:developer' as developer;
import 'dart:io';

import 'package:blood_donation/app/app_util/app_center.dart';
import 'package:blood_donation/utils/firebase_manager.dart';
import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

import '../../app/app_util/app_state.dart';
import '../../utils/app_utils.dart';
import '../storage/local_storage.dart';
import 'backend_provider.dart';

// Helper class để queue các request đang chờ refresh token
class _PendingRequest {
  final RequestOptions requestOptions;
  final ErrorInterceptorHandler handler;
  
  _PendingRequest(this.requestOptions, this.handler);
}

class HeaderInterceptor extends InterceptorsWrapper {
  // Flag để tránh vòng lặp khi refresh token
  bool _isRefreshing = false;
  // Queue các request đang chờ refresh token
  final List<_PendingRequest> _pendingRequests = [];

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
      
      // Nếu request hiện tại là refresh-token endpoint và gặp 401
      // → Refresh token fail, clear auth và logout ngay
      if (isRefreshTokenEndpoint) {
        developer.log("[HeaderInterceptor] ❌ Refresh token endpoint returned 401. Clearing authentication...");
        _isRefreshing = false;
        GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
        await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
        BackendProvider().notifyAuthentication(isAuthenticated: false);
        super.onError(err, handler);
        return;
      }
      
      // Nếu đang refresh token (một request khác đang refresh), queue request này
      if (_isRefreshing) {
        developer.log("[HeaderInterceptor] ⏳ Token is being refreshed by another request. Queueing this request...");
        _pendingRequests.add(_PendingRequest(err.requestOptions, handler));
        return; // Đợi refresh token hoàn tất, request sẽ được retry sau
      }

      // Nếu chưa đang refresh, thử refresh token
      // Chỉ refresh nếu có token trong localStorage VÀ token chưa hết hạn (theo JWT expiration)
      var localStorage = LocalStorage();
      final accessToken = localStorage.authentication?.accessToken ?? '';
      final hasValidToken = accessToken.isNotEmpty;
      // Chỉ refresh nếu token chưa hết hạn theo JWT (nếu token đã hết hạn hoàn toàn, không thể refresh)
      bool isTokenExpired = false;
      if (hasValidToken) {
        try {
          isTokenExpired = JwtDecoder.isExpired(accessToken);
          developer.log("[HeaderInterceptor] Token expiration check: isExpired=$isTokenExpired");
        } catch (e) {
          developer.log("[HeaderInterceptor] ❌ Error checking token expiration: $e");
          isTokenExpired = true; // Nếu không parse được, coi như đã hết hạn
        }
      }
      
      if (!_isRefreshing && hasValidToken && !isTokenExpired) {
        _isRefreshing = true;
        try {
          developer.log("[HeaderInterceptor] Attempting to refresh token...");
          developer.log("[HeaderInterceptor] Current token exists in localStorage, attempting refresh...");
          var token = await BackendProvider().refreshToken();
          if (token?.isNotEmpty == true) {
            developer.log("[HeaderInterceptor] ✓ Token refreshed successfully");
            // Cập nhật token mới vào AppCenter và localStorage
            final appCenter = GetIt.instance<AppCenter>();
            if (appCenter.authentication != null) {
              appCenter.authentication!.accessToken = token;
              await appCenter.localStorage.saveAuthentication(
                  authentication: appCenter.authentication!);
              developer.log("[HeaderInterceptor] ✓ Token updated in localStorage");
            } else {
              developer.log("[HeaderInterceptor] ⚠️ AppCenter.authentication is null, cannot update token");
            }
            BackendProvider().notifyAuthentication(isAuthenticated: true);
            
            // Retry tất cả các request đang chờ (bao gồm request hiện tại)
            final dio = Dio(BaseOptions(
              baseUrl: BackendProvider().url,
              connectTimeout: const Duration(seconds: 30),
              receiveTimeout: const Duration(seconds: 30),
            ));
            
            // Retry request hiện tại
            developer.log("[HeaderInterceptor] Retrying original request with new token...");
            final opts = err.requestOptions.copyWith();
            opts.headers["Authorization"] = "Bearer $token";
            opts.headers["FireBaseToken"] = err.requestOptions.headers["FireBaseToken"] ?? FireBaseManager.firebaseToken;
            opts.headers["AppVersion"] = err.requestOptions.headers["AppVersion"] ?? AppUtils.instance.appVersion;
            opts.headers["DeviceId"] = err.requestOptions.headers["DeviceId"] ?? AppUtils.instance.deviceId;
            
            developer.log("[HeaderInterceptor] Retrying: ${opts.method} ${opts.path}");
            final response = await dio.fetch(opts);
            developer.log("[HeaderInterceptor] ✓ Retry successful with status: ${response.statusCode}");
            
            // Retry tất cả các request trong queue
            developer.log("[HeaderInterceptor] Retrying ${_pendingRequests.length} queued requests...");
            for (final pending in _pendingRequests) {
              try {
                final pendingOpts = pending.requestOptions.copyWith();
                pendingOpts.headers["Authorization"] = "Bearer $token";
                pendingOpts.headers["FireBaseToken"] = pending.requestOptions.headers["FireBaseToken"] ?? FireBaseManager.firebaseToken;
                pendingOpts.headers["AppVersion"] = pending.requestOptions.headers["AppVersion"] ?? AppUtils.instance.appVersion;
                pendingOpts.headers["DeviceId"] = pending.requestOptions.headers["DeviceId"] ?? AppUtils.instance.deviceId;
                
                final pendingResponse = await dio.fetch(pendingOpts);
                pending.handler.resolve(pendingResponse);
              } catch (e) {
                developer.log("[HeaderInterceptor] ❌ Failed to retry queued request: $e");
                pending.handler.reject(DioException(
                  requestOptions: pending.requestOptions,
                  error: e,
                ));
              }
            }
            _pendingRequests.clear();
            _isRefreshing = false;
            
            return handler.resolve(response);
          } else {
            // Refresh token thất bại, reject tất cả các request và clear auth
            developer.log("[HeaderInterceptor] ❌ Token refresh failed. Rejecting ${_pendingRequests.length} queued requests and clearing authentication...");
            for (final pending in _pendingRequests) {
              pending.handler.reject(DioException(
                requestOptions: pending.requestOptions,
                response: err.response,
              ));
            }
            _pendingRequests.clear();
            _isRefreshing = false;
            GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
            await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
            BackendProvider().notifyAuthentication(isAuthenticated: false);
          }
        } catch (e) {
          // Lỗi khi refresh token, reject tất cả các request và clear auth
          developer.log("[HeaderInterceptor] ❌ Error refreshing token: $e");
          for (final pending in _pendingRequests) {
            pending.handler.reject(DioException(
              requestOptions: pending.requestOptions,
              error: e,
            ));
          }
          _pendingRequests.clear();
          _isRefreshing = false;
          GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
          await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
          BackendProvider().notifyAuthentication(isAuthenticated: false);
        }
      } else if (!hasValidToken) {
        // Không có token trong localStorage, không thể refresh
        developer.log("[HeaderInterceptor] ⚠️ No token in localStorage, cannot refresh. Clearing authentication...");
        GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
        await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
        BackendProvider().notifyAuthentication(isAuthenticated: false);
      } else if (isTokenExpired) {
        // Token đã hết hạn theo JWT, không thể refresh
        developer.log("[HeaderInterceptor] ❌ Token expired (JWT expiration), cannot refresh. Clearing authentication...");
        GetIt.instance<AppCenter>().state.status.value = PageStatus.loading;
        await GetIt.instance<AppCenter>().localStorage.clearAuthentication();
        BackendProvider().notifyAuthentication(isAuthenticated: false);
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
