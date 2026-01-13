using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views.Authenticate;
using BB.CR.Views.Otp;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDMNguoiHienMauRepository
    {
        Task<ReturnResponse<DMNguoiHienMau>> UpdateAsync(int id, DMNguoiHienMau model, ILogger logger);
        Task<ReturnResponse<List<DMNguoiHienMau>>> LoadAsync(Views.Criterias.DMNguoiHienMauCriteria criteria, ILogger logger);
        Task<ReturnResponse<List<DMNguoiHienMau>>> UpdateAsync(List<DMNguoiHienMau> model, ILogger logger);
        Task<ReturnResponse<DMNguoiHienMau>> GetAsync(int id, ILogger logger);
        Task<ReturnResponse<DMNguoiHienMau>> CreateAsync(DMNguoiHienMau model, ILogger logger);

        Task<ReturnResponse<DMNguoiHienMau?>> GetAsync(string hoTen, int namSinh, string cmnd, ILogger logger);
        Task<ReturnResponse<DMNguoiHienMau?>> UpdateAsync(SettingUserView view, ILogger logger);

        Task<DMNguoiHienMau?> GetByClientIdAsync(string clientId, ILogger logger);

        Task<ReturnResponse<DMNguoiHienMau>> GetByIdCardAsync(string idCard, string? phoneNumber, ILogger logger);
        Task<ReturnResponse<DMNguoiHienMau>> CreateRegisterAsync(PersonalInfo view, IMapper mapper, ILogger logger);

        Task<ResultView?> GetAsync(string idCardNr, ILogger logger);
    }
}
