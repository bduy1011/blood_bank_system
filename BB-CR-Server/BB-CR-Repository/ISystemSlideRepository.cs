using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ISystemSlideRepository
    {
        Task<ReturnResponse<List<SystemSlide>>> LoadAsync(SystemSlideCriteria criteria, ILogger logger);
    }
}
