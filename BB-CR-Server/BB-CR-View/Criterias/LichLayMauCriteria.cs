using BB.CR.Providers.Bases;

namespace BB.CR.Views.Criterias
{
    public class DotLayMauCriteria : BaseCriteria
    {
        public List<long>? DotLayMauIds { get; set; }
        public List<TinhTrangDotLayMau?>? TinhTrangs { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string? Xa { get; set; }
        public string? Huyen { get; set; }
        public string? Tinh { get; set; }
    }
}
