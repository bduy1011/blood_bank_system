using BB.CR.Providers.Bases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDMXaRepository
    {
        Task<ReturnResponse<List<DMXaView>>> LoadAsync(ILogger logger, IMapper mapper);
        Task<ReturnResponse<bool>> UpdateAsync(List<DMXaView> views, ILogger logger);
    }
}
