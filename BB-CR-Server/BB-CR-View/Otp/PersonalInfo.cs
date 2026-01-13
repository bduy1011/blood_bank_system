namespace BB.CR.Views.Otp
{
    public class PersonalInfo
    {
        public required string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public required string IdCard { get; set; }
        public required string PhoneNr { get; set; }
    }
}
