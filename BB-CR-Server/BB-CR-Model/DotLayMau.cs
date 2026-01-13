using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class DotLayMau
    {
        public long DotLayMauId { get; set; }
        public required LoaiDotLayMau LoaiDot { get; set; }
        public required TinhTrangDotLayMau TinhTrang { get; set; }
        public required LoaiMauDotLayMau LoaiMau { get; set; }
        public required string Ten { get; set; }
        public required int SoLuongDuKien { get; set; }
        public required DateTime NgayGio { get; set; }
        public required byte NguonLayMau { get; set; }
        public required string DonViPhoiHopId { get; set; }
        public string? TenDonViPhoiHop { get; set; }
        public string? DiaDiemToChuc { get; set; }
        public string? NguoiLienHe { get; set; }
        public string? MaXa { get; set; }
        public string? TenXa { get; set; }
        public string? MaHuyen { get; set; }
        public string? TenHuyen { get; set; }
        public string? MaTinh { get; set; }
        public string? TenTinh { get; set; }
        public string? GoogleMapLink { get; set; }
        public string? GhiChu { get; set; }

        public required bool IsDuocDangKy { get; set; }

        public List<LichSuHienMau>? LichSuHienMaus { get; set; }
    }
}
