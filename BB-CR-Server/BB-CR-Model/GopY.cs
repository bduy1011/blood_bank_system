namespace BB.CR.Models
{
    public class GopY
    {
        public long Id { get; set; }
        public required DateTime Ngay { get; set; }
        public int? NguoiHienMauId { get; set; }
        public required string HoVaTen { get; set; }
        public required string LoaiGopY { get; set; }
        public string? NoiDung { get; set; }
        public string? Email { get; set; }
        public string? SoDT { get; set; }
        public required bool DaXem { get; set; }
        public string? HinhAnh1 { get; set; }
        public string? HinhAnh2 { get; set; }
        public string? HinhAnh3 { get; set; }

        public DMNguoiHienMau? DMNguoiHienMau { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
