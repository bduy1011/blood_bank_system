using BB.CR.Providers.Bases;

namespace BB.CR.Views
{
    public class GiaoDichView
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
        public string? LoaiPhieuDescription { get; set; }
        public string? TinhTrangDescription { get; set; }

        public List<GiaoDichChiTietView>? GiaoDichChiTietViews { get; set; }

        public string? DonViCapMauName { get; set; }
        public string? CreatorName { get; set; }
        public string? UpdatorName { get; set; }

        public decimal? TotalApproveQuantity
        {
            get;
            set;
        }
        public decimal? TotalQuantity { get; set; }
    }
}
