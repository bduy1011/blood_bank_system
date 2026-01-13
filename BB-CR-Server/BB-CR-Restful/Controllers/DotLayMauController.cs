using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dot-lay-mau"), ApiController, Authorize]
    public class DotLayMauController(IDotLayMauRepository dotLayMauRepository
        , ILogger<DotLayMauController> logger
        , IHttpContextAccessor httpContextAccessor) : BaseController(httpContextAccessor)
    {
        private readonly IDotLayMauRepository dotLayMauRepository = dotLayMauRepository;
        private readonly ILogger<DotLayMauController> logger = logger;

        [HttpPost("load"), SwaggerOperation(Summary = "Lấy danh sách đợt lấy máu")]
        public async Task<IActionResult> LoadByCriteria([FromBody] DotLayMauCriteria criteria)
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await dotLayMauRepository.LoadByCriteria(criteria, logger).ConfigureAwait(false), logger)
                .ConfigureAwait(false);

            await Task.Delay(Settings.DelayTime);

            return Ok(response);
        }

        [HttpPut("update/{dotLayMauId}/{tinhTrang}"), SwaggerOperation(Summary = "Cập nhật trạng thái đợt lấy máu")]
        public async Task<IActionResult> UpdateByIdAndTinhTrang(long dotLayMauId, TinhTrangDotLayMau tinhTrang)
        {
            ReturnResponse<Models.DotLayMau> response = new();

            try
            {
                response = await dotLayMauRepository.UpdateByIdAndTinhTrang(dotLayMauId, tinhTrang, logger).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                logger.Log(LogLevel.Error, "{error}", ex.InnerMessage());
                response.Error(System.Net.HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return Ok(response);
        }

        [HttpPut("create-or-update"), SwaggerOperation(Summary = "Cập nhật danh sách từ local")]
        public async Task<IActionResult> UpdateLocalAsync([FromBody] List<DotLayMau> model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dotLayMauRepository.UpdateLocalAsync(model, logger).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
