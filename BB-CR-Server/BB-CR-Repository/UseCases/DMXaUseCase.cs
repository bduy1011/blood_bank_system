using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class DMXaUseCase
    {
        public static async Task<ReturnResponse<List<DMXaView>>> LoadAsync(BloodBankContext context, IMapper mapper)
        {
            var data = await context.DMXa.AsNoTracking().Where(i => i.Active).OrderBy(i => i.Ten).ToListAsync().ConfigureAwait(false) ?? [];
            var view = mapper.Map<List<DMXaView>>(data);

            var response = new ReturnResponse<List<DMXaView>>();
            response.Success(view, CommonResources.Ok);
            return response;
        }

        public static async Task<ReturnResponse<bool>> UpdateAsync(List<DMXaView> views
            , BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            // Xoá hết dữ liệu hiện có.
            var datas = await context.DMXa.AsNoTracking().ToListAsync().ConfigureAwait(false);
            if (datas?.Count > 0)
            {
                context.DMXa.RemoveRange(datas);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            var models = views.Adapt<List<Models.DMXa>>();
            await context.DMXa.AddRangeAsync(models);

            var count = await context.SaveChangesAsync().ConfigureAwait(false);

            if (count == 0)
                response.Error(System.Net.HttpStatusCode.NoContent, CommonResources.NoContent);
            else
                response.Success(true, CommonResources.Ok);

            return response;
        }
    }
}
