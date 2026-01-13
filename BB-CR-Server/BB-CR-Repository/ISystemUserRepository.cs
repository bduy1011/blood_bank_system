using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views.Otp;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ISystemUserRepository
    {
        Task<ReturnResponse<SystemUser>> CreateAsync(SystemUser model, ILogger logger);
        Task<ReturnResponse<SystemUser>> GetAsync(Views.Authenticate.LoginView view, ILogger logger, string fireBaseToken);
        Task<ReturnResponse<SystemUser>> UpdateAsync(Views.Authenticate.ChangePasswordView view, ILogger logger);
        Task<ReturnResponse<SystemUser>> UpdateAsync(string code, SystemUser model, ILogger logger, IMapper mapper);
        Task<ReturnResponse<List<SystemUser>>> UpdateLocalAsync(List<SystemUser> model, ILogger logger, IMapper mapper);

        Task<ReturnResponse<bool>> RegisterByCMNDAsync(string cmnd, string? deviceId, string? fullName, ILogger logger);
        Task<ReturnResponse<SystemUser>> CheckOtpAsync(RegisterOtpView register, ILogger logger);
        Task<ReturnResponse<bool>> ResendOtpAsync(RegisterOtpView register, ILogger logger);

        Task<ReturnResponse<bool>> ChangePasswordAsync(ChangePasswordOtpView view, bool isRegister, ILogger logger
            , bool isOnlyLocal = false);

        Task<ReturnResponse<SystemUser?>> DeleteAsync(string userCode, ILogger logger);

        Task<ReturnResponse<bool>> RemoveAsync(string userCode, ILogger logger);
        Task<ReturnResponse<bool>> RemoveIdCardAsync(string idCard, ILogger logger);

        Task<ReturnResponse<SystemUser>> GetByIdCardNrAsync(string idCardNr, ILogger logger);
    }
}
