using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ISystemConfigRepository
    {
        Task<ReturnResponse<List<SystemConfig>>> LoadAsync(SystemConfigCriteria criteria, ILogger logger);
    }
}
