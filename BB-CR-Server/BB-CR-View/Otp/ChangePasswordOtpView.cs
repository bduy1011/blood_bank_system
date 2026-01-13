namespace BB.CR.Views.Otp
{
    public class ChangePasswordOtpView
    {
        public required string UserCode { get; set; }
        public string? OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmedNewPassword { get; set; }
    }
}
