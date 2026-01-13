using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class DMChungRepository : IDMChungRepository
    {
        public async Task<ReturnResponse<DMChungView>> LoadAsync(ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();
            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMChungUseCase.LoadAsync(context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }
    }
}
