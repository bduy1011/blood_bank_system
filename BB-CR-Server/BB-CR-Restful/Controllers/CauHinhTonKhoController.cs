using BB.CR.Models;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/cau-hinh-ton-kho")]
    [ApiController]
    public class CauHinhTonKhoController(ICauHinhTonKhoRepository repoCauHinhTonKho
        , ILogger<CauHinhTonKhoController> logger
        , IMapper mapper
        , IHttpContextAccessor contextAccessor) : BaseController(contextAccessor)
    {
        private readonly ICauHinhTonKhoRepository repoCauHinhTonKho = repoCauHinhTonKho;
        private readonly IMapper mapper = mapper;
        private readonly ILogger<CauHinhTonKhoController> logger = logger;

        [HttpGet("{id}"), SwaggerOperation(Summary = "Lay thong tin cau hinh ton kho theo {id}")]
        public async Task<IActionResult> Get(long id)
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await repoCauHinhTonKho.GetAsync(id, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false), logger)
                .ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPost, SwaggerOperation(Summary = "Tao thong tin cau hinh ton kho")]
        public async Task<IActionResult> Post([FromBody] CauHinhTonKho model)
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await repoCauHinhTonKho.CreateAsync(model, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false), logger)
                .ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut("{id}"), SwaggerOperation(Summary = "Cap nhat thong tin cau hinh ton kho theo {id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CauHinhTonKho model)
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await repoCauHinhTonKho.UpdateAsync(id, model, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false), logger)
                .ConfigureAwait(false);
            return Ok(response);
        }

        [HttpDelete("{id}"), SwaggerOperation(Summary = "Xoa thong tin cau hinh ton kho theo {id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await repoCauHinhTonKho.DeleteAsync(id, logger, this.GetIdentityCard()!).ConfigureAwait(false), logger)
                .ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPatch, SwaggerOperation(Summary = "Tim kiem thong tin cau hinh ton kho theo {id}")]
        public async Task<IActionResult> Load([FromBody] CauHinhTonKhoCriteria criteria)
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await repoCauHinhTonKho.LoadAsync(criteria, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false), logger)
                .ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("init"), SwaggerOperation(Summary = "Khoi tao thong tin cau hinh ton kho theo ngay")]
        public async Task<IActionResult> Init([FromQuery] DateTime ngay) 
        {
            var response = await BaseHandler
                .ExecuteAsync(async () => await repoCauHinhTonKho.InitAsync(ngay, logger, mapper).ConfigureAwait(false), logger)
                .ConfigureAwait(false);
            return Ok(response);
        }
    }
}
