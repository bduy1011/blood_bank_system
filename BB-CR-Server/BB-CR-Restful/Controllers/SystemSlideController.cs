using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/system-slide"), ApiController, Authorize]
    public class SystemSlideController(
        ISystemSlideRepository _SystemSlideRepository
        , ILogger<SystemSlideController> _logger) : ControllerBase
    {
        private readonly ISystemSlideRepository SystemSlideRepository = _SystemSlideRepository;
        private readonly ILogger<SystemSlideController> logger = _logger;


        [HttpPost("load"), SwaggerOperation(Summary = "Lấy danh sách slide")]
        public async Task<IActionResult> LoadAsync([FromBody] SystemSlideCriteria criteria)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await SystemSlideRepository.LoadAsync(criteria, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
