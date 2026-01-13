using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ICauHinhTonKhoRepository : IBaseRepository<CauHinhTonKhoView, CauHinhTonKhoCriteria, CauHinhTonKho>
    {
        Task<ReturnResponse<CauHinhTonKhoView>> InitAsync(DateTime ngay, ILogger logger, IMapper mapper);
    }
}
