using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class DMChungUseCase
    {
        public static async Task<ReturnResponse<DMChungView>> LoadAsync(BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<DMChungView>();

            var dmTinhs = await context.DMTinh.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var dmHuyens = await context.DMHuyen.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var dmXas = await context.DMXa.AsNoTracking().ToListAsync().ConfigureAwait(false);

            var data = new DMChungView();
            if (dmTinhs?.Count > 0) data.DMTinhs = mapper.Map<List<DMTinhView>>(dmTinhs);
            if (dmHuyens?.Count > 0) data.DMHuyens = mapper.Map<List<DMHuyenView>>(dmHuyens);
            if (dmXas?.Count > 0) data.DMXas = mapper.Map<List<DMXaView>>(dmXas);

            response.Success(data, CommonResources.Ok);
            return response;
        }
    }
}
