using BB.CR.Providers.Bases;

namespace BB.CR.Views.Criterias
{
    public class GiaoDichCriteria : BaseCriteria
    {
        public List<long>? GiaoDichIds { get; set; }
        public List<LoaiPhieu>? LoaiPhieus { get; set; }
        public List<TinhTrangGiaoDich>? TinhTrangs { get; set; }
        public DateTime? NgayTu { get; set; }
        public DateTime? NgayDen { get; set; }
        public string? MaDonViCapMau { get; set; }
    }
}
