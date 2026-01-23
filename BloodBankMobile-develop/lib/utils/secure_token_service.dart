import 'dart:convert';
import 'dart:developer';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:blood_donation/models/authentication.dart';

/// Service để quản lý tokens trong secure storage (Keychain/Keystore)
/// 
/// Sử dụng flutter_secure_storage để lưu tokens an toàn:
/// - iOS: Keychain
/// - Android: Keystore
class SecureTokenService {
  static final SecureTokenService _instance = SecureTokenService._internal();
  factory SecureTokenService() => _instance;
  SecureTokenService._internal();

  static const FlutterSecureStorage _storage = FlutterSecureStorage(
    aOptions: AndroidOptions(
      encryptedSharedPreferences: true,
    ),
    iOptions: IOSOptions(
      accessibility: KeychainAccessibility.first_unlock_this_device,
    ),
  );

  // Keys cho secure storage
  static const String _keyAccessToken = 'access_token';
  static const String _keyRefreshToken = 'refresh_token';
  static const String _keyBiometricEnabled = 'biometric_enabled';
  static const String _keyUserCode = 'user_code';
  static const String _keyUserName = 'user_name';
  static const String _keyAuthentication = 'authentication'; // Lưu toàn bộ Authentication object
  static const String _keyLastLoginUsername = 'last_login_username';
  static const String _keyLastLoginPassword = 'last_login_password';

  static const String _passwordKeyPrefix = 'login_password_';
  static String _keyPasswordForUsername(String username) =>
      '$_passwordKeyPrefix${username.trim()}';

  static const String _signatureKeyPrefix = 'user_signature_';
  static String _keySignatureForUserCode(String userCode) =>
      '$_signatureKeyPrefix${userCode.trim()}';

  /// Lưu mật khẩu đăng nhập vào secure storage (Keychain/Keystore)
  ///
  /// - Lưu `last_login_username` để lần sau có thể tự nạp đúng mật khẩu
  /// - Lưu mật khẩu theo từng username để tránh ghi đè khi đổi user
  Future<void> saveLoginCredentials({
    required String username,
    required String password,
  }) async {
    final u = username.trim();
    if (u.isEmpty || password.isEmpty) return;
    try {
      await _storage.write(key: _keyLastLoginUsername, value: u);
      await _storage.write(key: _keyLastLoginPassword, value: password);
      await _storage.write(key: _keyPasswordForUsername(u), value: password);
    } catch (e) {
      log("saveLoginCredentials() error: $e");
      rethrow;
    }
  }

  /// Lấy username lần đăng nhập gần nhất (nếu có)
  Future<String?> getLastLoginUsername() async {
    try {
      return await _storage.read(key: _keyLastLoginUsername);
    } catch (e) {
      log("getLastLoginUsername() error: $e");
      return null;
    }
  }

  /// Lấy mật khẩu đã lưu. Nếu không truyền username thì lấy theo last_login_username.
  Future<String?> getLoginPassword({String? username}) async {
    try {
      final u = (username ?? await getLastLoginUsername())?.trim();
      if (u == null || u.isEmpty) {
        // fallback: nếu chỉ lưu theo key "last_login_password"
        return await _storage.read(key: _keyLastLoginPassword);
      }
      return await _storage.read(key: _keyPasswordForUsername(u)) ??
          await _storage.read(key: _keyLastLoginPassword);
    } catch (e) {
      log("getLoginPassword() error: $e");
      return null;
    }
  }

  /// Xóa mật khẩu đã lưu (theo username hoặc theo last_login_username nếu null).
  /// Không xóa token/authentication, chỉ xóa credential.
  Future<void> deleteSavedPassword({String? username}) async {
    try {
      final u = (username ?? await getLastLoginUsername())?.trim();
      if (u != null && u.isNotEmpty) {
        await _storage.delete(key: _keyPasswordForUsername(u));
      }

      // Nếu last_login_username trùng thì xóa luôn các key "last_*"
      final lastU = (await _storage.read(key: _keyLastLoginUsername))?.trim();
      if (lastU != null &&
          lastU.isNotEmpty &&
          (u == null || u.isEmpty || lastU == u)) {
        await _storage.delete(key: _keyLastLoginUsername);
        await _storage.delete(key: _keyLastLoginPassword);
      }
    } catch (e) {
      log("deleteSavedPassword() error: $e");
      rethrow;
    }
  }

