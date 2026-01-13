using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    public class GopYUseCase
    {
        public static async Task<ReturnResponse<GopYView>> CreateAsync(GopY model, BloodBankContext context, IMapper mapper, string idCardNo)
        {
            var response = new ReturnResponse<GopYView>();
            model.DMNguoiHienMau = null; // Can not add reference DMNguoiHienMau

            model.CreatedBy = idCardNo;
            model.CreatedDate = DateTime.Now;

            await context.GopY.AddAsync(model).ConfigureAwait(false);
            var count = await context.SaveChangesAsync().ConfigureAwait(false);
            if (count == 0)
                response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
            else
            {
                if (model.NguoiHienMauId is not null)
                    model.DMNguoiHienMau = await context.DMNguoiHienMau.AsNoTracking()
                        .FirstOrDefaultAsync(i => i.NguoiHienMauId == model.NguoiHienMauId)
                        .ConfigureAwait(false);

                var view = mapper.Map<GopYView>(model);
                response.Success(view, CommonResources.Ok);
            }
            return response;
        }

        public static async Task<ReturnResponse<bool>> DeleteAsync(long id, BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();
            var data = await context.GopY.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                context.GopY.Remove(data);
                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(true, CommonResources.Ok);
            }
            return response;
        }

        public static async Task<ReturnResponse<GopYView>> GetAsync(long id, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<GopYView>();
            var data = await context.GopY.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                var view = mapper.Map<GopYView>(data);
                response.Success(view, CommonResources.Ok);
            }
            return response;
        }

        public static async Task<ReturnResponse<List<GopYView>>> LoadAsync(GopYCriteria criteria, BloodBankContext context, IMapper mapper, string idCardNo)
        {
            var response = new ReturnResponse<List<GopYView>>();
            var query = context.GopY.AsNoTracking();

            var user = await context.SystemUser.AsNoTracking().FirstOrDefaultAsync(i => i.IdCardNr == idCardNo).ConfigureAwait(false);

            if (criteria.Ids is not null && criteria.Ids.Count > 0) query = query.Where(i => criteria.Ids.Contains(i.Id));
            if (criteria.NgayTu is not null) query = query.Where(i => i.Ngay.Date >= criteria.NgayTu.Value.Date);
            if (criteria.NgayDen is not null) query = query.Where(i => i.Ngay.Date <= criteria.NgayDen.Value.Date);
            if (!string.IsNullOrWhiteSpace(criteria.LoaiGopY)) query = query.Where(i => criteria.LoaiGopY.Contains(i.LoaiGopY));
            if (!string.IsNullOrWhiteSpace(criteria.Email)) query = query.Where(i => i.Email != null && EF.Functions.Like(i.Email, $"%{criteria.Email}%"));
            if (!string.IsNullOrWhiteSpace(criteria.SoDT)) query = query.Where(i => i.SoDT != null && EF.Functions.Like(i.SoDT, $"%{criteria.SoDT}%"));
            if (criteria.DaXem is not null) query = query.Where(i => i.DaXem == criteria.DaXem);

            if (user is not null
                && user.AppRole != AppRole.Admin
                && user.AppRole != AppRole.Manager)
                query = query.Where(i => i.CreatedBy == idCardNo);

            query = query.OrderByDescending(i => i.Ngay).ApplyPagination(criteria.PageIndex, criteria.PageSize);
            var data = await query.ToListAsync().ConfigureAwait(false);
            var view = mapper.Map<List<GopYView>>(data);
            if (view is null)
                response.Success([], CommonResources.NotFound);
            else
                response.Success(view, CommonResources.Ok);
            return response;
        }

        public static async Task<ReturnResponse<GopYView>> UpdateAsync(long id, GopY model, BloodBankContext context, IMapper mapper, string idCardNo)
        {
            var response = new ReturnResponse<GopYView>();
            var data = await context.GopY.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                data = mapper.Map<GopY>(model);

                data.UpdatedBy = idCardNo;
                data.UpdatedDate = DateTime.Now;

                context.GopY.Update(data);
                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                {
                    var view = mapper.Map<GopYView>(data);
                    response.Success(view, CommonResources.Ok);
                }
            }
            return response;
        }
    }
}
