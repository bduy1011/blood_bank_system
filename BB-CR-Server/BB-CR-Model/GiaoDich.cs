using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class GiaoDich
    {
        public long GiaoDichId { get; set; }
        public required LoaiPhieu LoaiPhieu { get; set; }
        public required TinhTrangGiaoDich TinhTrang { get; set; }
        public required DateTime Ngay { get; set; }
        public string? MaDonViCapMau { get; set; }
        public string? GhiChu { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsLocked { get; set; }

        public List<GiaoDichChiTiet>? GiaoDichChiTiets { get; set; }
        public DMDonViCapMau? DonViCapMau { get; set; }
    }
}
