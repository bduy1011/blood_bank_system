using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class TinTuc
    {
        public int TinTucId { get; set; }
        public required string TieuDe { get; set; }
        public string? NoiDung1 { get; set; }
        public string? NoiDung2 { get; set; }
        public LoaiTinTuc? Loai { get; set; }
        public ModuleTinTuc? Module { get; set; }
        public string? ThumbnailLink { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public required bool Active { get; set; }
    }
}
