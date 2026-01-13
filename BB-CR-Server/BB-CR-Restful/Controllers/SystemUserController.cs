using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Authenticate;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/system-user"), ApiController, Authorize]
    public class SystemUserController(ISystemUserRepository _systemUserRepository, ILogger<SystemUserController> _logger, IMapper _mapper, IConfiguration _configuration, IDMNguoiHienMauRepository _dmNguoiHienMauRepository
        , IHttpContextAccessor _contextAccessor) : BaseController(_contextAccessor)
    {
        private readonly ISystemUserRepository systemUserRepository = _systemUserRepository;
        private readonly ILogger<SystemUserController> logger = _logger;
        private readonly IMapper mapper = _mapper;

        private readonly IConfiguration configuration = _configuration;
        private readonly IDMNguoiHienMauRepository dmNguoiHienMauRepository = _dmNguoiHienMauRepository;

        [HttpPost("create"), SwaggerOperation(Summary = "Tạo mới thông tin SystemUser")]
        public async Task<IActionResult> CreateAsync([FromBody] RegisterView view)
        {
            SystemUser model = view.Adapt<SystemUser>();

            ReturnResponse<SystemUser> response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.CreateAsync(model, logger)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPut("update/{code}/{isModIdCard}"), SwaggerOperation(Summary = "Cập nhật thông tin SystemUser")]
        public async Task<IActionResult> UpdateAsync(string code, [FromBody] SystemUser model, bool isModIdCard)
        {
            logger.Log(LogLevel.Information, BaseVariable.ERROR_MESSAGE, JsonConvert.SerializeObject(model));

            if (string.IsNullOrWhiteSpace(model.DeviceId)) 
            {
                model.DeviceId = GetDevice();
            }

            if (string.IsNullOrWhiteSpace(model.FireBaseToken)) 
            {
                model.FireBaseToken = GetFirebaseToken();
            }

            var _response = await BaseHandler.ExecuteAsync(
                    async () => await systemUserRepository.UpdateAsync(code, model, logger, mapper).ConfigureAwait(false)
                    , logger).ConfigureAwait(false);

            if (_response is not null
                && _response.Data is not null
                && _response.Status == System.Net.HttpStatusCode.OK)
            {
                if (isModIdCard) // Cập nhật CCCD/CMND
                {
                    var authorize = new AuthorizeView
                    {
                        Name = _response?.Data?.Name,
                        UserCode = _response?.Data?.UserCode,
                        AppRole = _response?.Data?.AppRole ?? AppRole.User,
                        Active = _response?.Data?.Active ?? false,
                        DMNguoiHienMau = null,
                        PhoneNumber = _response?.Data?.PhoneNumber,
                        IdCardNr = _response?.Data?.IdCardNr,
                    };

                    var token = Helper.GenerateToken(_response?.Data, configuration);
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

                    var response = new ReturnResponse<AuthorizeView>();
                    response.Success(authorize, CommonResources.Ok);

                    return Ok(response);
                }
            }

            return Ok(_response);
        }

        [HttpPut("update-local"), SwaggerOperation(Summary = "Cập nhật thông tin SystemUser từ local")]
        public async Task<IActionResult> UpdateLocalAsync([FromBody] List<SystemUser> model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.UpdateLocalAsync(model, logger, mapper).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpDelete("delete-account/{userCode}"), SwaggerOperation(Summary = "Xoá tài khoản người dùng")]
        public async Task<IActionResult> DeleteAsync(string userCode)
        {
            var response = await BaseHandler.ExecuteAsync(async () => await systemUserRepository.DeleteAsync(userCode, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [Authorize, HttpPatch("xoa-cmnd-new/{idCard}"), SwaggerOperation(Summary = "Xoá CMND/CCCD của người dùng (Admin) dựa vào CMND")]
        public async Task<IActionResult> RemoveCMND(string idCard)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await systemUserRepository.RemoveIdCardAsync(idCard, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [Authorize, HttpGet("re-load-information")]
        public async Task<IActionResult> GetByIdCardNrAsync()
        {
            var response = new ReturnResponse<AuthorizeView>();

            try
            {
                var repository = await systemUserRepository.GetByIdCardNrAsync(this.GetIdentityCard()!, logger).ConfigureAwait(false);

                var authorize = new AuthorizeView
                {
                    Name = repository?.Data?.Name,
                    UserCode = repository?.Data?.UserCode,
                    AppRole = repository?.Data?.AppRole ?? AppRole.User,
                    Active = repository?.Data?.Active ?? false,
                    DMNguoiHienMau = null,
                    PhoneNumber = repository?.Data?.PhoneNumber,
                    IdCardNr = repository?.Data?.IdCardNr,
                };

                if (repository is null)
                    response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);
                else
                {
                    if (repository.Status != System.Net.HttpStatusCode.OK)
                        response.Error(repository.Status, repository.Message);
                    else
                    {
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
    }
}
