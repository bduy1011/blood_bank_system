using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public static class DMTinhAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<DMTinh, DMTinhView>
                .NewConfig()
                .Map(dest => dest.TenTinh, src => src.Ten);
        }
    }
}
