using BB.CR.Providers.Bases;

namespace BB.CR.Views.Criterias
{
    public class CauHinhTonKhoCriteria : BaseCriteria
    {
        public List<long?>? Ids { get; set; }
        public DateTime? NgayTu { get; set; }
        public DateTime? NgayDen { get; set; }
        public LoaiSanPham? LoaiSanPham { get; set; }
    }
}
