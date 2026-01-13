using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class CapMatKhauRepository : ICapMatKhauRepository
    {
        public async Task<ReturnResponse<bool>> CreateAsync(CapMatKhau model, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await CapMatKhauUseCase.CreateAsync(model, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<CapMatKhau>>> LoadAsync(CapMatKhauCriteria criteria, ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await CapMatKhauUseCase.LoadAsync(criteria, context).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }
    }
}
