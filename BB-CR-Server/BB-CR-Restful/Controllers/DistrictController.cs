using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dm-huyen"), ApiController, Authorize]
    public class DistrictController(IHttpContextAccessor _httpContextAccessor
        , IDMHuyenRepository _dmHuyenRepository
        , ILogger<DistrictController> _logger) : BaseController(_httpContextAccessor)
    {
        private readonly IDMHuyenRepository dmHuyenRepository = _dmHuyenRepository;
        private readonly ILogger<DistrictController> logger = _logger;

        [HttpPost("pull-local")]
        public async Task<IActionResult> UpdateAsync([FromBody] IList<DMHuyenView> views)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dmHuyenRepository.UpdateAsync(views, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
