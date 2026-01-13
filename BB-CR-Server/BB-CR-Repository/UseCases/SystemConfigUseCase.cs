using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views.Criterias;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class SystemConfigUseCase
    {
        public static async Task<ReturnResponse<List<SystemConfig>>> LoadAsync(SystemConfigCriteria criteria, BloodBankContext context)
        {
            ReturnResponse<List<SystemConfig>> response = new();

            var data = context.SystemConfig.AsNoTracking();

            if (string.IsNullOrEmpty(criteria.Key) == false)
                data = data.Where(i => i.Key == criteria.Key);

            var model = await data.ToListAsync().ConfigureAwait(false);
            if (model is null || model.Count == 0)
                response.Success([], CommonResources.NotFound);
            else
                response.Success(model, CommonResources.Ok);

            return response;
        }


    }
}
