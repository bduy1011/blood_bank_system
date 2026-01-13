using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views.Authenticate;
using BB.CR.Views.Otp;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class DMNguoiHienMauRepository : IDMNguoiHienMauRepository
    {
        public async Task<ReturnResponse<DMNguoiHienMau>> CreateAsync(DMNguoiHienMau model, ILogger logger)
        {
            using BloodBankContext context = new();
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            ReturnResponse<DMNguoiHienMau> response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.CreateAsync(model, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DMNguoiHienMau>> CreateRegisterAsync(PersonalInfo view, IMapper mapper, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.CreateRegisterAsync(view, context, mapper).ConfigureAwait(false),
                logger,
                context,
                transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<DMNguoiHienMau>> GetAsync(int id, ILogger logger)
        {
            using BloodBankContext context = new();

            ReturnResponse<DMNguoiHienMau> response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.GetAsync(id, context)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DMNguoiHienMau?>> GetAsync(string hoTen, int namSinh, string cmnd, ILogger logger)
        {
            using var context = new BloodBankContext();
            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.GetAsync(hoTen, namSinh, cmnd, context).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
            return response;
        }

        public async Task<ResultView?> GetAsync(string idCardNr
            , ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ReadAsync(async () => await DMNguoiHienMauUseCase.GetAsync(idCardNr, context).ConfigureAwait(false)
            , logger
            , context).ConfigureAwait(false);

            return response;
        }

        public async Task<DMNguoiHienMau?> GetByClientIdAsync(string clientId, ILogger logger)
        {
            using var context = new BloodBankContext();
            return await BaseUseCase.ReadAsync(
                async () => await DMNguoiHienMauUseCase.GetByClientIdAsync(clientId, context).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
        }

        public async Task<ReturnResponse<DMNguoiHienMau>> GetByIdCardAsync(string idCard, string? phoneNumber, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.GetByIdCardAsync(idCard, phoneNumber, context).ConfigureAwait(false),
                logger,
                context,
                transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<List<DMNguoiHienMau>>> LoadAsync(Views.Criterias.DMNguoiHienMauCriteria criteria, ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.LoadAsync(criteria, context).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DMNguoiHienMau>> UpdateAsync(int id, DMNguoiHienMau model, ILogger logger)
        {
            ReturnResponse<DMNguoiHienMau> response = new();

            var context = new BloodBankContext();
            var transaction = await context.Database
                .BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                var dmNguoiHienMau = await context.DMNguoiHienMau
                    .AsNoTracking().FirstOrDefaultAsync(i => i.NguoiHienMauId == id).ConfigureAwait(false);
                if (dmNguoiHienMau is null)
                {
                    response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);
                }
                else
                {
                    dmNguoiHienMau = model.Adapt<DMNguoiHienMau>();
                    context.DMNguoiHienMau.Update(dmNguoiHienMau);
                    int count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                    {
                        response.Error(System.Net.HttpStatusCode.NoContent, CommonResources.NoContent);
                    }
                    else
                    {
                        await transaction.CommitAsync().ConfigureAwait(false);
                        response.Success(dmNguoiHienMau, CommonResources.Ok);
                    }
                }
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                logger.Log(LogLevel.Error, "{message}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }
            finally
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
                await context.DisposeAsync().ConfigureAwait(false);
            }

            return response;
        }

        public async Task<ReturnResponse<List<DMNguoiHienMau>>> UpdateAsync(List<DMNguoiHienMau> model, ILogger logger)
        {
            using BloodBankContext context = new();
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            ReturnResponse<List<DMNguoiHienMau>> response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.UpdateAsync(model, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<DMNguoiHienMau?>> UpdateAsync(SettingUserView view, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DMNguoiHienMauUseCase.UpdateAsync(view, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }
    }
}
