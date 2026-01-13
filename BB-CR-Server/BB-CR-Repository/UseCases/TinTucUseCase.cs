using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    public class TinTucUseCase
    {
        public static async Task<ReturnResponse<List<TinTuc>>> LoadAsync(BloodBankContext context)
        {
            var response = new ReturnResponse<List<TinTuc>>();

            // Update get data 2 year (current year and current year - 1)
            var model = await context.TinTuc.AsNoTracking().Where(i => i.CreatedOn != null && (i.CreatedOn.Value.Year == DateTime.Now.Year || i.CreatedOn.Value.Year == DateTime.Now.AddYears(-1).Year)).OrderByDescending(i => i.CreatedOn).Take(10).ToListAsync().ConfigureAwait(false);
            response.Success(model, CommonResources.Ok);

            return response;
        }

        public static async Task<ReturnResponse<TinTuc>> GetAsync(BloodBankContext context, int id)
        {
            var response = new ReturnResponse<TinTuc>();

            var model = await context.TinTuc.AsNoTracking().FirstOrDefaultAsync(i => i.TinTucId == id).ConfigureAwait(false);
            response.Success(model, CommonResources.Ok);

            return response;
        }
    }
}
