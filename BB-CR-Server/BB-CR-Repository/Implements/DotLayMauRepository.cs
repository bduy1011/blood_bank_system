using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class DotLayMauRepository : IDotLayMauRepository
    {
        public async Task<ReturnResponse<List<DotLayMauView>>> LoadByCriteria(DotLayMauCriteria criteria, ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase
                .ExecuteAsync(async () => await DotLayMauUseCase.LoadByCriteria(criteria, context), logger, context)
                .ConfigureAwait(false);

            return response;
        }

        public async Task<ReturnResponse<Models.DotLayMau>> UpdateByIdAndTinhTrang(long dotLayMauId, TinhTrangDotLayMau tinhTrang, ILogger logger)
        {
            ReturnResponse<Models.DotLayMau> response = new();

            using BloodBankContext context = new();
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                Models.DotLayMau? data = await context.DotLayMau.AsNoTracking().FirstOrDefaultAsync(i => i.DotLayMauId == dotLayMauId).ConfigureAwait(false);
                if (data is null) // Not found data to update
                {
                    response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);
                }
                else
                {
                    data.TinhTrang = tinhTrang;
                    context.DotLayMau.Update(data);

                    int count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                    {
                        response.Error(System.Net.HttpStatusCode.NoContent, CommonResources.NoContent);
                    }
                    else
                    {
                        await transaction.CommitAsync().ConfigureAwait(false);
                        response.Success(data, CommonResources.Ok);
                    }
                }
            }
            catch (System.Exception ex)
            {
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                await transaction.RollbackAsync().ConfigureAwait(false);
            }
            finally
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
                await context.DisposeAsync().ConfigureAwait(false);
            }

            return response;
        }

        public async Task<ReturnResponse<List<DotLayMau>>> UpdateLocalAsync(List<DotLayMau> model, ILogger logger)
        {
            using var context = new BloodBankContext();
            using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = await BaseUseCase.ExecuteAsync(
                async () => await DotLayMauUseCase.UpdateLocalAsync(model, context).ConfigureAwait(false)
                , logger
                , context
                , transaction).ConfigureAwait(false);
            return response;
        }
    }
}
