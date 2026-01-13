using BB.CR.Providers.Bases;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Bases
{
    public interface IBaseRepository<TView, TCriteria, TModel>
    {
        Task<ReturnResponse<TView>> CreateAsync(TModel model, ILogger logger, IMapper mapper, string idCardNo);
        Task<ReturnResponse<bool>> DeleteAsync(long id, ILogger logger, string idCardNo);
        Task<ReturnResponse<TView>> GetAsync(long id, ILogger logger, IMapper mapper, string idCardNo);
        Task<ReturnResponse<List<TView>>> LoadAsync(TCriteria criteria, ILogger logger, IMapper mapper, string idCardNo);
        Task<ReturnResponse<TView>> UpdateAsync(long id, TModel model, ILogger logger, IMapper mapper, string idCardNo);
    }
}
