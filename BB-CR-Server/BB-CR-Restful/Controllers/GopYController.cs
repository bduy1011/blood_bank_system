using BB.CR.Models;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/gop-y"), ApiController, Authorize]
    public class GopYController(IGopYRepository _gopYRepository, ILogger<GopYController> _logger, IMapper _mapper, IHttpContextAccessor _httpContextAccessor) : BaseController(_httpContextAccessor)
    {
        private readonly ILogger<GopYController> logger = _logger;
        private readonly IGopYRepository gopYRepository = _gopYRepository;
        private readonly IMapper mapper = _mapper;

        [HttpPost("load"), SwaggerOperation(Summary = "Load dữ liệu góp ý với điều kiện criteria (GopY)")]
        public async Task<IActionResult> LoadAsync([FromBody] GopYCriteria criteria)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await gopYRepository.LoadAsync(criteria, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("get/{id}"), SwaggerOperation(Summary = "Get dữ liệu góp ý với id (GopY)")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await gopYRepository.GetAsync(id, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPost("create"), SwaggerOperation(Summary = "Create dữ liệu góp ý với model (GopY)")]
        public async Task<IActionResult> CreateAsync([FromBody] GopY model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await gopYRepository.CreateAsync(model, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut("update/{id}"), SwaggerOperation(Summary = "Update dữ liệu góp ý với model và id (GopY)")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] GopY model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await gopYRepository.UpdateAsync(id, model, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpDelete("delete/{id}"), SwaggerOperation(Summary = "Delete dữ liệu góp ý với id (GopY)")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await gopYRepository.DeleteAsync(id, logger, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
