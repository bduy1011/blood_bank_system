using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views.Authenticate;
using BB.CR.Views.Otp;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class SystemUserRepository : ISystemUserRepository
    {
        public async Task<ReturnResponse<SystemUser>> CreateAsync(SystemUser model, ILogger logger)
        {
            using BloodBankContext context = new();
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            ReturnResponse<SystemUser> response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.CreateAsync(model, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<SystemUser>> GetAsync(LoginView login, ILogger logger, string fireBaseToken)
        {
            var response = new ReturnResponse<SystemUser>();

            using var context = new BloodBankContext();

            try
            {
                var systemUser = await context.SystemUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.UserCode == login.UserCode && i.Active).ConfigureAwait(false);

                if (systemUser is null)
                {
                    response.Error(System.Net.HttpStatusCode.NotFound, BzLogicResource.AccountUnRegistered);
                }
                else
                {
                    var passwordHashed = AlgorisMd5.GetMd5Hash(login.Password);
                    if (!string.Equals(systemUser.Password, passwordHashed, StringComparison.OrdinalIgnoreCase))
                    {
                        response.Error(System.Net.HttpStatusCode.Conflict, BzLogicResource.PasswordUnMatched);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(fireBaseToken))
                        {
                            try
                            {
                                systemUser.FireBaseToken = fireBaseToken;
                                context.SystemUser
                                    .Update(systemUser);

                                await context
                                    .SaveChangesAsync()
                                    .ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
                            }
                        }

                        response.Success(systemUser, CommonResources.Ok);
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return response;
        }

        public async Task<ReturnResponse<SystemUser>> UpdateAsync(ChangePasswordView changePassword, ILogger logger)
        {
            var response = new ReturnResponse<SystemUser>();

            using var context = new BloodBankContext();
            using var transaction = await context.Database
                .BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                var systemUser = await context.SystemUser
                    .FirstOrDefaultAsync(i => string.Equals(i.UserCode, changePassword.UserCode, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);

                if (systemUser is null)
                {
                    response.Error(System.Net.HttpStatusCode.NotFound, BzLogicResource.AccountUnRegistered);
                }
                else
                {
                    var validErrorMessage = SystemUserUseCase.UpdateByChangePassword(systemUser, changePassword.OldPassword);
                    if (validErrorMessage is not null)
                    {
                        response.Error(System.Net.HttpStatusCode.Conflict, validErrorMessage);
                    }
                    else
                    {
                        systemUser.Password = AlgorisMd5.GetMd5Hash(changePassword.NewPassword);
                        context.SystemUser.Update(systemUser);

                        var count = await context
                            .SaveChangesAsync().ConfigureAwait(false);
                        if (count > 0)
                        {
                            await transaction
                                .CommitAsync().ConfigureAwait(false);
                            response.Success(systemUser, CommonResources.Ok);
                        }
                        else
                        {
                            response.Error(System.Net.HttpStatusCode.NoContent, CommonResources.NoContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
                await transaction
                    .RollbackAsync().ConfigureAwait(false);
            }
            finally
            {
                await transaction
                    .DisposeAsync().ConfigureAwait(false);
                await context
                    .DisposeAsync().ConfigureAwait(false);
            }

            return response;
        }

        public async Task<ReturnResponse<SystemUser>> UpdateAsync(string code, SystemUser model, ILogger logger, IMapper mapper)
        {
            BloodBankContext context = new();
            IDbContextTransaction transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            ReturnResponse<SystemUser> response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.UpdateAsync(code, model, context, mapper).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<List<SystemUser>>> UpdateLocalAsync(List<SystemUser> model, ILogger logger
            , IMapper mapper)
        {
            BloodBankContext context = new();
            IDbContextTransaction transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            ReturnResponse<List<SystemUser>> response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.UpdateLocalAsync(model, context, mapper).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<bool>> RegisterByCMNDAsync(string cmnd,
            string? deviceId,
            string? fullName,
            ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.RegisterByCMNDAsync(cmnd, deviceId, fullName, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<SystemUser>> CheckOtpAsync(RegisterOtpView register,
            ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.CheckOtpAsync(register, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<bool>> ResendOtpAsync(RegisterOtpView register,
            ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.ResendOtpAsync(register, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<bool>> ChangePasswordAsync(ChangePasswordOtpView view
            , bool isRegister
            , ILogger logger
            , bool isOnlyLocal = false)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.ChangePasswordAsync(view, isRegister, context, isOnlyLocal).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }

        public async Task<ReturnResponse<SystemUser?>> DeleteAsync(string userCode, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(async () => await SystemUserUseCase.DeleteAsync(userCode, context).ConfigureAwait(false)
            , logger
            , context
            , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<bool>> RemoveAsync(string userCode, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync()
                .ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.RemoveAsync(userCode, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<bool>> RemoveIdCardAsync(string idCard, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync()
                .ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await SystemUserUseCase.RemoveIdCardAsync(idCard, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<SystemUser>> GetByIdCardNrAsync(string idCardNr, ILogger logger)
        {
            using var context = new BloodBankContext();
            var response = await BaseUseCase
                .ExecuteAsync(async () => await SystemUserUseCase
                    .GetByIdCardNrAsync(idCardNr, context)
                    .ConfigureAwait(false), logger, context)
                .ConfigureAwait(false);
            return response;
        }
    }
}
