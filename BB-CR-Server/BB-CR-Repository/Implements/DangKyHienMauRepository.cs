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
    public class DangKyHienMauRepository : IDangKyHienMauRepository
    {
        public async Task<ReturnResponse<DangKyHienMauView>> CreateAsync(DangKyHienMau model, ILogger logger, IMapper mapper, string? userCode, string? identityCard)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DangKyHienMauUseCase.CreateAsync(model, context, mapper, userCode, identityCard).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DangKyHienMauView>> GetAsync(long id, ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DangKyHienMauUseCase.GetAsync(id, context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<DangKyHienMauView>>> LoadAsync(DangKyHienMauCriteria criteria
            , ILogger logger
            , IMapper mapper
            , string? userCode
            , bool isLocal = false)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DangKyHienMauUseCase.LoadAsync(criteria, context, mapper, userCode, isLocal).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DangKyHienMauView>> UpdateAsync(long id, DangKyHienMau model, ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DangKyHienMauUseCase.UpdateAsync(id, model, context, mapper).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }
    }
}
