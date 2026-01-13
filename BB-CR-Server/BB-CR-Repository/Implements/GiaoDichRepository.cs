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
    public class GiaoDichRepository : IGiaoDichRepository
    {
        public async Task<ReturnResponse<GiaoDichView>> ApproveAsync(long id, TinhTrangGiaoDich status, ILogger logger, string idCardNo, IMapper mapper)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GiaoDichUseCase.ApproveAsync(id, status, context, idCardNo, mapper).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<GiaoDichView>> CreateAsync(GiaoDich model, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var respone = await BaseUseCase.ExecuteAsync(async () => await GiaoDichUseCase.CreateAsync(model, mapper, context, idCardNo).ConfigureAwait(false)
            , logger
            , context
            , transaction).ConfigureAwait(false);

            return respone;
        }

        public async Task<ReturnResponse<bool>> DeleteAsync(long id, ILogger logger, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GiaoDichUseCase.DeleteAsync(id, context, idCardNo).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<GiaoDichView>> GetAsync(long id, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(async () => await GiaoDichUseCase.GetAsync(id, mapper, context).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<GiaoDichView>>> LoadAsync(GiaoDichCriteria criteria, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(async () => await GiaoDichUseCase.LoadAsync(criteria, mapper, context, idCardNo).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<GiaoDichView>> UpdateAsync(long id, GiaoDich model, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync()
                .ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await GiaoDichUseCase.UpdateAsync(id, model, mapper, context, idCardNo).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }
    }
}
