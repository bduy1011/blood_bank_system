using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class DMDonViCapMauAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<DMDonViCapMau, DMDonViCapMauView>
                .NewConfig();
        }
    }
}
