import 'dart:convert';
import 'dart:typed_data';
import 'dart:ui' as ui;

import 'package:blood_donation/utils/extension/context_ext.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

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

  bool get _hasInk => _points.any((p) => p != null);

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
      ..strokeWidth = 3.0
      ..strokeCap = StrokeCap.round
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
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const SizedBox(height: 6),
                Text(
                  'Vui lòng ký vào khung bên dưới.',
                  style: context.myTheme.textThemeT1.body.copyWith(
                    color: Colors.black87,
                  ),
                ),
                const SizedBox(height: 12),
                Expanded(
                  child: Container(
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(14),
                      border: Border.all(color: Colors.black12),
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black.withOpacity(0.05),
                          blurRadius: 10,
                          offset: const Offset(0, 6),
                        )
                      ],
                    ),
                    child: ClipRRect(
                      borderRadius: BorderRadius.circular(14),
                      child: GestureDetector(
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
                            // IMPORTANT: pass a new list instance so painter repaints
                            painter: _SignaturePainter(List<Offset?>.from(_points)),
                            child: const SizedBox.expand(),
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
                const SizedBox(height: 12),
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton.icon(
                    onPressed: _save,
                    icon: const Icon(Icons.save),
                    label: const Text('Lưu chữ ký'),
                    style: ElevatedButton.styleFrom(
                      backgroundColor: const Color(0xFFB22C2D),
                      foregroundColor: Colors.white,
                      padding: const EdgeInsets.symmetric(vertical: 14),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(14),
                      ),
                    ),
                  ),
                ),
                const SizedBox(height: 8),
                Text(
                  'Bạn có thể bấm “Xoá” để ký lại trước khi lưu.',
                  style: context.myTheme.textThemeT1.body.copyWith(
                    color: Colors.black54,
                    fontSize: 12,
                  ),
                ),
              ],
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
      ..strokeWidth = 3.0
      ..strokeCap = StrokeCap.round
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

