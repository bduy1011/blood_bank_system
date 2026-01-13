namespace BB.CR.Views.Criterias
{
    public class LichSuHienMauCriteria : BaseCriteria
    {
        public List<int>? NguoiHienMauIds { get; set; }
        public List<long>? LichLayMauIds { get; set; }
        public List<string>? MaVachIds { get; set; }
        public DateTime? NgayThuTu { get; set; }
        public DateTime? NgayThuDen { get; set; }
        public List<string>? MaSanPhams { get; set; }
        public List<string>? MaNhomMaus { get; set; }
        public bool? IsSanLocAmTinh { get; set; }
        public bool? IsSanLocDuongTinh { get; set; }
        public bool? IsSanLocKhongXacDinh { get; set; }

        public List<long>? DotHienMauIds { get; set; }
    }
}
