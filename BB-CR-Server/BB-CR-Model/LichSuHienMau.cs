using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class LichSuHienMau
    {
        public long Id { get; set; }
        public required int NguoiHienMauId { get; set; }
        public required string MaVachId { get; set; }
        public required DateTime NgayThu { get; set; }
        public required string MaSanPham { get; set; }
        public required string TenSanPham { get; set; }
        public required int? TheTich { get; set; }
        public DateTime? NgaySanXuat { get; set; }
        public DateTime? NgayCapPhat { get; set; }
        public string? CapPhatChoDonVi { get; set; }
        /// <summary>
        /// Nếu null: hiển thị "đã xuất cho bệnh nhân"
        /// </summary>
        public string? GhiChuTuiMau { get; set; }
        //public string? KetQuaNhomMau { get; set; }
        public KetQuaXetNghiem? KetQuaKTBT { get; set; }
        public KetQuaXetNghiem? KetQuaHbsAg { get; set; }
        public KetQuaXetNghiem? KetQuaHIV { get; set; }
        public KetQuaXetNghiem? KetQuaHCV { get; set; }
        public KetQuaXetNghiem? KetQuaGiangMai { get; set; }
        public KetQuaXetNghiem? KetQuaSotRet { get; set; }
        public bool? KetQuaNATDuongTinh { get; set; }

        public required long DotLayMauId { get; set; } // Bổ sung ngày 20240922

        public DMNguoiHienMau? DMNguoiHienMau { get; set; }
        public DotLayMau? DotLayMau { get; set; }
    }
}
