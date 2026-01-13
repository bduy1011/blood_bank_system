namespace BB.CR.Views.Criterias
{
    public class GopYCriteria : BaseCriteria
    {
        public List<long?>? Ids { get; set; }
        public DateTime? NgayTu { get; set; }
        public DateTime? NgayDen { get; set; }
        public string? LoaiGopY { get; set; }
        public string? Email { get; set; }
        public string? SoDT { get; set; }
        public bool? DaXem { get; set; }
    }
}
