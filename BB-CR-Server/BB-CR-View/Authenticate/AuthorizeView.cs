using BB.CR.Providers.Bases;

namespace BB.CR.Views.Authenticate
{
    public class AuthorizeView
    {
        public string? UserCode { get; set; }
        public string? Name { get; set; }
        public string? AccessToken { get; set; }
        public AppRole AppRole { get; set; }
        public bool Active { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IdCardNr { get; set; }

        public DMNguoiHienMauView? DMNguoiHienMau { get; set; }

        public int? SoLanHienMau { get; set; }
        public DateTime? NgayHienMauGanNhat { get; set; }

        public bool DuongTinhGanNhat { get; set; }

        public bool? IsDataQLMau { get; set; }
    }

    public class ResultView
    {
        public DMNguoiHienMauView? NguoiHienMau { get; set; }
        public int? SoLanHienMau { get; set; }
        public DateTime? NgayHienMauGanNhat { get; set; }
        public bool DuongTinhGanNhat { get; set; }
    }
}