  /// Xóa toàn bộ mật khẩu đã lưu (mọi username) + last_*.
  /// Không xóa token/authentication, chỉ xóa credential.
  Future<void> clearSavedPasswords() async {
    try {
      await _storage.delete(key: _keyLastLoginUsername);
      await _storage.delete(key: _keyLastLoginPassword);

      final all = await _storage.readAll();
      final passwordKeys =
          all.keys.where((k) => k.startsWith(_passwordKeyPrefix)).toList();
      for (final k in passwordKeys) {
        await _storage.delete(key: k);
      }
    } catch (e) {
      log("clearSavedPasswords() error: $e");
      rethrow;
    }
  }

  /// Lưu access token và refresh token vào secure storage
  /// 
  /// [accessToken]: Access token từ server
  /// [refreshToken]: Refresh token từ server (optional)
  /// [userCode]: Mã người dùng (để biết login vào tài khoản nào)
  /// [userName]: Tên người dùng (để hiển thị khi dùng biometric)
  Future<void> saveTokens({
    required String accessToken,
    String? refreshToken,
    String? userCode,
    String? userName,
  }) async {
    try {
      await _storage.write(key: _keyAccessToken, value: accessToken);
      if (refreshToken != null && refreshToken.isNotEmpty) {
        await _storage.write(key: _keyRefreshToken, value: refreshToken);
      }
      if (userCode != null && userCode.isNotEmpty) {
        await _storage.write(key: _keyUserCode, value: userCode);
      }
      if (userName != null && userName.isNotEmpty) {
        await _storage.write(key: _keyUserName, value: userName);
      }
      await _storage.write(key: _keyBiometricEnabled, value: 'true');
    } catch (e) {
      log("saveTokens() error: $e");
      rethrow;
    }
  }

  /// Lưu toàn bộ Authentication object vào secure storage
  /// Đây là cách an toàn để lưu thông tin user đầy đủ, được khóa bằng biometric
  Future<void> saveAuthentication(Authentication authentication) async {
    try {
      log("[SecureTokenService] Saving Authentication to secure storage...");
      log("[SecureTokenService] UserCode: ${authentication.userCode}, Name: ${authentication.name}");
      log("[SecureTokenService] AccessToken length: ${authentication.accessToken?.length ?? 0}");
      
      // Lưu toàn bộ Authentication object dạng JSON
      final authJson = jsonEncode(authentication.toJson());
      await _storage.write(key: _keyAuthentication, value: authJson);
      log("[SecureTokenService] ✓ Authentication object saved to secure storage");
      
      // Vẫn lưu các field riêng lẻ để tương thích với code cũ
      await saveTokens(
        accessToken: authentication.accessToken ?? '',
        refreshToken: null,
        userCode: authentication.userCode,
        userName: authentication.name,
      );
      
      log("[SecureTokenService] ✓ Tokens saved to secure storage successfully");
      
      // Verify sau khi lưu
      final verifyToken = await getAccessToken();
      final verifyAuth = await _storage.read(key: _keyAuthentication);
      log("[SecureTokenService] Verification - AccessToken exists: ${verifyToken != null && verifyToken.isNotEmpty}");
      log("[SecureTokenService] Verification - Authentication exists: ${verifyAuth != null && verifyAuth.isNotEmpty}");
    } catch (e) {
      log("[SecureTokenService] ❌ saveAuthentication() error: $e");
      rethrow;
    }
  }

