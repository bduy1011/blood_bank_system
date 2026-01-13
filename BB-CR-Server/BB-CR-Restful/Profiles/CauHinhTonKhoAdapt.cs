using BB.CR.Models;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class CauHinhTonKhoAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<CauHinhTonKho, CauHinhTonKhoView>
                .NewConfig()
                .Map(d => d.CauHinhTonKhoChiTietViews, s => s.CauHinhTonKhoChiTiets.Adapt<List<CauHinhTonKhoChiTietView>>());

            TypeAdapterConfig<CauHinhTonKhoChiTiet, CauHinhTonKhoChiTietView>
                .NewConfig()
                .Map(d => d.LoaiSanPhamDescription, s => s.LoaiSanPham.GetDescription())
                .Map(d => d.CauHinhTonKhoChiTietConViews, s => s.CauHinhTonKhoChiTietCons.Adapt<List<CauHinhTonKhoChiTietConView>>());

            TypeAdapterConfig<CauHinhTonKhoChiTietCon, CauHinhTonKhoChiTietConView>
                .NewConfig()
                .Map(d => d.MaNhomMauDescription, s => s.MaNhomMau.GetDescription());
        }
    }
}
