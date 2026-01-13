using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Repositories.Filters;
using BB.CR.Repositories.Mappers;
using BB.CR.Repositories.Resources;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    public class GiaoDichUseCase
    {
        public static async Task<ReturnResponse<GiaoDichView>> GetAsync(long id, IMapper mapper, BloodBankContext context)
        {
            var response = new ReturnResponse<GiaoDichView>();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var model = await context.GiaoDich.AsNoTracking().Include(i => i.GiaoDichChiTiets).ThenInclude(i => i.GiaoDichCons).FirstOrDefaultAsync(i => i.GiaoDichId == id).ConfigureAwait(false);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            if (model is null)
                response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                var view = GiaoDichMapper.ExecuteMapper(mapper, model);
                response.Success(view, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<GiaoDichView>> CreateAsync(GiaoDich model, IMapper mapper, BloodBankContext context, string idCardNo)
        {
            var response = new ReturnResponse<GiaoDichView>();

            var _ = mapper.Map<GiaoDich>(model);
            _.CreatedBy = idCardNo;
            _.CreatedDate = DateTime.Now;

            model.GiaoDichChiTiets?.ForEach(i =>
            {
                _.GiaoDichChiTiets ??= [];

                var _d = mapper.Map<GiaoDichChiTiet>(i);
                _.GiaoDichChiTiets.Add(_d);

                i.GiaoDichCons?.ForEach(si =>
                {
                    _d.GiaoDichCons ??= [];

                    var _sd = mapper.Map<GiaoDichCon>(si);
                    _d.GiaoDichCons.Add(_sd);
                });
            });

            await context.GiaoDich.AddAsync(_).ConfigureAwait(false);

            var count = await context.SaveChangesAsync().ConfigureAwait(false);
            if (count == 0)
                response.Error(System.Net.HttpStatusCode.NoContent, CommonResources.NoContent);
            else
            {
                var view = GiaoDichMapper.ExecuteMapper(mapper, _);
                response.Success(view, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<List<GiaoDichView>>> LoadAsync(GiaoDichCriteria criteria, IMapper mapper, BloodBankContext context, string idCardNo)
        {
            var response = new ReturnResponse<List<GiaoDichView>>();

            var users = context.SystemUser
                .AsNoTracking();

            var donViCungCap = context.DMDonViCapMau
                .AsNoTracking();

            var user = users.FirstOrDefault(i => i.IdCardNr == idCardNo);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var query = context.GiaoDich
                .AsNoTrackingWithIdentityResolution()
                .Include(i => i.GiaoDichChiTiets)
                .ThenInclude(i => i.GiaoDichCons)
                .Where(i => ((user != null && user.AppRole != AppRole.Admin && user.AppRole != AppRole.Manager) ? i.CreatedBy : idCardNo) == idCardNo);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            var _query = GiaoDichFilter.ExecuteFiltering(criteria, query, users, donViCungCap);

            _query = _query.OrderByDescending(i => i.Ngay).ApplyPagination(criteria.PageIndex, criteria.PageSize);
            var model = await _query.ToListAsync().ConfigureAwait(false);
            if (model?.Count > 0)
            {
                //var view = mapper.Map<List<GiaoDichView>>(model);
                response.Success(model, CommonResources.Ok);
            }
            else
                response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);

            return response;
        }

        public static async Task<ReturnResponse<GiaoDichView>> UpdateAsync(long id, GiaoDich model, IMapper mapper, BloodBankContext context, string idCardNo)
        {
            var response = new ReturnResponse<GiaoDichView>();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var data = await context.GiaoDich.AsNoTracking()
                .Include(i => i.GiaoDichChiTiets)
                .ThenInclude(i => i.GiaoDichCons)
                .FirstOrDefaultAsync(i => i.GiaoDichId == id).ConfigureAwait(false);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                if (data.TinhTrang != TinhTrangGiaoDich.ChoXacNhan)
                {
                    response.Error(HttpStatusCode.Conflict, GiaoDichResources.TransactionChangedStatus);
                }
                else
                {
                    data = mapper.Map<GiaoDich>(model);
                    if (model.GiaoDichChiTiets?.Count > 0)
                    {
                        data.GiaoDichChiTiets = [];
                        data.GiaoDichChiTiets.AddRange(model.GiaoDichChiTiets);
                    }

                    data.UpdatedBy = idCardNo;
                    data.UpdatedDate = DateTime.Now;
                    context.GiaoDich.Update(data);

                    var count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                        response.Error(HttpStatusCode.Conflict, CommonResources.NoContent);
                    else
                    {
                        var view = GiaoDichMapper.ExecuteMapper(mapper, data);
                        response.Success(view, CommonResources.Ok);
                    }
                }
            }

            return response;
        }

        public static async Task<ReturnResponse<bool>> DeleteAsync(long id, BloodBankContext context, string idCardNo)
        {
            var response = new ReturnResponse<bool>();

            var model = await context.GiaoDich.AsNoTracking().FirstOrDefaultAsync(i => i.GiaoDichId == id).ConfigureAwait(false);
            if (model is null)
            {
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            }
            else
            {
                model.TinhTrang = TinhTrangGiaoDich.Huy;
                model.UpdatedBy = idCardNo;
                model.UpdatedDate = DateTime.Now;
                context.GiaoDich.Update(model);

                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                {
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                }
                else
                {
                    response.Success(true, CommonResources.Ok);
                }
            }

            return response;
        }

        public static async Task<ReturnResponse<GiaoDichView>> ApproveAsync(long id, TinhTrangGiaoDich status, BloodBankContext context, string idCardNo, IMapper mapper)
        {
            var response = new ReturnResponse<GiaoDichView>();

            var model = await context.GiaoDich.AsNoTracking()
                .Include(i => i.GiaoDichChiTiets!)
                .ThenInclude(i => i.GiaoDichCons)
                .FirstOrDefaultAsync(i => i.GiaoDichId == id).ConfigureAwait(false);

            if (model is null)
            {
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            }
            else
            {
                status = TinhTrangGiaoDich.ChoXacNhan;
                model.GiaoDichChiTiets?.ForEach(i =>
                {
                    bool? isValid = null;

                    if (i.GiaoDichCons?.Count > 0)
                    {
                        isValid = i.GiaoDichCons.All(o => o.SoLuong == o.SoLuongDuyet);
                        if (isValid is not null)
                        {
                            status = isValid == true ? TinhTrangGiaoDich.DaDuyet : TinhTrangGiaoDich.DuyetMotPhan;
                        }
                    }
                });

                model.TinhTrang = status;
                model.UpdatedBy = idCardNo;
                model.UpdatedDate = DateTime.Now;
                context.GiaoDich.Update(model);

                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                {
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                }
                else
                {
                    var view = mapper.Map<GiaoDichView>(model);
                    response.Success(view, CommonResources.Ok);
                }
            }

            return response;
        }
    }
}
