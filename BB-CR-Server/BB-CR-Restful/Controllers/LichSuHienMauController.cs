using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/lich-su-hien-mau"), ApiController, Authorize]
    public class LichSuHienMauController(
        ILichSuHienMauRepository _lichSuHienMauRepository
        , ILogger<LichSuHienMauController> _logger
        , IMapper _mapper
        , IHttpContextAccessor _httpContextAccessor) : BaseController(_httpContextAccessor)
    {
        private readonly ILichSuHienMauRepository lichSuHienMauRepository = _lichSuHienMauRepository;
        private readonly ILogger<LichSuHienMauController> logger = _logger;
        private readonly IMapper mapper = _mapper;

        [HttpPut("create-or-update"), SwaggerOperation(Summary = "Cập nhật danh sách từ hệ thống local")]
        public async Task<IActionResult> UpdateLocalAsync([FromBody] List<LichSuHienMau> model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await lichSuHienMauRepository.UpdateLocalAsync(model, logger, mapper).ConfigureAwait(false),
                logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPost("load"), SwaggerOperation(Summary = "Lấy danh sách theo điều kiện")]
        public async Task<IActionResult> LoadAsync([FromBody] LichSuHienMauCriteria criteria)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await lichSuHienMauRepository.LoadAsync(criteria, logger, mapper).ConfigureAwait(false),
                logger).ConfigureAwait(false);

            await Task.Delay(Settings.DelayTime);

            return Ok(response);
        }
    }
}