  /// Lấy toàn bộ Authentication object từ secure storage
  /// Trả về null nếu không tìm thấy hoặc lỗi
  Future<Authentication?> getAuthentication() async {
    try {
      final authJson = await _storage.read(key: _keyAuthentication);
      if (authJson == null || authJson.isEmpty) {
        log("No authentication found in secure storage");
        return null;
      }
      
      final authMap = jsonDecode(authJson) as Map<String, dynamic>;
      final authentication = Authentication.fromJson(authMap);
      
      log("Authentication object retrieved from secure storage: userCode=${authentication.userCode}");
      return authentication;
    } catch (e) {
      log("getAuthentication() error: $e");
      return null;
    }
  }

  /// Lấy access token từ secure storage
  Future<String?> getAccessToken() async {
    try {
      return await _storage.read(key: _keyAccessToken);
    } catch (e) {
      log("getAccessToken() error: $e");
      return null;
    }
  }

  /// Lấy refresh token từ secure storage
  Future<String?> getRefreshToken() async {
    try {
      return await _storage.read(key: _keyRefreshToken);
    } catch (e) {
      log("getRefreshToken() error: $e");
      return null;
    }
  }

  /// Kiểm tra xem có tokens đã lưu không
  Future<bool> hasTokens() async {
    try {
      log("[SecureTokenService] Checking if tokens exist...");
      final accessToken = await getAccessToken();
      final hasToken = accessToken != null && accessToken.isNotEmpty;
      log("[SecureTokenService] hasTokens result: $hasToken (accessToken length: ${accessToken?.length ?? 0})");
      
      // Cũng check Authentication object
      final authJson = await _storage.read(key: _keyAuthentication);
      final hasAuth = authJson != null && authJson.isNotEmpty;
      log("[SecureTokenService] hasAuthentication: $hasAuth");
      
      return hasToken || hasAuth; // Return true nếu có accessToken HOẶC Authentication object
    } catch (e) {
      log("[SecureTokenService] ❌ hasTokens() error: $e");
      return false;
    }
  }

  /// Kiểm tra xem biometric login đã được bật chưa
  Future<bool> isBiometricEnabled() async {
    try {
      final enabled = await _storage.read(key: _keyBiometricEnabled);
      return enabled == 'true';
    } catch (e) {
      log("isBiometricEnabled() error: $e");
      return false;
    }
  }

  /// Kiểm tra xem access token có hết hạn không
  Future<bool> isAccessTokenExpired() async {
    try {
      final accessToken = await getAccessToken();
      if (accessToken == null || accessToken.isEmpty) {
        return true;
      }
      
      try {
        return JwtDecoder.isExpired(accessToken);
      } catch (e) {
        // Nếu không parse được JWT (token không hợp lệ), coi như đã hết hạn
        log("JWT decode error: $e");
        return true;
      }
    } catch (e) {
      log("isAccessTokenExpired() error: $e");
      return true;
    }
  }

  /// Lấy userCode từ secure storage
  Future<String?> getUserCode() async {
    try {
      return await _storage.read(key: _keyUserCode);
    } catch (e) {
      log("getUserCode() error: $e");
      return null;
    }
  }

  /// Lấy userName từ secure storage
  Future<String?> getUserName() async {
    try {
      return await _storage.read(key: _keyUserName);
    } catch (e) {
      log("getUserName() error: $e");
      return null;
    }
  }

  /// Lấy thông tin user đã lưu (userCode và userName)
  /// Ưu tiên lấy từ Authentication object, nếu không có mới lấy từ các field riêng lẻ
  Future<Map<String, String?>> getUserInfo() async {
    try {
      // Ưu tiên lấy từ Authentication object
      final auth = await getAuthentication();
      if (auth != null) {
        return {
          'userCode': auth.userCode,
          'userName': auth.name,
        };
      }
      
      // Fallback: lấy từ các field riêng lẻ (tương thích với code cũ)
      final userCode = await getUserCode();
      final userName = await getUserName();
      return {
        'userCode': userCode,
        'userName': userName,
      };
    } catch (e) {
      log("getUserInfo() error: $e");
      return {'userCode': null, 'userName': null};
    }
  }

