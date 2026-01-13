using BB.CR.Providers.Bases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDMTinhRepository
    {
        Task<ReturnResponse<List<DMTinhView>>> LoadAsync(ILogger logger, IMapper mapper);
    }
}
