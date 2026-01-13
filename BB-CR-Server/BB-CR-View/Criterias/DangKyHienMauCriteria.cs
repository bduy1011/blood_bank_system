using BB.CR.Providers.Bases;

namespace BB.CR.Views.Criterias
{
    public class DangKyHienMauCriteria : BaseCriteria
    {
        public List<long>? Ids { get; set; }
        public DateTime? NgayTu { get; set; }
        public DateTime? NgayDen { get; set; }
        public List<int>? NguoiHienMauIds { get; set; }
        public List<TinhTrangDangKyHienMau>? TinhTrangs { get; set; }
        public string? HoTen { get; set; }
        public int? NamSinh { get; set; }
        public string? CMND { get; set; }
        public string? Tinh { get; set; }
        public string? Xa { get; set; }
        public string? Huyen { get; set; }
        public string? Site { get; set; }

        public string? MaDonViCapMau { get; set; }

        public bool? IsLoadAll { get; set; }

        public List<long>? DotLayMauIds { get; set; }
    }
}
