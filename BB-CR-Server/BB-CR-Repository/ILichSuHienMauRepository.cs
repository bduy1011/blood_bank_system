using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ILichSuHienMauRepository
    {
        Task<ReturnResponse<List<LichSuHienMauView>>> LoadAsync(LichSuHienMauCriteria criteria, ILogger logger
            , IMapper mapper);

        Task<ReturnResponse<bool>> UpdateLocalAsync(List<LichSuHienMau> model, ILogger logger, IMapper mapper);

        Task<LetterView?> GetLetterAsync(long id, ILogger logger);
    }
}
