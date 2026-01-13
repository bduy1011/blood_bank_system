using BB.CR.Models;
using BB.CR.Providers.Bases;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ITinTucRepository
    {
        Task<ReturnResponse<List<TinTuc>>> LoadAsync(ILogger logger);
        Task<ReturnResponse<TinTuc>> GetAsync(int id, ILogger logger);
    }
}
