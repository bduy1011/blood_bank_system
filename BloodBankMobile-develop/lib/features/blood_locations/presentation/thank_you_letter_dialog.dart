import 'dart:math' show sqrt;

import 'package:blood_donation/core/localization/app_locale.dart';
import 'package:blood_donation/models/donation_blood_history_response.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';

import '../../../app/app_util/app_center.dart';

/// Dialog thư cảm ơn dạng phong bì hồng, nội dung + logo CR giống mockup.
class ThankYouLetterDialog extends StatelessWidget {
  const ThankYouLetterDialog({
    super.key,
    required this.item,
  });

  final DonationBloodHistoryResponse item;

  static const Color _envelopePink = Color(0xFFDF9E9D);
  static const Color _envelopePinkDark = Color(0xFFDE9493);

  /// Calm medical blue for title (#2F6FD6)
  static const Color _titleBlue = Color.fromARGB(255, 19, 71, 156);
  static const Color _darkBlue = Color.fromARGB(255, 10, 55, 128);

  /// Red for donation info (#E05555)
  static const Color _donationRed = Color(0xFFE05555);

  /// Black for body lines per target UI (#000000)
  static const Color _bodyBlack = Color(0xFF000000);
  static const Color _heartRed = Color(0xFFE53935);
  static const Color _letterWhite = Color(0xFFE4DBD7);

  static Future<void> show(
      BuildContext context, DonationBloodHistoryResponse item) async {
    await showDialog<void>(
      context: context,
      barrierColor: Colors.black54,
      barrierDismissible: true,
      builder: (ctx) => ThankYouLetterDialog(item: item),
    );
  }

  String get _donorName {
    final name = GetIt.instance<AppCenter>().authentication?.name ?? '';
    if (name.isEmpty) return 'Người hiến máu';
    return name.toUpperCase();
  }

  String get _honorific => 'Anh';

  String get _productText => item.tenSanPham ?? '';

  String get _dateText {
    if (item.ngayThu == null) return '';
    return 'ngày ${DateFormat('dd/MM/yyyy').format(item.ngayThu!)}';
  }

