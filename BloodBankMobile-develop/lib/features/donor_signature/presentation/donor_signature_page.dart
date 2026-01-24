import 'dart:convert';
import 'dart:typed_data';
import 'dart:ui' as ui;

import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:blood_donation/utils/secure_token_service.dart';
import 'package:blood_donation/app/app_util/app_center.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:get_it/get_it.dart';

class DonorSignatureResult {
  final Uint8List pngBytes;

  const DonorSignatureResult(this.pngBytes);

  String get base64Png => base64Encode(pngBytes);
}

class DonorSignaturePage extends StatefulWidget {
  const DonorSignaturePage({
    super.key,
    this.title,
  });

  final String? title;

  @override
  State<DonorSignaturePage> createState() => _DonorSignaturePageState();
}

class _DonorSignaturePageState extends State<DonorSignaturePage> {
  final GlobalKey _padKey = GlobalKey();
  final List<Offset?> _points = <Offset?>[];
  final SecureTokenService _tokenService = SecureTokenService();
  final appCenter = GetIt.instance<AppCenter>();
  
  bool _hasSavedSignature = false;
  Uint8List? _savedSignatureBytes;

  bool get _hasInk => _points.any((p) => p != null);

  @override
  void initState() {
    super.initState();
    _loadSavedSignature();
  }

  Future<void> _loadSavedSignature() async {
    final userCode = appCenter.authentication?.userCode;
    if (userCode == null || userCode.isEmpty) return;

    // 1. Kiểm tra local storage trước (nhanh)
    var savedSignature = await _tokenService.getUserSignature(userCode: userCode);
    
    // 2. Nếu không có trong local, lấy từ server theo identityCard/userCode
    if (savedSignature == null || savedSignature.isEmpty) {
      try {
        final response = await appCenter.backendProvider.getUserSignature(includeImage: true);
        if (response?.status == 200 &&
            response?.data != null &&
            response!.data!.isSigned == true &&
            response.data!.signatureBase64 != null &&
            response.data!.signatureBase64!.isNotEmpty) {
          savedSignature = response.data!.signatureBase64!;
          
          // Lưu vào local storage để lần sau không cần lấy lại
          try {
            await _tokenService.saveUserSignature(
              userCode: userCode,
              signatureBase64Png: savedSignature,
            );
          } catch (e) {
            // Ignore save errors
          }
        }
      } catch (e) {
        // Ignore errors, tiếp tục với local storage
      }
    }

    // 3. Hiển thị chữ ký nếu có
    if (savedSignature != null && savedSignature.isNotEmpty) {
      try {
        final bytes = base64Decode(savedSignature);
        setState(() {
          _hasSavedSignature = true;
          _savedSignatureBytes = bytes;
        });
      } catch (e) {
        // Ignore decode errors
      }
    }
  }

  Future<void> _useSavedSignature() async {
    if (_savedSignatureBytes != null) {
      Get.back(result: DonorSignatureResult(_savedSignatureBytes!));
    }
  }

  Future<Uint8List?> _exportPng() async {
    final renderObject = _padKey.currentContext?.findRenderObject();
    if (renderObject is! RenderBox) return null;

    final size = renderObject.size;
    if (size.isEmpty) return null;

    final recorder = ui.PictureRecorder();
    final canvas = Canvas(recorder);

    // White background
    final bgPaint = Paint()..color = Colors.white;
    canvas.drawRect(Offset.zero & size, bgPaint);

    // Draw strokes
    final paint = Paint()
      ..color = Colors.black
      ..strokeWidth = 3.5
      ..strokeCap = StrokeCap.round
      ..strokeJoin = StrokeJoin.round
      ..style = PaintingStyle.stroke;

    for (int i = 0; i < _points.length - 1; i++) {
      final p1 = _points[i];
      final p2 = _points[i + 1];
      if (p1 != null && p2 != null) {
        canvas.drawLine(p1, p2, paint);
      }
    }

    final picture = recorder.endRecording();
    final img = await picture.toImage(size.width.ceil(), size.height.ceil());
    final bytes = await img.toByteData(format: ui.ImageByteFormat.png);
    return bytes?.buffer.asUint8List();
  }

  void _clear() {
    setState(_points.clear);
  }

  Future<void> _save() async {
    if (!_hasInk) {
      Get.snackbar(
        'Chưa có chữ ký',
        'Vui lòng ký vào khung trước khi lưu.',
        snackPosition: SnackPosition.BOTTOM,
      );
      return;
    }

    final png = await _exportPng();
    if (png == null || png.isEmpty) {
      Get.snackbar(
        'Lỗi',
        'Không xuất được ảnh chữ ký. Vui lòng thử lại.',
        snackPosition: SnackPosition.BOTTOM,
      );
      return;
    }

    Get.back(result: DonorSignatureResult(png));
  }

