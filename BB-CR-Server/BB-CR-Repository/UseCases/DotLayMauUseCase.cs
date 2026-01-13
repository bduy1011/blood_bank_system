using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    public class DotLayMauUseCase
    {
        public static async Task<ReturnResponse<List<DotLayMauView>>> LoadByCriteria(DotLayMauCriteria criteria
            , BloodBankContext context)
        {
            var response = new ReturnResponse<List<DotLayMauView>>();
            var query = context.DotLayMau
                .AsNoTracking()
                .Where(i => i.IsDuocDangKy); // chỉ hiển thị những đợt đăng ký hiến máu true

            if (criteria.DotLayMauIds?.Count > 0)
                query = query
                    .Where(i => criteria.DotLayMauIds.Contains(i.DotLayMauId));

            if (criteria.TinhTrangs?.Count > 0)
                query = query
                    .Where(i => criteria.TinhTrangs.Contains(i.TinhTrang));

            if (criteria.TuNgay is not null && criteria.DenNgay is not null)
                query = query
                    .Where(i => criteria.TuNgay.Value.Date <= i.NgayGio.Date && criteria.DenNgay.Value.Date >= i.NgayGio.Date);
            else if (criteria.TuNgay is null && criteria.DenNgay is null)
                query = query
                    .Where(i => i.NgayGio >= DateTime.Now.AddDays(1));

            if (!string.IsNullOrEmpty(criteria.Xa))
                query = query
                    .Where(i => i.MaXa == criteria.Xa);

            if (!string.IsNullOrEmpty(criteria.Huyen))
                query = query
                    .Where(i => i.MaHuyen == criteria.Huyen);

            if (!string.IsNullOrEmpty(criteria.Tinh))
                query = query
                    .Where(i => i.MaTinh == criteria.Tinh);

            var daDangKy = context.DangKyHienMau
                .AsNoTracking()
                .Where(i => i.TinhTrang == TinhTrangDangKyHienMau.DaDangKy || i.TinhTrang == TinhTrangDangKyHienMau.DaTiepNhan)
                .GroupBy(i => i.DotLayMauId)
                .Select(i => new { DotLayMauId = i.Key, SoLuongDangKy = i.Count(o => o.Id > 0) });

            var result = from dotlaymau in query
                         join slDaDangKy in daDangKy on dotlaymau.DotLayMauId equals slDaDangKy.DotLayMauId into _slDaDangKy
                         from dadangKy in _slDaDangKy.DefaultIfEmpty()
                         select new DotLayMauView
                         {
                             DotLayMauId = dotlaymau.DotLayMauId,
                             LoaiDot = dotlaymau.LoaiDot,
                             TinhTrang = dotlaymau.TinhTrang,
                             LoaiMau = dotlaymau.LoaiMau,
                             Ten = dotlaymau.Ten,
                             SoLuongDuKien = dotlaymau.SoLuongDuKien,
                             NgayGio = dotlaymau.NgayGio,
                             NguonLayMau = dotlaymau.NguonLayMau,
                             DonViPhoiHopId = dotlaymau.DonViPhoiHopId,
                             IsDuocDangKy = dotlaymau.IsDuocDangKy,
                             TenDonViPhoiHop = dotlaymau.TenDonViPhoiHop,
                             DiaDiemToChuc = dotlaymau.DiaDiemToChuc,
                             NguoiLienHe = dotlaymau.NguoiLienHe,
                             MaXa = dotlaymau.MaXa,
                             TenXa = dotlaymau.TenXa,
                             MaHuyen = dotlaymau.MaHuyen,
                             TenHuyen = dotlaymau.TenHuyen,
                             MaTinh = dotlaymau.MaTinh,
                             TenTinh = dotlaymau.TenTinh,
                             GoogleMapLink = dotlaymau.GoogleMapLink,
                             GhiChu = dotlaymau.GhiChu,
                             SoLuongDangKyHienMau = dadangKy.SoLuongDangKy
                         };

            result = result
                .OrderBy(i => i.NgayGio)
                .ApplyPagination(criteria.PageIndex, criteria.PageSize);

            var model = await result
                .ToListAsync()
                .ConfigureAwait(false);

            response.Success(model.Adapt<List<DotLayMauView>>(), CommonResources.Ok);
            return response;
        }

        public static async Task<ReturnResponse<List<DotLayMau>>> UpdateLocalAsync(List<DotLayMau> model
            , BloodBankContext context)
        {
            var response = new ReturnResponse<List<DotLayMau>>();
            var data = await context.DotLayMau
                .AsNoTracking().Where(i => model.Select(o => o.DotLayMauId).Contains(i.DotLayMauId)).ToListAsync().ConfigureAwait(false);
            if (data.Count == 0)
                await context.DotLayMau.AddRangeAsync(model).ConfigureAwait(false);
            else
            {
                data = model.Where(i => data.Select(o => o.DotLayMauId).Contains(i.DotLayMauId)).ToList();
                context.DotLayMau.UpdateRange(data);

                var undata = model.Where(i => !data.Select(o => o.DotLayMauId).Contains(i.DotLayMauId)).ToList();
                if (undata.Count > 0)
                    await context.DotLayMau.AddRangeAsync(undata).ConfigureAwait(false);
            }

            var count = await context.SaveChangesAsync().ConfigureAwait(false);
            if (count == 0)
                response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
            else
                response.Success(data, CommonResources.Ok);

            return response;
        }
    }
}
