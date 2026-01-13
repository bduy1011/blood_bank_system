using BB.CR.Models;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class GiaoDichAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<GiaoDich, GiaoDichView>
                .NewConfig()
                .Map(d => d.LoaiPhieuDescription, s => s.LoaiPhieu.GetDescription())
                .Map(d => d.TinhTrangDescription, s => s.TinhTrang.GetDescription())
                .Map(d => d.GiaoDichChiTietViews, s => s.GiaoDichChiTiets.Adapt<List<GiaoDichChiTietView>>());

            TypeAdapterConfig<GiaoDich, GiaoDich>
                .NewConfig()
                //.Ignore(d => d.GiaoDichId)
                .Ignore(d => d.GiaoDichChiTiets)
                .Ignore(d => d.DonViCapMau);

            TypeAdapterConfig<GiaoDichChiTiet, GiaoDichChiTiet>
                .NewConfig()
                //.Ignore(d => d.GiaoDichChiTietId)
                .Ignore(d => d.GiaoDichCons);

            TypeAdapterConfig<GiaoDichCon, GiaoDichCon>
                .NewConfig()
                /*.Ignore(d => d.Id)*/;
        }
    }
}
