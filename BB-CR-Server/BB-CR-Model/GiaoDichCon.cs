using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class GiaoDichCon
    {
        public long Id { get; set; }
        public NhomMau? MaNhomMau { get; set; }
        public required int SoLuong { get; set; }

        public long? GiaoDichChiTietId { get; set; }
        public GiaoDichChiTiet? GiaoDichChiTiet { get; set; }

        public int? SoLuongDuyet { get; set; }
    }
}
