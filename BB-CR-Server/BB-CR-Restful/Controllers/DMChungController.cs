using BB.CR.Providers.Bases;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dm-chung"), ApiController, Authorize]
    public class DMChungController(IDistributedCache _distributedCache,
        ILogger<DMChungController> _logger,
        IDMChungRepository _dmChungRepository,
        IMapper _mapper,
        IDMXaRepository _dmXaRepository,
        IDMHuyenRepository _dmHuyenRepository,
        IDMTinhRepository _dmTinhRepository,
        ISurveyQuestionRepository _surveyQuestionRepository) : ControllerBase
    {
        private readonly IDistributedCache distributedCache = _distributedCache;
        private readonly ILogger<DMChungController> logger = _logger;
        private readonly IDMChungRepository dmChungRepository = _dmChungRepository;
        private readonly IMapper mapper = _mapper;
        private readonly IDMXaRepository dmXaRepository = _dmXaRepository;
        private readonly IDMHuyenRepository dmHuyenRepository = _dmHuyenRepository;
        private readonly IDMTinhRepository dmTinhRepository = _dmTinhRepository;

        private readonly ISurveyQuestionRepository surveyQuestionRepository = _surveyQuestionRepository;

#if DEBUG
        [HttpGet("load"), SwaggerOperation(Summary = "Load all danh mục tỉnh, huyện, xã (Cached)")]
        public async Task<IActionResult> LoadAsync()
        {
            ReturnResponse<DMChungView> response = await BaseHandler.CacheAsync(
                async () => await dmChungRepository.LoadAsync(logger, mapper).ConfigureAwait(false)
                , logger
                , distributedCache
                , KeyCached.CACHED_COMMON).ConfigureAwait(false);

            return Ok(response);
        }
#endif

        [HttpGet("load-nhom-mau"), SwaggerOperation(Summary = "Danh sách nhóm máu")]
        public async Task<IActionResult> LoadBloodGroupAsync()
        {
            var response = await BaseHandler.GetEnumAsync<NhomMau>(
                logger
                , distributedCache
                , KeyCached.CACHED_BLOOD_GROUP).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("load-tinh"), SwaggerOperation(Summary = "Danh sách tỉnh / thành phố")]
        public async Task<IActionResult> LoadProvinceAsync()
        {
            var response = await BaseHandler.CacheAsync(
                async () => await dmTinhRepository.LoadAsync(logger, mapper).ConfigureAwait(false)
                , logger
                , distributedCache
                , KeyCached.CACHED_PROVINCE).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("load-quan"), SwaggerOperation(Summary = "Danh sách quận / huyện")]
        public async Task<IActionResult> LoadDistrictAsync()
        {
            var response = await BaseHandler.CacheAsync(
                async () => await dmHuyenRepository.LoadAsync(logger, mapper).ConfigureAwait(false)
                , logger
                , distributedCache
                , KeyCached.CACHED_DISTRICT).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("load-xa"), SwaggerOperation(Summary = "Danh sách phường / xã")]
        public async Task<IActionResult> LoadWardAsync()
        {
            var response = await BaseHandler.CacheAsync(
                async () => await dmXaRepository.LoadAsync(logger, mapper).ConfigureAwait(false)
                , logger
                , distributedCache
                , KeyCached.CACHED_WARD).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("load-bang-cau-hoi"), SwaggerOperation(Summary = "Danh sách câu hỏi hiến máu")]
        public async Task<IActionResult> LoadSurveyQuestion()
        {
            var response = await BaseHandler.CacheAsync(
                async () => await surveyQuestionRepository.LoadAsync(logger, mapper).ConfigureAwait(false)
                , logger
                , distributedCache
                , KeyCached.CACHED_SURVEY_QUESTION).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
