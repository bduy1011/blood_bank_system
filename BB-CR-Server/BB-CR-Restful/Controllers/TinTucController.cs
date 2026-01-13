using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/tin-tuc"), ApiController, Authorize]
    public class TinTucController(IHttpContextAccessor _httpContextAccessor
        , ITinTucRepository _tinTucRepository
        , ILogger<TinTucController> _logger) : BaseController(_httpContextAccessor)
    {
        private readonly ITinTucRepository tinTucRepository = _tinTucRepository;
        private readonly ILogger<TinTucController> logger = _logger;

        [HttpGet("load"), SwaggerOperation(Summary = "Hiển thị tin tức (10 bản tin gần nhất của năm)")]
        public async Task<IActionResult> LoadAsync()
        {
            var response = await BaseHandler.ExecuteAsync(async () => await tinTucRepository.LoadAsync(logger).ConfigureAwait(false)
            , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("get/{id}"), SwaggerOperation(Summary = "Lấy tin tức theo id")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await BaseHandler.ExecuteAsync(async () => await tinTucRepository.GetAsync(id, logger).ConfigureAwait(false)
            , logger).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