  @override
  Widget build(BuildContext context) {
    final title = widget.title ?? 'Ký xác nhận';

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
            title,
            style: context.myTheme.textThemeT1.title.copyWith(
              color: Colors.white,
            ),
          ),
          centerTitle: true,
          backgroundColor: Colors.transparent,
          elevation: 0,
          leading: IconButton(
            icon: const Icon(Icons.close, color: Colors.white),
            onPressed: () => Get.back(),
          ),
          actions: [
            TextButton.icon(
              onPressed: _clear,
              icon: const Icon(Icons.delete_outline, color: Colors.white),
              label: const Text(
                'Xoá',
                style: TextStyle(color: Colors.white),
              ),
            ),
          ],
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
            child: Padding(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Header section
                  Row(
                    children: [
                      Container(
                        padding: const EdgeInsets.all(10),
                        decoration: BoxDecoration(
                          color: const Color(0xFFB22C2D).withOpacity(0.1),
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: const Icon(
                          Icons.edit_note,
                          color: Color(0xFFB22C2D),
                          size: 24,
                        ),
                      ),
                      const SizedBox(width: 12),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Ký xác nhận tiếp nhận',
                              style: context.myTheme.textThemeT1.title.copyWith(
                                fontWeight: FontWeight.bold,
                                fontSize: 18,
                              ),
                            ),
                            const SizedBox(height: 4),
                            Text(
                              'Vui lòng ký vào khung bên dưới',
                              style: context.myTheme.textThemeT1.body.copyWith(
                                color: Colors.black54,
                                fontSize: 13,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                  // Hiển thị option dùng chữ ký đã lưu nếu có
                  if (_hasSavedSignature) ...[
                    const SizedBox(height: 20),
                    Container(
                      padding: const EdgeInsets.all(16),
                      decoration: BoxDecoration(
                        color: const Color(0xFFB22C2D).withOpacity(0.05),
                        borderRadius: BorderRadius.circular(16),
                        border: Border.all(
                          color: const Color(0xFFB22C2D).withOpacity(0.2),
                          width: 1.5,
                        ),
                      ),
                      child: Column(
                        children: [
                          Row(
                            children: [
                              Container(
                                padding: const EdgeInsets.all(8),
                                decoration: BoxDecoration(
                                  color: const Color(0xFFB22C2D).withOpacity(0.1),
                                  borderRadius: BorderRadius.circular(8),
                                ),
                                child: const Icon(
                                  Icons.history,
                                  color: Color(0xFFB22C2D),
                                  size: 20,
                                ),
                              ),
                              const SizedBox(width: 12),
                              Expanded(
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      'Chữ ký đã lưu',
                                      style: context.myTheme.textThemeT1.body.copyWith(
                                        fontWeight: FontWeight.w600,
                                        fontSize: 14,
                                      ),
                                    ),
                                    Text(
                                      'Bạn có thể dùng lại chữ ký đã lưu trước đó',
                                      style: context.myTheme.textThemeT1.body.copyWith(
                                        color: Colors.black54,
                                        fontSize: 12,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ),
                          const SizedBox(height: 12),
                          SizedBox(
                            width: double.infinity,
                            child: ElevatedButton.icon(
                              onPressed: _useSavedSignature,
                              icon: const Icon(Icons.check_circle, size: 20),
                              label: const Text(
                                'Dùng chữ ký đã lưu',
                                style: TextStyle(
                                  fontSize: 15,
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                              style: ElevatedButton.styleFrom(
                                backgroundColor: const Color(0xFFB22C2D),
                                foregroundColor: Colors.white,
                                padding: const EdgeInsets.symmetric(vertical: 14),
                                shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(12),
                                ),
                                elevation: 0,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                    const SizedBox(height: 20),
                    Row(
                      children: [
                        Expanded(child: Divider(color: Colors.grey[300], thickness: 1)),
                        Padding(
                          padding: const EdgeInsets.symmetric(horizontal: 16),
                          child: Text(
                            'HOẶC KÝ MỚI',
                            style: context.myTheme.textThemeT1.body.copyWith(
                              color: Colors.black54,
                              fontSize: 11,
                              fontWeight: FontWeight.w600,
                              letterSpacing: 1,
                            ),
                          ),
                        ),
                        Expanded(child: Divider(color: Colors.grey[300], thickness: 1)),
                      ],
                    ),
                  ],
                  const SizedBox(height: 20),
                  // Signature pad
                  Expanded(
                    child: Container(
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(20),
                        border: Border.all(
                          color: Colors.grey[300]!,
                          width: 2,
                        ),
                        boxShadow: [
                          BoxShadow(
                            color: Colors.black.withOpacity(0.08),
                            blurRadius: 20,
                            offset: const Offset(0, 8),
                          ),
                        ],
                      ),
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(18),
                        child: Stack(
                          children: [
                            // Background pattern (subtle lines)
                            CustomPaint(
                              painter: _BackgroundPatternPainter(),
                              child: const SizedBox.expand(),
                            ),
                            // Signature canvas
                            GestureDetector(
                              onPanStart: (d) {
                                final box = _padKey.currentContext?.findRenderObject();
                                if (box is RenderBox) {
                                  setState(() {
                                    _points.add(box.globalToLocal(d.globalPosition));
                                  });
                                }
                              },
                              onPanUpdate: (d) {
                                final box = _padKey.currentContext?.findRenderObject();
                                if (box is RenderBox) {
                                  setState(() {
                                    _points.add(box.globalToLocal(d.globalPosition));
                                  });
                                }
                              },
                              onPanEnd: (_) => setState(() => _points.add(null)),
                              child: RepaintBoundary(
                                key: _padKey,
                                child: CustomPaint(
                                  painter: _SignaturePainter(List<Offset?>.from(_points)),
                                  child: Container(
                                    color: Colors.transparent,
                                    child: _hasInk
                                        ? null
                                        : Center(
                                            child: Column(
                                              mainAxisSize: MainAxisSize.min,
                                              children: [
                                                Icon(
                                                  Icons.edit_note,
                                                  size: 48,
                                                  color: Colors.grey[300],
                                                ),
                                                const SizedBox(height: 8),
                                                Text(
                                                  'Ký vào đây',
                                                  style: context.myTheme.textThemeT1.body.copyWith(
                                                    color: Colors.grey[400],
                                                    fontSize: 14,
                                                  ),
                                                ),
                                              ],
                                            ),
                                          ),
                                  ),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: 20),
                  // Action buttons
                  Row(
                    children: [
                      Expanded(
                        flex: 2,
                        child: ElevatedButton.icon(
                          onPressed: _save,
                          icon: const Icon(Icons.check_circle, size: 22),
                          label: const Text(
                            'Lưu chữ ký',
                            style: TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                          style: ElevatedButton.styleFrom(
                            backgroundColor: const Color(0xFFB22C2D),
                            foregroundColor: Colors.white,
                            padding: const EdgeInsets.symmetric(vertical: 16),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(16),
                            ),
                            elevation: 2,
                          ),
                        ),
                      ),
                      const SizedBox(width: 12),
                      Expanded(
                        child: OutlinedButton.icon(
                          onPressed: _hasInk ? _clear : null,
                          icon: const Icon(Icons.refresh, size: 20),
                          label: const Text(
                            'Xóa',
                            style: TextStyle(fontSize: 14),
                          ),
                          style: OutlinedButton.styleFrom(
                            foregroundColor: Colors.black87,
                            side: BorderSide(color: Colors.grey[400]!),
                            padding: const EdgeInsets.symmetric(vertical: 16),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(16),
                            ),
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 12),
                  // Note
                  Container(
                    padding: const EdgeInsets.all(12),
                    decoration: BoxDecoration(
                      color: Colors.blue[50],
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(color: Colors.blue[100]!),
                    ),
                    child: Row(
                      children: [
                        Icon(Icons.info_outline, size: 18, color: Colors.blue[700]),
                        const SizedBox(width: 8),
                        Expanded(
                          child: Text(
                            'Bạn có thể bấm "Xóa" để ký lại trước khi lưu.',
                            style: context.myTheme.textThemeT1.body.copyWith(
                              color: Colors.blue[900],
                              fontSize: 12,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class _SignaturePainter extends CustomPainter {
  _SignaturePainter(this.points);

  final List<Offset?> points;

  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = Colors.black
      ..strokeWidth = 3.5
      ..strokeCap = StrokeCap.round
      ..strokeJoin = StrokeJoin.round
      ..style = PaintingStyle.stroke;

    for (int i = 0; i < points.length - 1; i++) {
      final p1 = points[i];
      final p2 = points[i + 1];
      if (p1 != null && p2 != null) {
        canvas.drawLine(p1, p2, paint);
      }
    }
  }

  @override
  bool shouldRepaint(covariant _SignaturePainter oldDelegate) =>
      oldDelegate.points != points;
}

class _BackgroundPatternPainter extends CustomPainter {
  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = Colors.grey[200]!
      ..strokeWidth = 0.5;

    // Vẽ đường kẻ ngang mờ
    const lineSpacing = 30.0;
    for (double y = lineSpacing; y < size.height; y += lineSpacing) {
      canvas.drawLine(
        Offset(0, y),
        Offset(size.width, y),
        paint,
      );
    }
  }

  @override
  bool shouldRepaint(covariant _BackgroundPatternPainter oldDelegate) => false;
}
