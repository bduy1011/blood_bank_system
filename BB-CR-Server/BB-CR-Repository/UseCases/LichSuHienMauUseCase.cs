using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class LichSuHienMauUseCase
    {
        public static async Task<ReturnResponse<List<LichSuHienMauView>>> LoadAsync(LichSuHienMauCriteria criteria, BloodBankContext context
            , IMapper mapper)
        {
            ReturnResponse<List<LichSuHienMauView>> response = new();

            var data = context.LichSuHienMau.AsNoTracking();

            if (criteria.NguoiHienMauIds?.Count > 0) data = data.Where(i => criteria.NguoiHienMauIds.Contains(i.NguoiHienMauId));
            if (criteria.DotHienMauIds?.Count > 0) data = data.Where(i => criteria.DotHienMauIds.Contains(i.DotLayMauId));
            //if (criteria.LichLayMauIds?.Count > 0) data = data.Where(i => criteria.LichLayMauIds.Contains(i.LichLayMauId.GetValueOrDefault()));
            if (criteria.MaVachIds?.Count > 0) data = data.Where(i => criteria.MaVachIds.Contains(i.MaVachId));
            if (criteria.NgayThuTu is not null) data = data.Where(i => criteria.NgayThuTu.Value.Date <= i.NgayThu.Date);
            if (criteria.NgayThuDen is not null) data = data.Where(i => criteria.NgayThuDen.Value.Date >= i.NgayThu.Date);
            if (criteria.MaSanPhams?.Count > 0) data = data.Where(i => criteria.MaSanPhams.Contains(i.MaSanPham));
            //if (criteria.MaNhomMaus?.Count > 0) data = data.Where(i => criteria.MaNhomMaus.Contains(i.KetQuaNhomMau));
            if (criteria.IsSanLocAmTinh is not null)
                data = data.Where(
                    i => KetQuaXetNghiem.AM_TINH == i.KetQuaKTBT
                        || KetQuaXetNghiem.AM_TINH == i.KetQuaHbsAg
                        || KetQuaXetNghiem.AM_TINH == i.KetQuaHCV
                        || KetQuaXetNghiem.AM_TINH == i.KetQuaHIV
                        || KetQuaXetNghiem.AM_TINH == i.KetQuaGiangMai
                        || KetQuaXetNghiem.AM_TINH == i.KetQuaSotRet);
            if (criteria.IsSanLocDuongTinh is not null)
                data = data.Where(
                    i => KetQuaXetNghiem.DUONG_TINH == i.KetQuaKTBT
                        || KetQuaXetNghiem.DUONG_TINH == i.KetQuaHbsAg
                        || KetQuaXetNghiem.DUONG_TINH == i.KetQuaHCV
                        || KetQuaXetNghiem.DUONG_TINH == i.KetQuaHIV
                        || KetQuaXetNghiem.DUONG_TINH == i.KetQuaGiangMai
                        || KetQuaXetNghiem.DUONG_TINH == i.KetQuaSotRet);
            if (criteria.IsSanLocKhongXacDinh is not null)
                data = data.Where(
                    i => KetQuaXetNghiem.CHUA_XAC_DINH == i.KetQuaKTBT
                        || KetQuaXetNghiem.CHUA_XAC_DINH == i.KetQuaHbsAg
                        || KetQuaXetNghiem.CHUA_XAC_DINH == i.KetQuaHCV
                        || KetQuaXetNghiem.CHUA_XAC_DINH == i.KetQuaHIV
                        || KetQuaXetNghiem.CHUA_XAC_DINH == i.KetQuaGiangMai
                        || KetQuaXetNghiem.CHUA_XAC_DINH == i.KetQuaSotRet);

            data = data.OrderByDescending(i => i.DotLayMauId).ThenByDescending(i => i.NguoiHienMauId).ApplyPagination(criteria.PageIndex, criteria.PageSize);

            var model = await data.ToListAsync().ConfigureAwait(false);
            if (model is null || model.Count == 0)
                response.Success([], CommonResources.NotFound);
            else
            {
                var list = mapper.Map<List<LichSuHienMauView>>(model);
                response.Success(list, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<bool>> UpdateLocalAsync(List<LichSuHienMau> model, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<bool>();

            //var nguoiHienMauIds = model.Select(o => o.NguoiHienMauId);
            var maVachIds = model.Select(o => o.MaVachId);
            var data = await context.LichSuHienMau.AsNoTracking().Where(i =>
                //nguoiHienMauIds.Contains(i.NguoiHienMauId)
                maVachIds.Contains(i.MaVachId)).ToListAsync().ConfigureAwait(false);
            if (data?.Count > 0)
            {
                data = data.Select(i =>
                {
                    var _ = model.FirstOrDefault(o => o.MaVachId == i.MaVachId);
                    if (_ is not null)
                    {
                        var _id = i.Id;
                        i = mapper.Map<LichSuHienMau>(_);
                        i.Id = _id;
                    }

                    return i;
                }).ToList();
                context.LichSuHienMau.UpdateRange(data);

                var undata = model.Where(i =>
                    //!data.Select(o => o.NguoiHienMauId).Contains(i.NguoiHienMauId)
                    //&& 
                    !data.Select(o => o.MaVachId).Contains(i.MaVachId)).ToList();
                if (undata?.Count > 0)
                    await context.LichSuHienMau.AddRangeAsync(undata).ConfigureAwait(false);
            }
            else
                await context.LichSuHienMau.AddRangeAsync(model).ConfigureAwait(false);

            var count = await context.SaveChangesAsync().ConfigureAwait(false);
            response.Success(true, CommonResources.Ok);

            return response;
        }

        public static async Task<LetterView?> GetLetterAsync(long id, BloodBankContext context)
        {
            var model = from lichSuHienMau in context.LichSuHienMau.AsNoTracking().Where(i => i.Id == id)
                        join dmNguoiHienMau in context.DMNguoiHienMau.AsNoTracking() on lichSuHienMau.NguoiHienMauId equals dmNguoiHienMau.NguoiHienMauId
                        select new LetterView
                        {
                            HoTen = dmNguoiHienMau.HoVaTen,
                            NgaySinh = dmNguoiHienMau.NgaySinh,
                            DiaChi = !string.IsNullOrWhiteSpace(dmNguoiHienMau.DiaChiTamTru) ? dmNguoiHienMau.DiaChiTamTru : dmNguoiHienMau.DiaChiLienLac,
                            NgayHienMau = lichSuHienMau.NgayThu.ToString(DateFormatter.DDMMYYYY_VN),
                            MaTuiMau = lichSuHienMau.MaVachId,
                            ThoiGianGapMat = Helpers.GetThoiGianGapMat(lichSuHienMau.NgayThu),
                        };

            return await model.FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
