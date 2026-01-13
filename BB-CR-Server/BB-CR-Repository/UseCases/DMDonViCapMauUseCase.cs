using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    public class DMDonViCapMauUseCase
    {
        public static async Task<ReturnResponse<DMDonViCapMauView>> GetAsync(string ma, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<DMDonViCapMauView>();

            var data = await context.DMDonViCapMau.AsNoTracking().FirstOrDefaultAsync(i => ma == i.MaDonVi && i.Active).ConfigureAwait(false);

            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
                response.Success(mapper.Map<DMDonViCapMauView>(data), CommonResources.Ok);

            return response;
        }

        public static async Task<ReturnResponse<DMDonViCapMauView>> UpdateAsync(string ma, DMDonViCapMau model, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<DMDonViCapMauView>();

            var data = await context.DMDonViCapMau.AsNoTracking().FirstOrDefaultAsync(i => ma == i.MaDonVi && i.Active).ConfigureAwait(false);

            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                data = mapper.Map<DMDonViCapMau>(model);
                context.DMDonViCapMau.Update(data);
                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(mapper.Map<DMDonViCapMauView>(data), CommonResources.Ok);
            }

            return response;
        }

        /// <summary>
        /// Add or update data from local
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResponse<bool>> UpdateLocalAsync(List<DMDonViCapMau> model, BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();
            var data = await context.DMDonViCapMau.AsNoTracking().Where(i => model.Select(o => o.MaDonVi).Contains(i.MaDonVi)).ToListAsync().ConfigureAwait(false);
            if (data.Count == 0)
            {
                await context.DMDonViCapMau.AddRangeAsync(model).ConfigureAwait(false);
            }
            else
            {
                data = model.Where(i => data.Select(o => o.MaDonVi).Contains(i.MaDonVi)).ToList();
                context.DMDonViCapMau.UpdateRange(data);

                var undata = model.Where(i => !data.Select(o => o.MaDonVi).Contains(i.MaDonVi)).ToList();
                if (undata.Count > 0)
                    await context.DMDonViCapMau.AddRangeAsync(undata).ConfigureAwait(false);
            }

            var count = await context.SaveChangesAsync().ConfigureAwait(false);
            if (count == 0)
                response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
            else
                response.Success(true, CommonResources.Ok);

            return response;
        }

        public static async Task<ReturnResponse<List<DMDonViCapMau>>> LoadAsync(BloodBankContext context)
        {
            var response = new ReturnResponse<List<DMDonViCapMau>>();

            var data = await context.DMDonViCapMau.AsNoTracking().Where(i => i.Active == true && i.IsDonViCapMau == true /*&& i.IsHienThiApp == true*/).ToListAsync().ConfigureAwait(false);
            response.Success(data, CommonResources.Ok);

            return response;
        }
    }
}
