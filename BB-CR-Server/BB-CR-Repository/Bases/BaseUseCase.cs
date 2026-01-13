using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BB.CR.Repositories.Bases
{
    public class BaseUseCase
    {
        public static async Task<ReturnResponse<T>> ExecuteAsync<T>(Func<Task<ReturnResponse<T>>> action
            , ILogger logger
            , BloodBankContext context
            , IDbContextTransaction? transaction = null)
        {
            var response = new ReturnResponse<T>();

            try
            {
                response = await action().ConfigureAwait(false);
                if (response.Status == HttpStatusCode.OK && transaction is not null)
                    await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{message}", ex.InnerMessage());
                response.Error(HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
                if (transaction is not null)
                    await transaction.RollbackAsync().ConfigureAwait(false);
            }
            finally
            {
                if (transaction is not null)
                    await transaction.DisposeAsync().ConfigureAwait(false);
                await context.DisposeAsync().ConfigureAwait(false);
            }

            return response;
        }

        public static async Task<T?> ReadAsync<T>(Func<Task<T?>> action
            , ILogger logger
            , BloodBankContext context)
        {
            var response = default(T);

            try
            {
                response = await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{message}", ex.InnerMessage());
            }
            finally
            {
                await context.DisposeAsync().ConfigureAwait(false);
            }

            return response;
        }
    }
}
