using BB.CR.Models;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class DangKyHienMauAdapt
    {
        public static void ConfigureMapping()
        {
#pragma warning disable CS8603 // Possible null reference return.
            TypeAdapterConfig<DangKyHienMau, DangKyHienMauView>
                .NewConfig()
                .Map(dest => dest.TinhTrangDescription, src => src.TinhTrang.GetDescription())
                .Ignore(dest => dest.TraLoiCauHoi);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
