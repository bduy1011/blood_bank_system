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
    public class GopYRepository : IGopYRepository
    {
        public async Task<ReturnResponse<GopYView>> CreateAsync(GopY model, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GopYUseCase.CreateAsync(model, context, mapper, idCardNo).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<bool>> DeleteAsync(long id, ILogger logger, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GopYUseCase.DeleteAsync(id, context)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<GopYView>> GetAsync(long id, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GopYUseCase.GetAsync(id, context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<List<GopYView>>> LoadAsync(GopYCriteria criteria, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GopYUseCase.LoadAsync(criteria, context, mapper, idCardNo).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<GopYView>> UpdateAsync(long id, GopY model, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GopYUseCase.UpdateAsync(id, model, context, mapper, idCardNo).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }
    }
}
