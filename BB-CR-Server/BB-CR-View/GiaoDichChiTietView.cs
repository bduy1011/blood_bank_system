using BB.CR.Providers.Bases;

namespace BB.CR.Views
{
    public class GiaoDichChiTietView
    {
        public long GiaoDichChiTietId { get; set; }
        public long? GiaoDichId { get; set; }
        public required LoaiSanPham LoaiSanPham { get; set; }
        public string? DienGiai { get; set; }

        public string? LoaiSanPhamDescription { get; set; }
        public List<GiaoDichConView>? GiaoDichConViews { get; set; }
    }
}
