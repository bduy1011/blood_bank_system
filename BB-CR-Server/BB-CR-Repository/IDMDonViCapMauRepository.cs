using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDMDonViCapMauRepository
    {
        Task<ReturnResponse<DMDonViCapMauView>> GetAsync(string ma, ILogger logger, IMapper mapper);
        Task<ReturnResponse<DMDonViCapMauView>> UpdateAsync(string ma, DMDonViCapMau model, ILogger logger, IMapper mapper);

        Task<ReturnResponse<bool>> UpdateLocalAsync(List<DMDonViCapMau> model, ILogger logger);

        Task<ReturnResponse<List<DMDonViCapMau>>> LoadAsync(ILogger logger);
    }
}
