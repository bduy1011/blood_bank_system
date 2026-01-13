using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Repositories.Filters;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    internal class CauHinhTonKhoUseCase
    {
        internal static async Task<ReturnResponse<CauHinhTonKhoView>> CreateAsync(CauHinhTonKho model, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<CauHinhTonKhoView>();

            await context.CauHinhTonKho
                .AddAsync(model)
                .ConfigureAwait(false);

            int count = await context
                .SaveChangesAsync()
                .ConfigureAwait(false);
            if (count == 0)
                response
                    .Error(HttpStatusCode.NoContent, CommonResources.NoContent);
            else
            {
                var _ = mapper.Map<CauHinhTonKhoView>(model);
                response
                    .Success(_, CommonResources.Ok);
            }

            return response;
        }

        internal static async Task<ReturnResponse<bool>> DeleteAsync(long id, BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var model = await context.CauHinhTonKho
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(i => i.Id == id)
                .ConfigureAwait(false);

            if (model is null)
                response
                    .Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                context.CauHinhTonKho
                    .Remove(model);
                int count = await context
                    .SaveChangesAsync()
                    .ConfigureAwait(false);
                if (count == 0)
                    response
                        .Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response
                        .Success(true, CommonResources.Ok);
            }

            return response;
        }

        internal static async Task<ReturnResponse<CauHinhTonKhoView>> GetAsync(long id, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<CauHinhTonKhoView>();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var model = await context.CauHinhTonKho
                .AsNoTrackingWithIdentityResolution()
                .Include(i => i.CauHinhTonKhoChiTiets)
                .ThenInclude(i => i.CauHinhTonKhoChiTietCons)
                .FirstOrDefaultAsync(i => i.Id == id)
                .ConfigureAwait(false);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            if (model is null)
                response
                    .Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                var giaoDichs = await context.GiaoDich.AsNoTracking()
                    .Include(i => i.GiaoDichChiTiets!)
                    .ThenInclude(i => i.GiaoDichCons)
                    .Where(i => i.Ngay == model.Ngay && (i.TinhTrang != TinhTrangGiaoDich.Huy || i.TinhTrang != TinhTrangGiaoDich.TuChoi))
                    .ToListAsync().ConfigureAwait(false);
                if (giaoDichs?.Count > 0)
                {
                    model.CauHinhTonKhoChiTiets?.ForEach(i =>
                    {
                        var chiTiets = giaoDichs.SelectMany(o => o.GiaoDichChiTiets!.Where(i => i.LoaiSanPham == i.LoaiSanPham))?.ToList();
                        if (chiTiets?.Select(z => z.LoaiSanPham)?.Contains(i.LoaiSanPham) == true)
                        {
                            i.CauHinhTonKhoChiTietCons?.ForEach(o =>
                            {
                                o.SoLuongDuocBooking = chiTiets?.SelectMany(t => t.GiaoDichCons!.Where(j => j.MaNhomMau == o.MaNhomMau))?.Sum(j => j.SoLuongDuyet ?? j.SoLuong) ?? 0;
                            });
                        }
                    });

                    try
                    {
                        context.CauHinhTonKho.Update(model);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                    }
                    catch
                    {
                    }
                }

                var _ = mapper
                    .Map<CauHinhTonKhoView>(model);
                response
                    .Success(_, CommonResources.Ok);
            }

            return response;
        }

        internal static async Task<ReturnResponse<List<CauHinhTonKhoView>>> LoadAsync(CauHinhTonKhoCriteria criteria, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<List<CauHinhTonKhoView>>();

            var model = context.CauHinhTonKho
                .AsNoTrackingWithIdentityResolution();
            model = CauHinhTonKhoFilter
                .Seek(criteria, model);
            model = model
                .ApplyPagination(criteria.PageIndex, criteria.PageSize);

            var _ = mapper
                .Map<List<CauHinhTonKhoView>>(await model
                    .ToListAsync()
                    .ConfigureAwait(false));

            response
                .Success(_, CommonResources.Ok);
            return response;
        }

        internal static async Task<ReturnResponse<CauHinhTonKhoView>> UpdateAsync(long id, CauHinhTonKho model, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<CauHinhTonKhoView>();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var _model = await context.CauHinhTonKho
                .AsNoTrackingWithIdentityResolution()
                .Include(i => i.CauHinhTonKhoChiTiets)
                .ThenInclude(i => i.CauHinhTonKhoChiTietCons)
                .FirstOrDefaultAsync(i => i.Id == id)
                .ConfigureAwait(false);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            if (_model is null)
                response
                    .Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                context.CauHinhTonKho
                    .Update(model);
                int count = await context
                    .SaveChangesAsync()
                    .ConfigureAwait(false);
                if (count == 0)
                    response
                        .Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                {
                    var _ = mapper.Map<CauHinhTonKhoView>(model);
                    response
                        .Success(_, CommonResources.Ok);
                }
            }

            return response;
        }

        internal static async Task<ReturnResponse<CauHinhTonKhoView>> InitAsync(DateTime ngay, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<CauHinhTonKhoView>();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var model = await context.CauHinhTonKho
                .AsNoTrackingWithIdentityResolution()
                .Include(i => i.CauHinhTonKhoChiTiets)
                .ThenInclude(i => i.CauHinhTonKhoChiTietCons)
                .FirstOrDefaultAsync(i => i.Ngay.Date == ngay.Date)
                .ConfigureAwait(false);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            if (model is null)
            {
                model = new CauHinhTonKho
                {
                    Ngay = ngay,
                    CauHinhTonKhoChiTiets = []
                };

                var loaiSanPhams = EnumExtension.ToList<LoaiSanPham>();
                var maNhomMaus = EnumExtension.ToList<NhomMau>().Where(j => j.Key != NhomMau.KXD);
                Parallel.ForEach(loaiSanPhams, i =>
                {
                    var cauHinhTonKhoChiTiet = new CauHinhTonKhoChiTiet
                    {
                        LoaiSanPham = i.Key,
                        CauHinhTonKhoChiTietCons = []
                    };
                    Parallel.ForEach(maNhomMaus, o =>
                    {
                        var cauHinhChiTietCon = new CauHinhTonKhoChiTietCon
                        {
                            MaNhomMau = o.Key,
                            SoLuong = 0,
                            SoLuongDuocBooking = 0,
                        };

                        cauHinhTonKhoChiTiet.CauHinhTonKhoChiTietCons.Add(cauHinhChiTietCon);
                    });
                    model.CauHinhTonKhoChiTiets.Add(cauHinhTonKhoChiTiet);
                });

                var _ = mapper
                    .Map<CauHinhTonKhoView>(model);
                response
                    .Success(_, CommonResources.Ok);
            }
            else
            {
                var _ = mapper
                    .Map<CauHinhTonKhoView>(model);
                response
                    .Success(_, CommonResources.Ok);
            }

            return response;
        }
    }
}
