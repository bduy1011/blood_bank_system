using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class DMHuyenAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<DMHuyen, DMHuyenView>
                .NewConfig()
                .Map(dest => dest.TenHuyen, src => src.Ten);
        }
    }
}
