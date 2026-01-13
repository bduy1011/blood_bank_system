using BB.CR.Providers.Bases;

namespace BB.CR.Views
{
    public class GiaoDichConView
    {
        public long? Id { get; set; }
        public NhomMau? MaNhomMau { get; set; }
        public required int SoLuong { get; set; }
        public bool? DaDuyet { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }

        public long? GiaoDichChiTietId { get; set; }

        public int? SoLuongDuyet { get; set; }
        public string? MaNhomMauDescription { get; set; }
    }
}
