using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class TraLoiCauHoiChiTietController(IHttpContextAccessor _httpContextAccessor
        , ITraLoiCauHoiChiTietRepository _traLoiCauHoiChiTietRepository
        , ILogger<TraLoiCauHoiChiTietController> _logger) : BaseController(_httpContextAccessor)
    {
        private readonly ITraLoiCauHoiChiTietRepository traLoiCauHoiChiTietRepository = _traLoiCauHoiChiTietRepository;
        private readonly ILogger<TraLoiCauHoiChiTietController> logger = _logger;

        [HttpGet("get/{id}"), SwaggerOperation(Summary = "Lấy chi tiết câu trả lời bằng id")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await traLoiCauHoiChiTietRepository.GetAsync(id, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
