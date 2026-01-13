using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dm-xa"), ApiController, Authorize]
    public class WardController(IHttpContextAccessor _httpContextAccessor
        , IDMXaRepository _dMXaRepository
        , ILogger<WardController> _logger) : BaseController(_httpContextAccessor)
    {
        private readonly IDMXaRepository dMXaRepository = _dMXaRepository;
        private readonly ILogger<WardController> logger = _logger;

        [HttpPost("pull-local")]
        public async Task<IActionResult> UpdateAsync([FromBody] List<DMXaView> views)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dMXaRepository.UpdateAsync(views, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
