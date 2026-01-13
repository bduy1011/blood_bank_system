using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class DMDonViCapMauRepository : IDMDonViCapMauRepository
    {
        public async Task<ReturnResponse<DMDonViCapMauView>> GetAsync(string ma, ILogger logger, IMapper mapper)
        {
            var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMDonViCapMauUseCase.GetAsync(ma, context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<DMDonViCapMau>>> LoadAsync(ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(async () => await DMDonViCapMauUseCase.LoadAsync(context).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DMDonViCapMauView>> UpdateAsync(string ma, DMDonViCapMau model, ILogger logger, IMapper mapper)
        {
            var context = new BloodBankContext();
            var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMDonViCapMauUseCase.UpdateAsync(ma, model, context, mapper).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<bool>> UpdateLocalAsync(List<DMDonViCapMau> model, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMDonViCapMauUseCase.UpdateLocalAsync(model, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }
    }
}
