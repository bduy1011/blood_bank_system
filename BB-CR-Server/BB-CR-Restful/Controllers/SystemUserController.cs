using System.IO;
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
                        AvatarUrl = _response?.Data?.AvatarUrl != null && _contextAccessor?.HttpContext?.Request != null
                            ? $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/system-user/avatar"
                            : null,
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
                    AvatarUrl = repository?.Data?.AvatarUrl != null && _contextAccessor?.HttpContext?.Request != null
                        ? $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/system-user/avatar"
                        : null,
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

        private static readonly HashSet<string> AllowedAvatarExtensions = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".heic", ".heif" };
        private const long MaxAvatarSizeBytes = 5 * 1024 * 1024; // 5MB

        [HttpPost("upload-avatar"), SwaggerOperation(Summary = "Upload hoặc cập nhật avatar cho user đăng nhập")]
        public async Task<IActionResult> UploadAvatar(IFormFile? file)
        {
            var userCode = GetUserCode();
            if (string.IsNullOrEmpty(userCode))
            {
                var err = new ReturnResponse<object>();
                err.Error(System.Net.HttpStatusCode.Unauthorized, CommonResources.SessionTimeOut);
                return Ok(err);
            }

            if (file == null || file.Length == 0)
            {
                var err = new ReturnResponse<object>();
                err.Error(System.Net.HttpStatusCode.BadRequest, "Chưa chọn ảnh.");
                return Ok(err);
            }

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || !AllowedAvatarExtensions.Contains(ext))
            {
                var err = new ReturnResponse<object>();
                err.Error(System.Net.HttpStatusCode.BadRequest, "Định dạng ảnh không hợp lệ. Chỉ chấp nhận: jpg, jpeg, png, gif, webp, heic, heif.");
                return Ok(err);
            }

            if (file.Length > MaxAvatarSizeBytes)
            {
                var err = new ReturnResponse<object>();
                err.Error(System.Net.HttpStatusCode.BadRequest, "Kích thước ảnh tối đa 5MB.");
                return Ok(err);
            }

            var safeUserCode = string.Join("_", userCode.Split(Path.GetInvalidFileNameChars()));
            var root = Path.Combine(AppContext.BaseDirectory, "App_Data", "avatars");
            var dir = Path.Combine(root, safeUserCode);
            Directory.CreateDirectory(dir);
            var fileName = "avatar" + ext;
            var fullPath = Path.Combine(dir, fileName);

            try
            {
                await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    await file.CopyToAsync(stream).ConfigureAwait(false);

                var relativePath = $"avatars/{safeUserCode}/{fileName}";
                var updateRes = await BaseHandler.ExecuteAsync(
                    async () => await systemUserRepository.UpdateAvatarUrlAsync(userCode, relativePath, logger).ConfigureAwait(false),
                    logger).ConfigureAwait(false);

                if (updateRes?.Status != System.Net.HttpStatusCode.OK)
                    return Ok(updateRes ?? new ReturnResponse<SystemUser>());

                var avatarUrl = _contextAccessor?.HttpContext?.Request != null
                    ? $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/system-user/avatar"
                    : null;
                var ok = new ReturnResponse<object>();
                ok.Success(new { avatarUrl }, CommonResources.Ok);
                return Ok(ok);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                var err = new ReturnResponse<object>();
                err.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
                return Ok(err);
            }
        }

        [HttpGet("avatar"), SwaggerOperation(Summary = "Lấy ảnh avatar của user đăng nhập")]
        public async Task<IActionResult> GetAvatar()
        {
            var userCode = GetUserCode();
            if (string.IsNullOrEmpty(userCode))
                return Unauthorized();

            var userRes = await systemUserRepository.GetByUserCodeAsync(userCode, logger).ConfigureAwait(false);
            if (userRes?.Data?.AvatarUrl == null || userRes.Status != System.Net.HttpStatusCode.OK)
                return NotFound();

            var fullPath = Path.Combine(AppContext.BaseDirectory, "App_Data", userRes.Data.AvatarUrl);
            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var contentType = "image/jpeg";
            var ext = Path.GetExtension(fullPath);
            if (string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase)) contentType = "image/png";
            else if (string.Equals(ext, ".gif", StringComparison.OrdinalIgnoreCase)) contentType = "image/gif";
            else if (string.Equals(ext, ".webp", StringComparison.OrdinalIgnoreCase)) contentType = "image/webp";
            else if (string.Equals(ext, ".heic", StringComparison.OrdinalIgnoreCase) || string.Equals(ext, ".heif", StringComparison.OrdinalIgnoreCase)) contentType = "image/heic";

            return PhysicalFile(fullPath, contentType);
        }
    }
}
