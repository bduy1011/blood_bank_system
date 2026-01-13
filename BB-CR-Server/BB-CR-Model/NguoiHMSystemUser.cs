namespace BB.CR.Models
{
    public class NguoiHMSystemUser
    {
        public long Id { get; set; }
        public string? PhoneNumber { get; set; }
        public int? DMNguoiHienMauId { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
