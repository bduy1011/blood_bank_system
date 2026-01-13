using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views.Criterias;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class SystemSlideUseCase
    {
        public static async Task<ReturnResponse<List<SystemSlide>>> LoadAsync(SystemSlideCriteria criteria, BloodBankContext context)
        {
            ReturnResponse<List<SystemSlide>> response = new();

            var data = context.SystemSlide.AsNoTracking();

            if (criteria.Id is not null && criteria.Id != 0)
                data = data.Where(i => i.Id == criteria.Id);

            var model = await data.ToListAsync().ConfigureAwait(false);
            if (model is null || model.Count == 0)
                response.Success([], CommonResources.NotFound);
            else
                response.Success(model, CommonResources.Ok);

            return response;
        }


    }
}
