using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class DMTinhUseCase
    {
        public static async Task<ReturnResponse<List<DMTinhView>>> LoadAsync(BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<List<DMTinhView>>();
            var data = await context.DMTinh.AsNoTracking().OrderByDescending(i => i.UuTien).ThenBy(i => i.Ten).ToListAsync().ConfigureAwait(false);
            var model = mapper.Map<List<DMTinhView>>(data);
            if (model is null || model.Count == 0)
                response.Success([], CommonResources.NotFound);
            else
                response.Success(model, CommonResources.Ok);
            return response;
        }
    }
}
