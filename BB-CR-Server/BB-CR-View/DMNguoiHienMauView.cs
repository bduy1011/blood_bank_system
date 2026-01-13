namespace BB.CR.Views
{
    public class DMNguoiHienMauView
    {
        public int? NguoiHienMauId { get; set; }
        public required string HoVaTen { get; set; }
        public string? HoVaTenNoDiacritics { get; set; }
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
        public string? DiaChiTamTru { get; set; }
        public string? DiaChiLienLac { get; set; }
        public string? DiaChiThuongTru { get; set; }
        public string? NgheNghiep { get; set; }
        public string? Email { get; set; }
        public string? SoDT { get; set; }
        public string? MaNhomMau { get; set; }
        public string? TenNhomMau { get; set; }
        public string? GhiChu { get; set; }
        public required int SoLanHienMau { get; set; }

        public string? MobileMacAddress { get; set; } // Bổ sung ngày 20240922
        /// <summary>
        /// Để update ngược về database máu local
        /// </summary>
        public DateTime? UpdatedTime { get; set; } // Bổ sung ngày 20240922

        //public List<LichSuHienMau>? LichSuHienMaus { get; set; }
        //public List<GopY>? GopYs { get; set; }
    }
}
