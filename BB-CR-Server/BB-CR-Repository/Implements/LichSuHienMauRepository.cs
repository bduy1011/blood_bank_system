using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class LichSuHienMauRepository : ILichSuHienMauRepository
    {
        public async Task<LetterView?> GetLetterAsync(long id, ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ReadAsync(async () => await LichSuHienMauUseCase.GetLetterAsync(id, context).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<LichSuHienMauView>>> LoadAsync(LichSuHienMauCriteria criteria, ILogger logger
            , IMapper mapper)
        {
            using BloodBankContext context = new();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await LichSuHienMauUseCase.LoadAsync(criteria, context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<bool>> UpdateLocalAsync(List<LichSuHienMau> model, ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await LichSuHienMauUseCase.UpdateLocalAsync(model, context, mapper).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }
    }
}
