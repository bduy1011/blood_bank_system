using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class LichSuHienMauAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<LichSuHienMau, LichSuHienMauView>
                .NewConfig()
                //.Ignore(dest => dest.KetQuaKTBTDescription)
                //.Ignore(dest => dest.KetQuaHbsAgDescription)
                //.Ignore(dest => dest.KetQuaGiangMaiDescription)
                //.Ignore(dest => dest.KetQuaHCVDescription)
                //.Ignore(dest => dest.KetQuaHIVDescription)
                //.Ignore(dest => dest.KetQuaSotRetDescription)
                .Map(d => d.Description, s => GetDescriptionKetQuaXetNghiem(s).Item1)
                .Map(d => d.IsHyperlink, s => GetHyperlink(s))
                .Map(d => d.GhiChuTuiMau, s => GetDescriptionKetQuaXetNghiem(s).Item2);

            TypeAdapterConfig<LichSuHienMau, LichSuHienMau>
                .NewConfig()
                .Ignore(d => d.Id);
        }

        internal static (string, string) GetDescriptionKetQuaXetNghiem(LichSuHienMau model)
        {
            if (model is null) return (string.Empty, string.Empty);

            var result = "Kết quả xét nghiệm:";

            if ((model.KetQuaKTBT is null || model.KetQuaKTBT == KetQuaXetNghiem.AM_TINH)
                && model.KetQuaGiangMai == KetQuaXetNghiem.AM_TINH
                && model.KetQuaHbsAg == KetQuaXetNghiem.AM_TINH
                && model.KetQuaHCV == KetQuaXetNghiem.AM_TINH
                && model.KetQuaHIV == KetQuaXetNghiem.AM_TINH
                && model.KetQuaSotRet == KetQuaXetNghiem.AM_TINH)
            {
                return ($"Kết quả xét nghiệm: {KetQuaXetNghiem.AM_TINH.GetDescription()}", "Túi máu đã được sử dụng !");
            }

            if ((model.KetQuaKTBT is not null && (model.KetQuaKTBT == KetQuaXetNghiem.CHUA_XET_NGHIEM || model.KetQuaKTBT == KetQuaXetNghiem.CHO_KET_QUA))
                || model.KetQuaGiangMai == KetQuaXetNghiem.CHUA_XET_NGHIEM || model.KetQuaGiangMai == KetQuaXetNghiem.CHO_KET_QUA
                || model.KetQuaHbsAg == KetQuaXetNghiem.CHUA_XET_NGHIEM || model.KetQuaHbsAg == KetQuaXetNghiem.CHO_KET_QUA
                || model.KetQuaHCV == KetQuaXetNghiem.CHUA_XET_NGHIEM || model.KetQuaHCV == KetQuaXetNghiem.CHO_KET_QUA
                || model.KetQuaHIV == KetQuaXetNghiem.CHUA_XET_NGHIEM || model.KetQuaHIV == KetQuaXetNghiem.CHO_KET_QUA
                || model.KetQuaSotRet == KetQuaXetNghiem.CHUA_XET_NGHIEM || model.KetQuaSotRet == KetQuaXetNghiem.CHO_KET_QUA)
            {
                return ($"{result} {KetQuaXetNghiem.CHO_KET_QUA.GetDescription()}", "");
            }

            return ($">> Thư mời tư vấn (nhấn để xem)", "");
        }

        internal static int? GetHyperlink(LichSuHienMau model)
        {
            if (model is null) return null;

            if (model.KetQuaGiangMai == KetQuaXetNghiem.DUONG_TINH
                || model.KetQuaHbsAg == KetQuaXetNghiem.DUONG_TINH
                || model.KetQuaHCV == KetQuaXetNghiem.DUONG_TINH
                || model.KetQuaHIV == KetQuaXetNghiem.DUONG_TINH
                || model.KetQuaKTBT == KetQuaXetNghiem.DUONG_TINH
                || model.KetQuaSotRet == KetQuaXetNghiem.DUONG_TINH)
            {
                if (model.KetQuaNATDuongTinh == true)
                    return 2; // hẹn đến sau 30 ngày như "không xác định"
                return 1;
            }

            if (model.KetQuaGiangMai == KetQuaXetNghiem.CHUA_XAC_DINH
                || model.KetQuaHbsAg == KetQuaXetNghiem.CHUA_XAC_DINH
                || model.KetQuaHCV == KetQuaXetNghiem.CHUA_XAC_DINH
                || model.KetQuaHIV == KetQuaXetNghiem.CHUA_XAC_DINH
                || model.KetQuaKTBT == KetQuaXetNghiem.CHUA_XAC_DINH
                || model.KetQuaSotRet == KetQuaXetNghiem.CHUA_XAC_DINH) return 2;

            return null;
        }
    }
}
