using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class TinTucRepository : ITinTucRepository
    {
        public async Task<ReturnResponse<TinTuc>> GetAsync(int id, ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(async () => await TinTucUseCase.GetAsync(context, id).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<TinTuc>>> LoadAsync(ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(async () => await TinTucUseCase.LoadAsync(context).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }
    }
}
