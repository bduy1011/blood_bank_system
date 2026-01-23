import 'package:shared_preferences/shared_preferences.dart';

class PrinterSettings {
  static const _keyMacAddress = 'kiosk_printer_mac';
  static const _keyPaperMm = 'kiosk_printer_paper_mm';

  static Future<String?> getMacAddress() async {
    final prefs = await SharedPreferences.getInstance();
    final mac = prefs.getString(_keyMacAddress);
    if (mac == null || mac.trim().isEmpty) return null;
    return mac.trim();
  }

  static Future<void> setMacAddress(String mac) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_keyMacAddress, mac.trim());
  }

  static Future<int> getPaperMm({int defaultValue = 58}) async {
    final prefs = await SharedPreferences.getInstance();
    final v = prefs.getInt(_keyPaperMm);
    if (v != null && (v == 58 || v == 80)) return v;
    return defaultValue;
  }

  static Future<void> setPaperMm(int mm) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setInt(_keyPaperMm, mm);
  }
}

