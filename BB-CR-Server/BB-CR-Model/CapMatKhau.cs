namespace BB.CR.Models
{
    public class CapMatKhau
    {
        public long Id { get; set; }
        public required string HoTen { get; set; }
        public required string CMND { get; set; }
        public required string SoDT { get; set; }
        public required DateTime CreatedDate { get; set; }
        public string? DeviceId { get; set; }

        public DateTime? ResetPasswordOn { get; set; }
    }
}
