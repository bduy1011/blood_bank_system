namespace BB.CR.Views.Authenticate
{
    public class RegisterView
    {
        public required string UserCode { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required bool IsMaHoa { get; set; }
        public string? DeviceId { get; set; }
    }
}
