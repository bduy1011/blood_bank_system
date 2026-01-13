using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Rest.MessageQueues;
using BB.CR.Rest.Services;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using BB.CR.Views.Otp;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BB.CR.Rest.Controllers
{
    [Route("api/system-user"), ApiController]
    public class AuthenticateController(ISystemUserRepository systemUserRepository
        , IDMNguoiHienMauRepository dmNguoiHienMauRepository
        , ILogger<AuthenticateController> logger
        , IConfiguration configuration
        , ITokenService tokenService
        , ICapMatKhauRepository capMatKhauRepository
        , IMessageProducer messageProducer
        , IHttpContextAccessor _contextAccessor) : ControllerBase
    {
        private readonly ISystemUserRepository systemUserRepository = systemUserRepository;
        private readonly IDMNguoiHienMauRepository dmNguoiHienMauRepository = dmNguoiHienMauRepository;
        private readonly ILogger<AuthenticateController> logger = logger;
        private readonly IConfiguration configuration = configuration;

        private readonly ITokenService _tokenService = tokenService;

        private readonly ICapMatKhauRepository _capMatKhauRepository = capMatKhauRepository;
        private readonly IHttpContextAccessor contextAccessor = _contextAccessor;

        [AllowAnonymous, HttpPost("login"), SwaggerOperation(Summary = "Đăng nhập hệ thống app-mobile")]
        public async Task<IActionResult> Login([FromBody] Views.Authenticate.LoginView login)
        {
            var response = new ReturnResponse<Views.Authenticate.AuthorizeView>();

            try
            {
                string fireBaseToken = string.Empty;
                if (contextAccessor is not null
                    && contextAccessor.HttpContext is not null)
                {
                    contextAccessor.HttpContext.Request.Headers.TryGetValue("FireBaseToken", out var _fireBaseToken);
                    fireBaseToken = _fireBaseToken!;
                }

                var repository = await systemUserRepository.GetAsync(login, logger, fireBaseToken).ConfigureAwait(false);

                var authorize = new Views.Authenticate.AuthorizeView
                {
                    Name = repository?.Data?.Name,
                    UserCode = repository?.Data?.UserCode,
                    AppRole = repository?.Data?.AppRole ?? AppRole.User,
                    Active = repository?.Data?.Active ?? false,
                    DMNguoiHienMau = null,
                    PhoneNumber = repository?.Data?.PhoneNumber,
                    IdCardNr = repository?.Data?.IdCardNr,

                    IsDataQLMau = repository?.Data?.IsDataQLMau
                };

                if (repository is null)
                    response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);
                else
                {
                    if (repository.Status != System.Net.HttpStatusCode.OK)
                        response.Error(repository.Status, repository.Message);
                    else
                    {
                        await Task.Delay(Settings.DelayTime);

                        var token = Helper.GenerateToken(repository.Data, configuration);
                        authorize.AccessToken = token;

                        if (!string.IsNullOrWhiteSpace(authorize.IdCardNr))
                        {
                            var view = await BaseHandler.ExecuteAsync(async () => await dmNguoiHienMauRepository.GetAsync(authorize.IdCardNr ?? string.Empty, logger).ConfigureAwait(false)
                            , logger).ConfigureAwait(false);

                            authorize.DMNguoiHienMau = view?.NguoiHienMau;
                            authorize.SoLanHienMau = view?.NguoiHienMau?.SoLanHienMau;
                            authorize.NgayHienMauGanNhat = view?.NgayHienMauGanNhat;
                            authorize.DuongTinhGanNhat = view?.DuongTinhGanNhat ?? true;
                        }

                        response.Success(authorize, CommonResources.Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return Ok(response);
        }

        [AllowAnonymous, HttpPost("register"), SwaggerOperation(Summary = "Đăng ký người dùng app-mobile")]
        public async Task<IActionResult> Register([FromBody] Views.Authenticate.RegisterView register)
        {
            var response = new ReturnResponse<Models.SystemUser>();

            logger.Log(LogLevel.Information, BaseVariable.ERROR_MESSAGE, JsonConvert.SerializeObject(register));
            
                try
            {
                var systemUser = register.Adapt<Models.SystemUser>();
                //systemUser.MaDonViCapMau = $"{1}"; // Remove 20240927
                systemUser.Active = true;
                systemUser.IdCardNr = register.UserCode; // UserCode (CMND) sẽ tự gắn vào IdCardNr
                systemUser.Password = AlgorisMd5.GetMd5Hash(register.Password);
                if (!string.IsNullOrWhiteSpace(register.DeviceId))
                {
                    systemUser.DeviceId = register.DeviceId;
                }

                response = await systemUserRepository
                    .CreateAsync(systemUser, logger).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return Ok(response);
        }

        [AllowAnonymous, HttpPost("register-phone"), SwaggerOperation(Summary = "Đăng ký người dùng từ số điện thoại")]
        public async Task<IActionResult> RegisterPhoneNumber([FromBody] RegisterOtpView register)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.RegisterByCMNDAsync(register.CMND, register.DeviceId!, register.FullName, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            if (!Settings.BreakGenerate)
            {
                // Flow send content to brand name and send otp to end-user
            }

            return Ok(response);
        }

        [AllowAnonymous, HttpPost("check-otp"), SwaggerOperation(Summary = "Kiểm tra otp có hợp lệ hay không")]
        public async Task<IActionResult> CheckOtpValid([FromBody] RegisterOtpView register)
        {
            var response = new ReturnResponse<Views.Authenticate.AuthorizeView>();
            var repository = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.CheckOtpAsync(register, logger).ConfigureAwait(false),
                logger).ConfigureAwait(false);

            if (repository.Status == System.Net.HttpStatusCode.OK
                && repository.Data is not null)
            {
                var token = Helper.GenerateToken(repository.Data, configuration);

                var authorize = new Views.Authenticate.AuthorizeView
                {
                    AccessToken = token,
                    Name = repository.Data.Name,
                    UserCode = repository.Data.UserCode ?? repository.Data.PhoneNumber!,
                    AppRole = repository.Data.AppRole,
                    Active = repository.Data.Active,
                    PhoneNumber = repository.Data.PhoneNumber,
                    DMNguoiHienMau = null,
                    IdCardNr = repository.Data.IdCardNr,
                };

                var nguoiHienMau = await BaseHandler.ExecuteAsync(
                    async () => await dmNguoiHienMauRepository.GetByIdCardAsync(authorize.IdCardNr!, authorize.PhoneNumber!, logger).ConfigureAwait(false),
                    logger).ConfigureAwait(false);
                if (nguoiHienMau is not null
                    && nguoiHienMau.Status == System.Net.HttpStatusCode.OK
                    && nguoiHienMau.Data is not null)
                {
                    authorize.DMNguoiHienMau = nguoiHienMau.Data.Adapt<DMNguoiHienMauView>();
                }

                response.Success(authorize, CommonResources.Ok);
            }

            return Ok(response);
        }

        [AllowAnonymous, HttpPost("resend-otp"), SwaggerOperation(Summary = "Gửi lại mã OTP cho số điện thoại")]
        public async Task<IActionResult> ResendPhoneNumber([FromBody] RegisterOtpView register)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.ResendOtpAsync(register, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            if (!Settings.BreakGenerate)
            {
                // Flow send content to brand name and send otp to end-user
            }

            return Ok(response);
        }

        [Authorize, HttpPost("change-password/{isRegister}"), SwaggerOperation(Summary = "Thay đổi mật khẩu khi đăng ký / mật khẩu cũ")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordOtpView view, bool isRegister = false)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.ChangePasswordAsync(view, isRegister, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize, HttpGet("logout"), SwaggerOperation(Summary = "Đăng xuất khỏi hệ thống")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _tokenService.RevokeToken(token);
            await Task.Delay(1000);
            return Ok(new { Message = "Logout successful" });
        }

        [AllowAnonymous, HttpPost("tao-yeu-cau-cap-mk"), SwaggerOperation(Summary = "Tạo yêu cầu cấp mật khẩu gửi cho local")]
        public async Task<IActionResult> CreateCapMatKhau([FromBody] CapMatKhau model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await _capMatKhauRepository.CreateAsync(model, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [Authorize, HttpPost("ds-yeu-cau-cho-local"), SwaggerOperation(Summary = "Danh sách các yêu cầu cho local")]
        public async Task<IActionResult> LoadCapMatKhau([FromBody] CapMatKhauCriteria criteria)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await _capMatKhauRepository.LoadAsync(criteria, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [AllowAnonymous, HttpPost("change-password-admin"), SwaggerOperation(Summary = "Thay đổi mật khẩu của admin")]
        public async Task<IActionResult> ChangePasswordAdmin([FromBody] ChangePasswordOtpView view)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.ChangePasswordAsync(view, true, logger, true).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            messageProducer.Send(response, nameof(ChangePasswordAdmin), logger);

            return Ok(response);
        }

        [Authorize, HttpPatch("xoa-cmnd/{userCode}"), SwaggerOperation(Summary = "Xoá CMND/CCCD của người dùng (Admin)")]
        public async Task<IActionResult> RemoveCMND(string userCode)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.RemoveAsync(userCode, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [AllowAnonymous, HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            string newToken = string.Empty;

            try
            {
                var claims = contextAccessor?.HttpContext?.User.Claims;
                if (claims is null)
                    return Unauthorized(CommonResources.SessionTimeOut);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                    configuration["Jwt:Issuer"],
                    claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

                await Task.Delay(1000);

                newToken = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
            }

            return Ok(newToken);
        }
    }
}
