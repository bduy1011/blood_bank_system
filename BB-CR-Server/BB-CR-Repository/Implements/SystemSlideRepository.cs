using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class SystemSlideRepository : ISystemSlideRepository
    {
        public async Task<ReturnResponse<List<SystemSlide>>> LoadAsync(SystemSlideCriteria criteria, ILogger logger)
        {
            using BloodBankContext context = new();

            ReturnResponse<List<SystemSlide>> response = await BaseUseCase.ExecuteAsync(
                async () => await SystemSlideUseCase.LoadAsync(criteria, context)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

    }
}
