using BB.CR.Providers.Bases;
using System.Text.Json.Serialization;

namespace BB.CR.Models
{
    public class CauHinhTonKho
    {
        public long Id { get; set; }
        public required DateTime Ngay { get; set; }

        public List<CauHinhTonKhoChiTiet>? CauHinhTonKhoChiTiets { get; set; }
    }

    public class CauHinhTonKhoChiTiet 
    {
        public long Id { get; set; }
        public long? CauHinhTonKhoId { get; set; }
        [JsonIgnore]
        public CauHinhTonKho? CauHinhTonKho { get; set; }

        public required LoaiSanPham LoaiSanPham { get; set; }

        public List<CauHinhTonKhoChiTietCon>? CauHinhTonKhoChiTietCons { get; set; }
    }

    public class CauHinhTonKhoChiTietCon
    {
        public long Id { get; set; }
        public long? CauHinhTonKhoChiTietId { get; set; }
        [JsonIgnore]
        public CauHinhTonKhoChiTiet? CauHinhTonKhoChiTiet { get; set; }
        public required int SoLuong { get; set; }
        public required int SoLuongDuocBooking { get; set; }

        public required NhomMau MaNhomMau { get; set; }
    }
}
