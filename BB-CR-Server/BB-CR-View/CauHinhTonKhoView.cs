using BB.CR.Providers.Bases;

namespace BB.CR.Views
{
    public class CauHinhTonKhoView
    {
        public long Id { get; set; }
        public required DateTime Ngay { get; set; }

        public List<CauHinhTonKhoChiTietView>? CauHinhTonKhoChiTietViews { get; set; }
    }

    public class CauHinhTonKhoChiTietView 
    {
        public long Id { get; set; }
        public long? CauHinhTonKhoId { get; set; }
        public required LoaiSanPham LoaiSanPham { get; set; }
        public string? LoaiSanPhamDescription { get; set; }

        public List<CauHinhTonKhoChiTietConView>? CauHinhTonKhoChiTietConViews { get; set; }
    }

    public class CauHinhTonKhoChiTietConView
    {
        public long Id { get; set; }
        public long? CauHinhTonKhoChiTietId { get; set; }

        public required int SoLuong { get; set; }
        public required int SoLuongDuocBooking { get; set; }

        public required NhomMau MaNhomMau { get; set; }
        public string? MaNhomMauDescription { get; set; }
    }
}
