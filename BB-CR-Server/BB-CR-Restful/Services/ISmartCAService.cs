namespace BB.CR.Rest.Services
{
    /// <summary>
    /// Service để tích hợp với SmartCA cho chữ ký số
    /// </summary>
    public interface ISmartCAService
    {
        /// <summary>
        /// Xác thực chữ ký số từ SmartCA
        /// </summary>
        /// <param name="signedData">Dữ liệu đã được ký (Base64)</param>
        /// <param name="originalData">Dữ liệu gốc trước khi ký</param>
        /// <param name="certificateSerial">Serial number của chứng chỉ</param>
        /// <returns>Kết quả xác thực</returns>
        Task<SmartCAVerifyResult> VerifySignatureAsync(
            string signedData, 
            string originalData, 
            string? certificateSerial = null);

        /// <summary>
        /// Lấy thông tin chứng chỉ số từ SmartCA
        /// </summary>
        /// <param name="certificateSerial">Serial number của chứng chỉ</param>
        /// <returns>Thông tin chứng chỉ</returns>
        Task<SmartCACertificateInfo?> GetCertificateInfoAsync(string certificateSerial);

        /// <summary>
        /// Kiểm tra chứng chỉ còn hiệu lực không
        /// </summary>
        /// <param name="certificateSerial">Serial number của chứng chỉ</param>
        /// <returns>True nếu còn hiệu lực</returns>
        Task<bool> IsCertificateValidAsync(string certificateSerial);
    }

    /// <summary>
    /// Kết quả xác thực chữ ký số
    /// </summary>
    public class SmartCAVerifyResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
        public SmartCACertificateInfo? CertificateInfo { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }

    /// <summary>
    /// Thông tin chứng chỉ số
    /// </summary>
    public class SmartCACertificateInfo
    {
        public string? SerialNumber { get; set; }
        public string? Issuer { get; set; }
        public string? Subject { get; set; }
        public string? CommonName { get; set; }
        public string? Email { get; set; }
        public string? Organization { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsValid { get; set; }
    }
}
