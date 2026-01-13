using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BB.CR.Rest.Controllers
{
    [Route("api/files-manage"), ApiController, Authorize]
    public class UploadFilesController(IHttpContextAccessor _httpContextAccessor, ILichSuHienMauRepository lichSuHienMauRepository, ILogger<UploadFilesController> logger) : BaseController(_httpContextAccessor)
    {
        private readonly ILichSuHienMauRepository _lichSuHienMauRepository = lichSuHienMauRepository;
        private readonly ILogger<UploadFilesController> _logger = logger;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            await Task.Delay(1000);
            //var root = Path.Combine(Settings.PathTemp, "temp");
            return Ok();
        }

        [HttpGet("get-letter/{id}/{type}"), SwaggerOperation(Summary = "Lay thu moi dang html de hien thi len device")]
        public async Task<IActionResult> GetAsync(long id, int type)
        {
            string fileContent = string.Empty;

            if (type == 1) // Duong tinh
                fileContent = await System.IO.File.ReadAllTextAsync("Templates\\GiayMoiDuongTinh.html").ConfigureAwait(false);
            else if (type == 2) // Chu xac dinh
                fileContent = await System.IO.File.ReadAllTextAsync("Templates\\GiayMoiChuaXacDinh.html").ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                var letter = await BaseHandler.ExecuteAsync(async () => await _lichSuHienMauRepository.GetLetterAsync(id, _logger).ConfigureAwait(false)
                , _logger).ConfigureAwait(false);

                if (letter is not null)
                {
                    var currentDate = DateTime.Now;
                    var signedDate = $"Ngày {currentDate.Day} tháng {currentDate.Month} năm {currentDate.Year}";

                    fileContent = fileContent.Replace("[HoTen]", letter.HoTen).Replace("[NgaySinh]", letter.NgaySinh).Replace("[DiaChi]", letter.DiaChi).Replace("[NgayHienMau]", letter.NgayHienMau).Replace("[MaTuiMau]", letter.MaTuiMau).Replace("[Parameters.prDate]", signedDate);
                    if (type == 2)
                        fileContent = fileContent.Replace("[ThoiGianGapMat]", $"từ ngày {letter.ThoiGianGapMat}");
                }
            }

            return base.Content(fileContent.Replace(" ws4", string.Empty), "text/html");
        }
    }
}
