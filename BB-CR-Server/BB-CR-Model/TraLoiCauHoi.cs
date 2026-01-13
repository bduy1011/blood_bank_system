namespace BB.CR.Models
{
    public class TraLoiCauHoi
    {
        public long Id { get; set; }
        public required DateTime Ngay { get; set; }
        public string? GhiChu { get; set; }

        public List<TraLoiCauHoiChiTiet>? TraLoiCauHoiChiTiets { get; set; }
        public DangKyHienMau? DangKyHienMau { get; set; }
    }
}
