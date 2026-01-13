using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class TraLoiCauHoiAdapt
    {
        public static void ConfigureMapping()
        {
#pragma warning disable CS8603 // Possible null reference return.
            TypeAdapterConfig<TraLoiCauHoi, TraLoiCauHoiView>
                .NewConfig()
                .PreserveReference(true)
                .MaxDepth(2)
                .Map(dest => dest.TraLoiCauHoiChiTiets, src => src.TraLoiCauHoiChiTiets)
                .Ignore(dest => dest.DangKyHienMau);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
