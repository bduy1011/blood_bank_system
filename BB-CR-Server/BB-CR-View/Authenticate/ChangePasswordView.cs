namespace BB.CR.Views.Authenticate
{
    public class ChangePasswordView
    {
        public required string UserCode { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
