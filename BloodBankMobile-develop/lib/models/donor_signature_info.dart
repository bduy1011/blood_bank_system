class DonorSignatureInfo {
  final bool isSigned;
  final DateTime? signedAt;
  final String? mimeType;
  final String? signatureBase64;

  const DonorSignatureInfo({
    required this.isSigned,
    this.signedAt,
    this.mimeType,
    this.signatureBase64,
  });

  factory DonorSignatureInfo.fromJson(Map<String, dynamic> json) {
    return DonorSignatureInfo(
      isSigned: json['isSigned'] == true,
      signedAt: json['signedAt'] != null
          ? DateTime.tryParse(json['signedAt'].toString())
          : null,
      mimeType: json['mimeType']?.toString(),
      signatureBase64: json['signatureBase64']?.toString(),
    );
  }
}