  @override
  Widget build(BuildContext context) {
    final w = MediaQuery.sizeOf(context).width;
    final contentWidth = (w * 0.88).clamp(280.0, 400.0);
    const stackH = 300.0;
    const letterInset = 14.0;
    const letterTop = 14.0;
    // Tính toán vị trí top thực tế của lá thư để các phần tử khác (tim) neo theo
    const double actualLetterTop = letterTop - 100;
    const letterBottom = 58.0;
    return Dialog(
      backgroundColor: Colors.transparent,
      insetPadding: const EdgeInsets.symmetric(horizontal: 20),
      child: Material(
        color: Colors.transparent,
        child: Center(
          child: SizedBox(
            width: contentWidth,
            height: stackH,
            child: Transform.rotate(
              angle: -0.02,
              alignment: Alignment.center,
              child: Stack(
                clipBehavior: Clip.none,
                alignment: Alignment.center,
                children: [
                  // 1) Nắp mở lên — nằm dưới lá thư

                  Positioned(
                    left: 0,
                    right: 0,
                    bottom: 0,
                    top: stackH / 2 - _flapH,
                    child: _buildEnvelopeFlap(contentWidth, stackH),
                  ),

                  // 2) Lá thư ở giữa
                  Positioned(
                    left: letterInset,
                    right: letterInset,
                    top: actualLetterTop,
                    bottom: letterBottom - 50,
                    child: Container(
                      decoration: BoxDecoration(
                        boxShadow: [
                          BoxShadow(
                            color: Colors.black.withOpacity(0.12),
                            blurRadius: 8,
                            offset: const Offset(0, 2),
                          ),
                        ],
                      ),
                      child: ClipPath(
                        clipper: _LetterShapeClipper(),
                        child: Container(
                          decoration: const BoxDecoration(
                            color: _letterWhite,
                          ),
                          child: Stack(
                            clipBehavior: Clip.none,
                            children: [
                              Padding(
                                padding:
                                    const EdgeInsets.fromLTRB(20, 18, 20, 24),
                                child: SingleChildScrollView(
                                  child: SizedBox(
                                    width: double.infinity,
                                    child: Column(
                                      mainAxisSize: MainAxisSize.min,
                                      crossAxisAlignment:
                                          CrossAxisAlignment.center,
                                      children: [
                                        Text(
                                          AppLocale.thankYouLetterTitle
                                              .translate(context),
                                          style: const TextStyle(
                                            fontFamily: 'Dancing Script',
                                            fontSize: 30,
                                            fontWeight: FontWeight.bold,
                                            color: _titleBlue,
                                            fontStyle: FontStyle.italic,
                                            letterSpacing: 0.3,
                                          ),
                                          textAlign: TextAlign.center,
                                        ),
                                        const Text(
                                          'Thật tuyệt vời !!!',
                                          style: TextStyle(
                                            fontSize: 15,
                                            color: _darkBlue,
                                            fontWeight: FontWeight.w500,
                                          ),
                                          textAlign: TextAlign.center,
                                        ),
                                        const SizedBox(height: 4),
                                        Text(
                                          '$_honorific $_donorName đã hiến máu thành công',
                                          style: const TextStyle(
                                            fontSize: 14,
                                            color: _darkBlue,
                                            fontWeight: FontWeight.w500,
                                          ),
                                          textAlign: TextAlign.center,
                                        ),
                                        if (_productText.isNotEmpty ||
                                            _dateText.isNotEmpty) ...[
                                          const SizedBox(height: 6),
                                          if (_productText.isNotEmpty)
                                            Text(
                                              _productText,
                                              style: const TextStyle(
                                                fontSize: 14,
                                                color: _donationRed,
                                                fontWeight: FontWeight.w600,
                                              ),
                                              textAlign: TextAlign.center,
                                            ),
                                          if (_productText.isNotEmpty &&
                                              _dateText.isNotEmpty)
                                            const SizedBox(height: 2),
                                          if (_dateText.isNotEmpty)
                                            Text(
                                              _dateText,
                                              style: const TextStyle(
                                                fontSize: 14,
                                                color: _donationRed,
                                                fontWeight: FontWeight.w600,
                                              ),
                                              textAlign: TextAlign.center,
                                            ),
                                        ],
                                        const SizedBox(height: 8),
                                        Padding(
                                          padding: const EdgeInsets.symmetric(
                                              horizontal: 4),
                                          child: Text(
                                            'Cảm ơn $_honorific $_donorName đã sẻ chia giọt máu của mình đến với mọi người!',
                                            style: const TextStyle(
                                              fontSize: 14,
                                              color: _darkBlue,
                                              height: 1.5,
                                              fontWeight: FontWeight.w500,
                                            ),
                                            textAlign: TextAlign.center,
                                            maxLines: 4,
                                          ),
                                        ),
                                        const SizedBox(height: 5),
                                        _buildLogo(),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ),

                  // 3) Thân che lá thư — vẽ trên thư, có khuyết đúng chỗ lá thư
                  Positioned(
                    left: 0,
                    right: 0,
                    top: stackH * 0.5,
                    child: _buildEnvelopeBodyOverlay(
                      contentWidth,
                      stackH,
                    ),
                  ),
                  Positioned(
                    top: stackH * 0.4 + 140, // = cy trong painter
                    left: contentWidth / 2 - 10, // = cx
                    child: Transform.rotate(
                      angle: -0.15, // Nghiêng sang trái ~15 độ
                      child: const Icon(
                        Icons.favorite,
                        color: Color(0xFFE53935),
                        size: 30,
                      ),
                    ),
                  ),

                  // 2 trái tim góc phải lá thư - luôn đi theo đỉnh trên phải của lá thư
                  Positioned(
                    // 30px above letter top
                    top: actualLetterTop - 30,
                    // 8px left of letter's visual right edge
                    right: letterInset + 8,
                    child: Transform.rotate(
                      angle: -0.22,
                      child: const Icon(Icons.favorite,
                          color: _heartRed, size: 20),
                    ),
                  ),
                  Positioned(
                    // 10px above letter top
                    top: actualLetterTop - 10,
                    // 15px right of letter's visual right edge (floating out)
                    right: letterInset - 15,
                    child: Transform.rotate(
                      angle: 0.2,
                      child: const Icon(Icons.favorite,
                          color: _heartRed, size: 35),
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

  static const double _flapH = 120.0;

  /// Tính toán vị trí Y của đỉnh trái để cạnh trái bằng cạnh phải
  static double _calculateLeftTopY(double contentWidth, double envelopeH) {
    final w = contentWidth;
    final h = envelopeH * 0.7; // realHeight

    // Tính toán giống trong _EnvelopeBodyOverlayPainter
    const cot85 = 0.0875;
    const tan5 = 0.0875;

    const denomL = (1 / cot85) + tan5;
    final blX = (h + tan5 * w / 2) / denomL;
    final blY = blX / cot85;

    final brX = (h + w / cot85 + tan5 * w / 2) / denomL;
    final brY = (brX - w) / cot85;

    final rightLen = sqrt((brX - w) * (brX - w) + brY * brY);
    final tlY = blY - sqrt((rightLen * rightLen - blX * blX).abs());

    return tlY;
  }

  /// Nắp phong bì (tam giác gập ra, mở lên) — nằm dưới lá thư.
  Widget _buildEnvelopeFlap(double contentWidth, double envelopeH) {
    final leftTopY = _calculateLeftTopY(contentWidth, envelopeH);

    return SizedBox(
      width: contentWidth,
      height: envelopeH,
      child: CustomPaint(
        size: Size(contentWidth, envelopeH),
        painter: _EnvelopeFlapPainter(
          flapColor: _envelopePinkDark,
          flapH: _flapH,
          leftTopY: leftTopY,
        ),
      ),
    );
  }

  /// Thân phong bì (3 tam giác) vẽ trên lá thư, có khuyết chỗ thư để thư lộ ra.
  Widget _buildEnvelopeBodyOverlay(double contentWidth, double envelopeH) {
    // Chiều cao thực của phần vẽ (tương ứng với baseY = h * 0.7 cũ)
    final realHeight = envelopeH * 0.7;

    return CustomPaint(
      size: Size(contentWidth, realHeight),
      painter: _EnvelopeBodyOverlayPainter(
        bodyColor: _envelopePink,
      ),
    );
  }

  static const String _crIconAsset = 'assets/icons/app_cr_icon.png';

  Widget _buildLogo() {
    return SizedBox(
      width: 90,
      height: 90,
      child: Image.asset(
        _crIconAsset,
        fit: BoxFit.contain,
      ),
    );
  }
}

/// Hình lá thư: cạnh trái đỉnh trên gần trái hơn đỉnh dưới, cạnh phải đỉnh trên gần phải hơn đỉnh dưới, cạnh trên đỉnh trái thấp hơn đỉnh phải.
class _LetterShapeClipper extends CustomClipper<Path> {
  @override
  Path getClip(Size size) {
    const topDip = 0.0;
    const sideSlant = 10.0;
    final w = size.width;
    final h = size.height;
    return Path()
      ..moveTo(0, topDip)
      ..lineTo(w, 0)
      ..lineTo(w - sideSlant, h)
      ..lineTo(sideSlant, h)
      ..close();
  }

  @override
  bool shouldReclip(covariant CustomClipper<Path> oldClipper) => false;
}

/// Chỉ vẽ nắp phong bì (tam giác gập ra).
class _EnvelopeFlapPainter extends CustomPainter {
  _EnvelopeFlapPainter({
    required this.flapColor,
    required this.flapH,
    required this.leftTopY,
  });

  final Color flapColor;
  final double flapH;
  final double leftTopY;

  @override
  void paint(Canvas canvas, Size size) {
    final w = size.width;
    final cx = w * 0.5;
    final flapPath = Path()
      ..moveTo(0, flapH + leftTopY)
      ..lineTo(cx, leftTopY)
      ..lineTo(w, flapH)
      ..close();
    canvas.drawPath(flapPath, Paint()..color = flapColor);

    // Tam giác dưới đối xứng
    final bottomFlapPath = Path()
      ..moveTo(0, flapH + leftTopY)
      ..lineTo(cx, flapH * 2)
      ..lineTo(w, flapH)
      ..close();
    canvas.drawPath(bottomFlapPath, Paint()..color = flapColor);

    final stroke = Paint()
      ..color = flapColor.withOpacity(0.7)
      ..style = PaintingStyle.stroke
      ..strokeWidth = 1;
    canvas.drawLine(Offset(0, flapH + leftTopY), Offset(cx, leftTopY), stroke);
    canvas.drawLine(Offset(cx, leftTopY), Offset(w, flapH), stroke);

    // Stroke dưới
    canvas.drawLine(Offset(0, flapH + leftTopY), Offset(cx, flapH * 2), stroke);
    canvas.drawLine(Offset(cx, flapH * 2), Offset(w, flapH), stroke);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => false;
}

/// Thân = 3 tam giác đỉnh hướng vào nhau, đáy có khuyết tam giác ngược (inverted V), trừ vùng lá thư.
class _EnvelopeBodyOverlayPainter extends CustomPainter {
  _EnvelopeBodyOverlayPainter({required this.bodyColor});
  final Color bodyColor;

  @override
  void paint(Canvas canvas, Size size) {
    final w = size.width;
    final h = size.height;

    final cx = w / 2;
    // Tỷ lệ đỉnh V: cy = h * (4/7)
    final cy = h * (4 / 7);

    const cot85 = 0.0875;
    const tan5 = 0.0875;

    const denomL = (1 / cot85) + tan5;
    final blX = (h + tan5 * w / 2) / denomL;
    final blY = blX / cot85;
    final blPoint = Offset(blX, blY);

    // BR: x = w + y * cot85 ; y - h = -tan5 * (x - w/2)
    final brX = (h + w / cot85 + tan5 * w / 2) / denomL;
    final brY = (brX - w) / cot85;
    final brPoint = Offset(brX, brY);

    const radius = 20.0;

    // 2) TAM GIÁC PHẢI (Right Triangle) - Tính trước để lấy độ dài
    final trPoint = Offset(w, 0);
    final rightVec = brPoint - trPoint;
    final rightLen = rightVec.distance;
    final stopRight = brPoint - rightVec * (radius * 1.5 / rightLen);

    // 1) TAM GIÁC TRÁI (Left Triangle)
    // Tính toán điểm bắt đầu sao cho cạnh trái có độ dài bằng cạnh phải
    // Cạnh trái từ (0, tlY) đến (blX, blY)
    // Độ dài: sqrt(blX^2 + (blY - tlY)^2) = rightLen
    // => (blY - tlY)^2 = rightLen^2 - blX^2
    // => tlY = blY - sqrt(rightLen^2 - blX^2)
    final tlY = blY - sqrt((rightLen * rightLen - blX * blX).abs());

    final leftLen = (Offset(blX, blY) - Offset(0, tlY)).distance;
    // Điểm dừng trên cạnh trái để nhường chỗ cho bo tròn
    final stopLeft = Offset(blX * (leftLen - radius * 1.5) / leftLen,
        tlY + (blY - tlY) * (leftLen - radius * 1.5) / leftLen);

    final leftTriangle = Path()
      ..moveTo(0, tlY)
      ..lineTo(stopLeft.dx, stopLeft.dy)
      ..lineTo(cx, cy)
      ..close();

    canvas.drawPath(leftTriangle, Paint()..color = bodyColor);

    final rightTriangle = Path()
      ..moveTo(w, 0)
      ..lineTo(stopRight.dx, stopRight.dy)
      ..lineTo(cx, cy)
      ..close();

    canvas.drawPath(rightTriangle, Paint()..color = bodyColor);

    // 3) TAM GIÁC DƯỚI (Bottom Triangle) - Chứa bo góc
    final bottomPath = Path()
      ..moveTo(cx, cy)
      ..lineTo(stopLeft.dx, stopLeft.dy)
      // Bo góc BL qua đỉnh BL
      ..quadraticBezierTo(
          blX, blY, blX + (brX - blX) * 0.1, blY + (brY - blY) * 0.1)
      // Line đến gần BR
      ..lineTo(brX - (brX - blX) * 0.1, brY - (brY - blY) * 0.1)
      // Bo góc BR qua đỉnh BR
      ..quadraticBezierTo(brX, brY, stopRight.dx, stopRight.dy)
      ..close();

    canvas.drawPath(
      bottomPath,
      Paint()..color = ThankYouLetterDialog._envelopePinkDark,
    );
  }

  @override
  bool shouldRepaint(_) => false;
}
