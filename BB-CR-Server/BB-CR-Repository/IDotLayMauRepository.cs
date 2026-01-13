using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDotLayMauRepository
    {
        Task<ReturnResponse<List<DotLayMauView>>> LoadByCriteria(DotLayMauCriteria criteria, ILogger logger);
        Task<ReturnResponse<Models.DotLayMau>> UpdateByIdAndTinhTrang(long lichLayMauId, TinhTrangDotLayMau tinhTrang, ILogger logger);
        Task<ReturnResponse<List<DotLayMau>>> UpdateLocalAsync(List<DotLayMau> model, ILogger logger);
    }
}
