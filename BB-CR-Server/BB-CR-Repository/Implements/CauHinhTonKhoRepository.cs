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
    public class CauHinhTonKhoRepository : ICauHinhTonKhoRepository
    {
        public async Task<ReturnResponse<CauHinhTonKhoView>> CreateAsync(CauHinhTonKho model, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database
                .BeginTransactionAsync()
                .ConfigureAwait(false);

            var response = await BaseUseCase
                .ExecuteAsync(async () => await CauHinhTonKhoUseCase
                    .CreateAsync(model, context, mapper)
                    .ConfigureAwait(false), logger, context, transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<bool>> DeleteAsync(long id, ILogger logger, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database
                .BeginTransactionAsync()
                .ConfigureAwait(false);

            var response = await BaseUseCase
                .ExecuteAsync(async () => await CauHinhTonKhoUseCase
                    .DeleteAsync(id, context)
                    .ConfigureAwait(false), logger, context, transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<CauHinhTonKhoView>> GetAsync(long id, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase
                .ExecuteAsync(async () => await CauHinhTonKhoUseCase
                    .GetAsync(id, context, mapper)
                    .ConfigureAwait(false), logger, context).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<CauHinhTonKhoView>> InitAsync(DateTime ngay, ILogger logger, IMapper mapper)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase
                .ExecuteAsync(async () => await CauHinhTonKhoUseCase
                    .InitAsync(ngay, context, mapper)
                    .ConfigureAwait(false), logger, context).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<List<CauHinhTonKhoView>>> LoadAsync(CauHinhTonKhoCriteria criteria, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase
                .ExecuteAsync(async () => await CauHinhTonKhoUseCase
                    .LoadAsync(criteria, context, mapper)
                    .ConfigureAwait(false), logger, context).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<CauHinhTonKhoView>> UpdateAsync(long id, CauHinhTonKho model, ILogger logger, IMapper mapper, string idCardNo)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database
                .BeginTransactionAsync()
                .ConfigureAwait(false);

            var response = await BaseUseCase
                .ExecuteAsync(async () => await CauHinhTonKhoUseCase
                    .UpdateAsync(id, model, context, mapper)
                    .ConfigureAwait(false), logger, context, transaction).ConfigureAwait(false);
            return response;
        }
    }
}
