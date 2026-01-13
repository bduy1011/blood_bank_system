using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Authenticate;
using BB.CR.Views.Otp;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dm-nguoi-hien-mau"), ApiController, Authorize]
    public class DMNguoiHienMauController(IDMNguoiHienMauRepository dmNguoiHienMauRepository,
        ILogger<DMNguoiHienMauController> logger,
        IMapper mapper) : ControllerBase
    {
        private readonly IDMNguoiHienMauRepository dmNguoiHienMauRepository = dmNguoiHienMauRepository;
        private readonly ILogger<DMNguoiHienMauController> logger = logger;
        private readonly IMapper mapper = mapper;

        [HttpPost("load"), SwaggerOperation(Summary = "Get theo model search DMNguoiHienMau")]
        public async Task<IActionResult> LoadAsync([FromBody] Views.Criterias.DMNguoiHienMauCriteria criteria)
        {
            var response = new ReturnResponse<List<Models.DMNguoiHienMau>>();

            try
            {
                response = await dmNguoiHienMauRepository
                    .LoadAsync(criteria, logger).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return Ok(response);
        }

        [HttpPut("update/{nguoiHienMauId}"), SwaggerOperation(Summary = "Update thông tin NHM theo NguoiHienMauId")]
        public async Task<IActionResult> UpdateAsync(int nguoiHienMauId, [FromBody] Views.DMNguoiHienMauView nguoiHienMau)
        {
            var response = new ReturnResponse<Models.DMNguoiHienMau>();

            try
            {
                Models.DMNguoiHienMau model = nguoiHienMau.Adapt<Models.DMNguoiHienMau>();
                response = await dmNguoiHienMauRepository
                    .UpdateAsync(nguoiHienMauId, model, logger).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return Ok(response);
        }

        [HttpPost("create-or-update"), SwaggerOperation(Summary = "Cập nhật danh sách người hiến máu từ local")]
        public async Task<IActionResult> UpdateAsync([FromBody] List<DMNguoiHienMau> model)
        {
            ReturnResponse<List<DMNguoiHienMau>> response = await BaseHandler.ExecuteAsync(
                async () => await dmNguoiHienMauRepository.UpdateAsync(model, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

#if DEBUG
        [HttpGet("get"), SwaggerOperation(Summary = "Get thông tin người hiến máu theo param")]
        public async Task<IActionResult> GetAsync([FromQuery] string hoTen, [FromQuery] int namSinh, [FromQuery] string cmnd)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmNguoiHienMauRepository.GetAsync(hoTen, namSinh, cmnd, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }
#endif

        [HttpPut("mapping"), SwaggerOperation(Summary = "Mapping thông tin người hiến máu")]
        public async Task<IActionResult> MappingAsync([FromBody] SettingUserView view)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmNguoiHienMauRepository.UpdateAsync(view, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("get/{identityCard}"), SwaggerOperation(Summary = "Lấy thông tin người hiến máu đã có để mapping với số điện thoại")]
        public async Task<IActionResult> GetAsync(string identityCard, [FromQuery] string? phoneNumber = null)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmNguoiHienMauRepository.GetByIdCardAsync(identityCard, phoneNumber, logger).ConfigureAwait(false),
                logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost("create"), SwaggerOperation(Summary = "Cập nhật thông tin người dùng mới")]
        public async Task<IActionResult> PostAsync([FromBody] PersonalInfo personalInfo)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmNguoiHienMauRepository.CreateRegisterAsync(personalInfo, mapper, logger).ConfigureAwait(false),
                logger).ConfigureAwait(false);
            return Ok();
        }
    }
}
