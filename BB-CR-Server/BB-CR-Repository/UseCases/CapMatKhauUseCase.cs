using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Repositories.Filters;
using BB.CR.Views.Criterias;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    internal class CapMatKhauUseCase
    {
        internal static async Task<ReturnResponse<bool>> CreateAsync(CapMatKhau model, BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var _models = await context.CapMatKhaus
                .AsNoTracking()
                .Where(i => i.CMND == model.CMND && i.ResetPasswordOn == null)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync()
                .ConfigureAwait(false);
            if (_models?.Count > 0)
            {
                var item = _models.FirstOrDefault();
                response.Error(HttpStatusCode.Conflict, $"{Environment.NewLine}{string.Format(BzLogicResource.CapMatKhauTonTai, item?.CMND, item?.CreatedDate.ToString("dd/MM/yyyy HH:mm"))}");
            }
            else
            {
                await context.CapMatKhaus.AddAsync(model).ConfigureAwait(false);

                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.Conflict, CommonResources.NoContent);
                else
                    response.Success(true, CommonResources.Ok);
            }

            return response;
        }

        internal static async Task<ReturnResponse<List<CapMatKhau>>> LoadAsync(CapMatKhauCriteria criteria, BloodBankContext context)
        {
            var response = new ReturnResponse<List<CapMatKhau>>();

            var model = context.CapMatKhaus.AsNoTracking();
            model = CapMatKhauFilter.Seek(criteria, model);
            model.OrderByDescending(i => i.CreatedDate).ApplyPagination(criteria.PageIndex, criteria.PageSize);

            var _ = await model.ToListAsync().ConfigureAwait(false);
            if (_?.Count > 0)
                response.Success(_, CommonResources.Ok);
            else
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);

            return response;
        }
    }
}
