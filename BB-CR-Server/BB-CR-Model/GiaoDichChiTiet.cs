using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class GiaoDichChiTiet
    {
        public long GiaoDichChiTietId { get; set; }
        public long? GiaoDichId { get; set; }
        public required LoaiSanPham LoaiSanPham { get; set; }
        public string? DienGiai { get; set; }

        public GiaoDich? GiaoDich { get; set; }
        public List<GiaoDichCon>? GiaoDichCons { get; set; }
    }
}
