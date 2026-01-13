namespace BB.CR.Views.Criterias
{
    public class GiaoDichChiTietCriteria : BaseCriteria
    {
        public List<long?>? GiaoDichChiTietIds { get; set; }
        public List<long?>? GiaoDichIds { get; set; }
        public bool? DaDuyet { get; set; }
        public string? LoaiSanPham { get; set; }
    }
}
