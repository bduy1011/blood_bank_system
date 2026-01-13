using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views.Criterias;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ICapMatKhauRepository
    {
        Task<ReturnResponse<bool>> CreateAsync(CapMatKhau model, ILogger logger);
        Task<ReturnResponse<List<CapMatKhau>>> LoadAsync(CapMatKhauCriteria criteria, ILogger logger);
    }
}
