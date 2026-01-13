using BB.CR.Models;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class GiaoDichConAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<GiaoDichCon, GiaoDichConView>
                .NewConfig()
                .Map(d => d.MaNhomMauDescription, s => s.MaNhomMau.GetValueOrDefault(Providers.Bases.NhomMau.KXD).GetDescription());

            TypeAdapterConfig<GiaoDichConView, GiaoDichCon>
                .NewConfig();
        }
    }
}
