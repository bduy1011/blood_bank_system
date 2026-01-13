namespace BB.CR.Views
{
    public class DMDonViCapMauView
    {
        public required string MaDonVi { get; set; }
        public required string TenDonVi { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public required bool IsBvNhi { get; set; }
        public required bool IsDonViCapMau { get; set; }
        public required bool IsDonViNhanMau { get; set; }
        public required int UuTien { get; set; }
        public required bool Active { get; set; }
        public required bool IsHienThiApp { get; set; }
    }
}
