using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IGiaoDichRepository : IBaseRepository<GiaoDichView, GiaoDichCriteria, GiaoDich>
    {
        Task<ReturnResponse<GiaoDichView>> ApproveAsync(long id, TinhTrangGiaoDich status, ILogger logger, string idCardNo, IMapper mapper);
    }
}
