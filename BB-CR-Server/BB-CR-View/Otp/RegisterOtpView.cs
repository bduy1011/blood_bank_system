namespace BB.CR.Views.Otp
{
    public class RegisterOtpView
    {
        public required string FullName { get; set; }
        public required string CMND { get; set; }
        public string? OtpCode { get; set; }
        public string? DeviceId { get; set; }
    }
}
