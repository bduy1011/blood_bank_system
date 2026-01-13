using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
#if DEBUG
    [Route("api/lich-su-hien-mau"), ApiController, Authorize]
    public class LichSuLayMauController(ILichSuHienMauRepository _lichSuHienMauRepository, ILogger<LichSuLayMauController> _logger) : ControllerBase
    {
        private readonly ILichSuHienMauRepository lichSuHienMauRepository = _lichSuHienMauRepository;
        private readonly ILogger<LichSuLayMauController> logger = _logger;

        [HttpPost("load"), SwaggerOperation(Summary = "Get theo model search LichSuHienMau")]
        public async Task<IActionResult> LoadAsync([FromBody] LichSuHienMauCriteria criteria)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await lichSuHienMauRepository.LoadAsync(criteria, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }
    }
#endif 
}
