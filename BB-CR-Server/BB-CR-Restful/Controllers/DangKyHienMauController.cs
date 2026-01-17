using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BB.CR.Rest.Controllers
{
    [Route("api/dang-ky-hien-mau"), ApiController, Authorize]
    public class DangKyHienMauController(IDangKyHienMauRepository _dangKyHienMauRepository,
        ILogger<DangKyHienMauController> _logger,
        IMapper _mapper,
        IHttpContextAccessor _httpContextAccessor) : BaseController(_httpContextAccessor)
    {
        private readonly IDangKyHienMauRepository dangKyHienMauRepository = _dangKyHienMauRepository;
        private readonly IMapper mapper = _mapper;
        private readonly ILogger<DangKyHienMauController> logger = _logger;

        [HttpPost("load"), SwaggerOperation(Summary = "Lấy danh sách DangKyHienMau")]
        public async Task<IActionResult> LoadAsync([FromBody] DangKyHienMauCriteria criteria)
        {
            // Tracing data input
            logger.Log(LogLevel.Information, BaseVariable.ERROR_MESSAGE, JsonConvert.SerializeObject(criteria));

            var response = await BaseHandler.ExecuteAsync(
                async () => await dangKyHienMauRepository.LoadAsync(criteria, logger, mapper, this.GetIdentityCard(), criteria.IsLoadAll ?? false).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("get/{id}"), SwaggerOperation(Summary = "Lấy thông tin DangKyHienMau")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dangKyHienMauRepository.GetAsync(id, logger, mapper).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost("create"), SwaggerOperation(Summary = "Tạo thông tin DangKyHienMau")]
        public async Task<IActionResult> CreateAsync([FromBody] DangKyHienMauView model)
        {
            logger.Log(LogLevel.Information, BaseVariable.ERROR_MESSAGE, JsonConvert.SerializeObject(model));

            // check câu hỏi đã trả lời đủ chưa
            var response = new ReturnResponse<DangKyHienMauView>();
            if (model.SurveyQuestions is null || model.SurveyQuestions.Count == 0 || model.SurveyQuestions.Any(i => i.MaleSkip != true && !i.Yes.HasValue && !i.No.HasValue))
            {
                response.Error(HttpStatusCode.Conflict, "Vui lòng trả lời đầy đủ các câu hỏi!");
                return Ok(response);
            }

            // check tuổi được đăng ký hiến máu
            var (tuoi, soNgayConDu) = TinhTuoi(model.NgaySinh, model.NamSinh);
            var tuoi60vaHonNgay = tuoi == 60 && soNgayConDu > 0;
            if (tuoi < 18 || tuoi > 60 || tuoi60vaHonNgay)
            {
                string messTuoi = tuoi60vaHonNgay ? $"{tuoi.ToString()} tuổi {soNgayConDu} ngày" : tuoi.ToString();
                response.Error(HttpStatusCode.Conflict, $"Tuổi của bạn ({messTuoi}) không phù hợp để hiến máu (phải từ 18 đến 60 tuổi)");
                return Ok(response);
            }

            var _model = model.Adapt<DangKyHienMau>();
            var _modelChildrenDetail = model.SurveyQuestions.Adapt<List<TraLoiCauHoiChiTiet>>();
            if (_modelChildrenDetail is not null
                && _modelChildrenDetail.Count > 0)
            {
                _model.TraLoiCauHoi = new TraLoiCauHoi
                {
                    Ngay = DateTime.Now,
                    TraLoiCauHoiChiTiets = _modelChildrenDetail
                };
            }

            response = await BaseHandler.ExecuteAsync(
                async () => await dangKyHienMauRepository.CreateAsync(_model, logger, mapper, this.GetUserCode(), this.GetIdentityCard()).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPut("update/{id}"), SwaggerOperation(Summary = "Cập nhật thông tin DangKyHienMau")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] DangKyHienMau model)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dangKyHienMauRepository.UpdateAsync(id, model, logger, mapper).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("donor-signature/{id}"), SwaggerOperation(Summary = "Lấy chữ ký tay của người hiến (nếu có)")]
        public async Task<IActionResult> GetDonorSignatureAsync(long id, [FromQuery] bool includeImage = false)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dangKyHienMauRepository
                    .GetDonorSignatureAsync(id, includeImage, logger, mapper, this.GetIdentityCard())
                    .ConfigureAwait(false),
                logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost("donor-signature/{id}"), SwaggerOperation(Summary = "Lưu chữ ký tay của người hiến (Base64 PNG)")]
        public async Task<IActionResult> SaveDonorSignatureAsync(long id, [FromBody] DonorSignatureSaveRequest request)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await dangKyHienMauRepository
                    .SaveDonorSignatureAsync(id, request, logger, mapper, this.GetIdentityCard(), this.GetDevice())
                    .ConfigureAwait(false),
                logger).ConfigureAwait(false);

            return Ok(response);
        }

        /// <summary>
        ///  Code từ chat GPT =))))
        /// </summary>
        /// <param name="ngaySinh"></param>
        /// <param name="namSinh"></param>
        /// <returns></returns>
        private static (int tuoi, int soNgayConDu) TinhTuoi(DateTime? ngaySinh, int? namSinh)
        {
            DateTime ngayHienTai = DateTime.Today;

            if (ngaySinh.HasValue)
            {
                int tuoi = ngayHienTai.Year - ngaySinh.Value.Year;

                // Xác định ngày sinh nhật gần nhất
                DateTime sinhNhatGanNhat = new DateTime(ngayHienTai.Year, ngaySinh.Value.Month, ngaySinh.Value.Day);

                // Nếu sinh nhật chưa đến trong năm nay, giảm tuổi
                if (ngayHienTai < sinhNhatGanNhat)
                {
                    tuoi--;
                    sinhNhatGanNhat = sinhNhatGanNhat.AddYears(-1); // Lấy sinh nhật năm trước
                }

                // Tính số ngày còn dư từ sinh nhật gần nhất đến hiện tại
                int soNgayConDu = (ngayHienTai - sinhNhatGanNhat).Days;
                return (tuoi, soNgayConDu);
            }
            else if (namSinh.HasValue)
            {
                int tuoi = ngayHienTai.Year - namSinh.Value;
                return (tuoi, -1); // Không thể tính số ngày nếu không có ngày sinh
            }

            return (0, -1); // Trả về giá trị mặc định nếu cả hai tham số đều null
        }

    }
}