  /// Cập nhật access token (sau khi refresh)
  /// Cũng cập nhật token trong Authentication object nếu có
  Future<void> updateAccessToken(String newAccessToken) async {
    try {
      await _storage.write(key: _keyAccessToken, value: newAccessToken);
      
      // Cập nhật token trong Authentication object nếu có
      final auth = await getAuthentication();
      if (auth != null) {
        auth.accessToken = newAccessToken;
        await saveAuthentication(auth);
        log("Access token updated in Authentication object");
      }
    } catch (e) {
      log("updateAccessToken() error: $e");
      rethrow;
    }
  }

  /// Lưu chữ ký của user vào secure storage (theo userCode)
  /// Chữ ký được lưu dạng Base64 PNG
  Future<void> saveUserSignature({
    required String userCode,
    required String signatureBase64Png,
  }) async {
    try {
      final code = userCode.trim();
      if (code.isEmpty || signatureBase64Png.isEmpty) return;
      
      final key = _keySignatureForUserCode(code);
      await _storage.write(key: key, value: signatureBase64Png);
      log("[SecureTokenService] ✓ User signature saved for userCode: $code");
    } catch (e) {
      log("[SecureTokenService] saveUserSignature() error: $e");
      rethrow;
    }
  }

  /// Lấy chữ ký đã lưu của user (theo userCode)
  /// Trả về null nếu không tìm thấy
  Future<String?> getUserSignature({required String userCode}) async {
    try {
      final code = userCode.trim();
      if (code.isEmpty) return null;
      
      final key = _keySignatureForUserCode(code);
      final signature = await _storage.read(key: key);
      if (signature != null && signature.isNotEmpty) {
        log("[SecureTokenService] ✓ User signature retrieved for userCode: $code");
      }
      return signature;
    } catch (e) {
      log("[SecureTokenService] getUserSignature() error: $e");
      return null;
    }
  }

  /// Xóa chữ ký đã lưu của user (theo userCode)
  Future<void> deleteUserSignature({required String userCode}) async {
    try {
      final code = userCode.trim();
      if (code.isEmpty) return;
      
      final key = _keySignatureForUserCode(code);
      await _storage.delete(key: key);
      log("[SecureTokenService] ✓ User signature deleted for userCode: $code");
    } catch (e) {
      log("[SecureTokenService] deleteUserSignature() error: $e");
    }
  }

  /// Xóa tất cả tokens (khi logout)
  Future<void> clearTokens() async {
    try {
      log("[SecureTokenService] clearTokens() - START - Clearing all tokens from secure storage");
      await _storage.delete(key: _keyAccessToken);
      log("[SecureTokenService] ✓ Deleted access_token");
      await _storage.delete(key: _keyRefreshToken);
      log("[SecureTokenService] ✓ Deleted refresh_token");
      await _storage.delete(key: _keyBiometricEnabled);
      log("[SecureTokenService] ✓ Deleted biometric_enabled");
      await _storage.delete(key: _keyUserCode);
      log("[SecureTokenService] ✓ Deleted user_code");
      await _storage.delete(key: _keyUserName);
      log("[SecureTokenService] ✓ Deleted user_name");
      await _storage.delete(key: _keyAuthentication);
      log("[SecureTokenService] ✓ Deleted authentication object");
      // Clear saved login credentials (passwords)
      try {
        await clearSavedPasswords();
      } catch (e) {
        // Không fail toàn bộ logout nếu không xóa được hết credential keys
        log("[SecureTokenService] ⚠️ clearTokens() - failed clearing password keys: $e");
      }
      log("[SecureTokenService] clearTokens() - SUCCESS - All tokens cleared");
    } catch (e) {
      log("[SecureTokenService] ❌ clearTokens() error: $e");
    }
  }
}
