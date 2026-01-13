using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class DangKyHienMau
    {
        public long Id { get; set; }
        public required DateTime Ngay { get; set; }
        public int? NguoiHienMauId { get; set; }
        public required string HoVaTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? NamSinh { get; set; }
        public string? CMND { get; set; }
        public bool? GioiTinh { get; set; }
        public string? MaXa { get; set; }
        public string? TenXa { get; set; }
        public string? MaHuyen { get; set; }
        public string? TenHuyen { get; set; }
        public string? MaTinh { get; set; }
        public string? TenTinh { get; set; }
        public string? DiaChiLienLac { get; set; }
        public string? DiaChiThuongTru { get; set; }
        public string? NgheNghiep { get; set; }
        public string? Email { get; set; }
        public string? SoDT { get; set; }
        public string? Site { get; set; }
        public required TinhTrangDangKyHienMau TinhTrang { get; set; }
        public string? CoQuan { get; set; }
        public long? TraLoiCauHoiId { get; set; }
        public TraLoiCauHoi? TraLoiCauHoi { get; set; }
        public required long DotLayMauId { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
