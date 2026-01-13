using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class SystemConfigRepository : ISystemConfigRepository
    {
        public async Task<ReturnResponse<List<SystemConfig>>> LoadAsync(SystemConfigCriteria criteria, ILogger logger)
        {
            using BloodBankContext context = new();

            ReturnResponse<List<SystemConfig>> response = await BaseUseCase.ExecuteAsync(
                async () => await SystemConfigUseCase.LoadAsync(criteria, context)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

    }
}
