using BB.CR.Models;
using BB.CR.Views.Otp;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class DMNguoiHienMauAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<PersonalInfo, DMNguoiHienMau>
                .NewConfig()
                .Map(d => d.HoVaTen, s => s.FullName)
                .Map(d => d.NgaySinh, s => s.Dob)
                .Map(d => d.GioiTinh, s => s.Gender)
                .Map(d => d.CMND, s => s.IdCard)
                .Map(d => d.SoDT, s => s.PhoneNr);
        }
    }
}
