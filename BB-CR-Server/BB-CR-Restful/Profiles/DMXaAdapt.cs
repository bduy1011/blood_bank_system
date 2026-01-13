using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class DMXaAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<DMXa, DMXaView>
                .NewConfig()
                .Map(dest => dest.TenXa, src => src.Ten);
        }
    }
}
