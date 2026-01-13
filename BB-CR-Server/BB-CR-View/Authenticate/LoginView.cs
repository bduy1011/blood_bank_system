namespace BB.CR.Views.Authenticate
{
    public class LoginView
    {
        public string? UserCode { get; set; }
        public required string Password { get; set; }

        /// <summary>
        /// Địa chỉ MAC đi theo thiết bị
        /// </summary>
        public required string DeviceId { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
