using BB.CR.Models;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dm-don-vi-cap-mau"), ApiController, Authorize]
    public class DMDonViCapMauController(IDMDonViCapMauRepository _dmDonViCapMauRepository, ILogger<DMDonViCapMauController> _logger, IMapper _mapper) : ControllerBase
    {
        private readonly IDMDonViCapMauRepository dmDonViCapMauRepository = _dmDonViCapMauRepository;
        private readonly ILogger<DMDonViCapMauController> logger = _logger;
        private readonly IMapper mapper = _mapper;

        [HttpGet("get/{ma}"), SwaggerOperation(Summary = "Lấy thông tin đơn vị cấp máu (DMDonViCapMau)")]
        public async Task<IActionResult> GetAsync(string ma)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmDonViCapMauRepository.GetAsync(ma, logger, mapper).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPut("update/{ma}"), SwaggerOperation(summary: "Cập nhật thông tin đơn vị cấp máu (DMDonViCapMau)")]
        public async Task<IActionResult> UpdateAsync(string ma, [FromBody] DMDonViCapMau model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmDonViCapMauRepository.UpdateAsync(ma, model, logger, mapper).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPut("create-or-update"), SwaggerOperation(Summary = "Cập nhật danh sách đơn vị cấp máu từ local")]
        public async Task<IActionResult> UpdateLocalAsync([FromBody] List<DMDonViCapMau> model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmDonViCapMauRepository.UpdateLocalAsync(model, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPost("load"), SwaggerOperation(Summary = "Danh sách đơn vị cấp máu với active = true")]
        public async Task<IActionResult> LoadAsync()
        {
            var response = await BaseHandler.ExecuteAsync(async () => await dmDonViCapMauRepository.LoadAsync(logger).ConfigureAwait(false)
            , logger).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
