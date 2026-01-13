using BB.CR.Models;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class GiaoDichChiTietAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<GiaoDichChiTiet, GiaoDichChiTietView>
                .NewConfig()
                .Map(d => d.LoaiSanPhamDescription, s => s.LoaiSanPham.GetDescription())
                .Map(d => d.GiaoDichConViews, s => s.GiaoDichCons.Adapt<List<GiaoDichConView>>());
        }
    }
}
