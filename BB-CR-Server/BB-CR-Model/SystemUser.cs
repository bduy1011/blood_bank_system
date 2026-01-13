using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class SystemUser
    {
        public required string UserCode { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required AppRole AppRole { get; set; }
        public required bool Active { get; set; }

        public string? DeviceId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public bool AcceptedOtp { get; set; }

        public bool IsDataQLMau { get; set; }

        public string? IdCardNr { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? FireBaseToken { get; set; }
    }
}
