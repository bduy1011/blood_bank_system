using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class DMTinhRepository : IDMTinhRepository
    {
        public async Task<ReturnResponse<List<DMTinhView>>> LoadAsync(ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();
            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMTinhUseCase.LoadAsync(context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
            return response;
        }
    }
}
