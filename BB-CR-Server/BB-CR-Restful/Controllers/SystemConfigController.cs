using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/system-config"), ApiController, Authorize]
    public class SystemConfigController(
        ISystemConfigRepository _SystemConfigRepository
        , ILogger<SystemConfigController> _logger) : ControllerBase
    {
        private readonly ISystemConfigRepository SystemConfigRepository = _SystemConfigRepository;
        private readonly ILogger<SystemConfigController> logger = _logger;


        [HttpPost("load"), SwaggerOperation(Summary = "Lấy danh sách cấu hình hệ thống")]
        public async Task<IActionResult> LoadAsync([FromBody] SystemConfigCriteria criteria)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await SystemConfigRepository.LoadAsync(criteria, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
