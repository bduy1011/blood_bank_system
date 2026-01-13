using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class DMHuyenUseCase
    {
        public static async Task<ReturnResponse<List<DMHuyenView>>> LoadAsync(BloodBankContext context, IMapper mapper)
        {
            var data = await context.DMHuyen.AsNoTracking().Where(i => i.Active).OrderBy(i => i.Ten).ToListAsync().ConfigureAwait(false) ?? [];
            var view = mapper.Map<List<DMHuyenView>>(data);

            var response = new ReturnResponse<List<DMHuyenView>>();
            response.Success(view, CommonResources.Ok);
            return response;
        }

        public static async Task<ReturnResponse<bool>> UpdateAsync(IList<DMHuyenView> views
            , BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var datas = await context.DMHuyen.AsNoTracking().ToListAsync().ConfigureAwait(false);

            if (datas?.Count > 0)
            {
                context.DMHuyen.RemoveRange(datas);
                var count = await context.SaveChangesAsync().ConfigureAwait(false);

                if (count > 0)
                {
                    var models = views.Adapt<List<DMHuyen>>();
                    await context.DMHuyen.AddRangeAsync(models).ConfigureAwait(false);
                    count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                        response.Error(System.Net.HttpStatusCode.NoContent, CommonResources.NoContent);
                    else
                        response.Success(true, CommonResources.Ok);
                }
            }
            else
                response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);

            return response;
        }
    }
}
