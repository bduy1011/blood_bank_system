namespace BB.CR.Views
{
    public class DonorSignatureInfoView
    {
        public bool IsSigned { get; set; }
        public DateTime? SignedAt { get; set; }
        public string? MimeType { get; set; }
        public string? SignatureBase64 { get; set; }
    }

    public class DonorSignatureSaveRequest
    {
        public string? SignatureBase64 { get; set; }
        public string? MimeType { get; set; }
        public bool UpdateStatusToDaTiepNhan { get; set; } = true;
    }
}

