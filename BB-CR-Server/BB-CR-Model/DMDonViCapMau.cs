namespace BB.CR.Models
{
    public class DMDonViCapMau
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
        //public string? Password { get; set; }
        //public string? SecureKey { get; set; }
        public List<GiaoDich>? GiaoDichs { get; set; }

        //public required bool IsHienThiApp { get; set; }
        //public List<SystemUser>? SystemUsers { get; set; } // Remove 20240927
        //public List<DangKyHienMau>? DangKyHienMaus { get; set; }
    }
}
