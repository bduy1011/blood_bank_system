using BB.CR.Providers.Bases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDMHuyenRepository
    {
        Task<ReturnResponse<List<DMHuyenView>>> LoadAsync(ILogger logger, IMapper mapper);
        Task<ReturnResponse<bool>> UpdateAsync(IList<DMHuyenView> views, ILogger logger);
    }
}
