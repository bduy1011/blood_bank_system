using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class DMHuyenRepository : IDMHuyenRepository
    {
        public async Task<ReturnResponse<List<DMHuyenView>>> LoadAsync(ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();
            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMHuyenUseCase.LoadAsync(context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<bool>> UpdateAsync(IList<DMHuyenView> views
            , ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMHuyenUseCase.UpdateAsync(views, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }
    }
}
