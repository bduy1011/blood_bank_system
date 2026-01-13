using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories;
using BB.CR.Rest.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BB.CR.Rest.Controllers
{
    [Route("api/giao-dich"), ApiController, Authorize]
    public class GiaoDichController(IHttpContextAccessor httpContextAccessor, IGiaoDichRepository _transactionRepository, ILogger<GiaoDichController> _logger, IMapper _mapper) : BaseController(httpContextAccessor)
    {
        private readonly IGiaoDichRepository transactionRepository = _transactionRepository;
        private readonly ILogger<GiaoDichController> logger = _logger;
        private readonly IMapper mapper = _mapper;

        [HttpPost("create"), SwaggerOperation(Summary = "Tạo mới giao dịch mua máu")]
        public async Task<IActionResult> CreateAsync([FromBody] GiaoDich model)
        {
            var response = await BaseHandler.ExecuteAsync(async () => await transactionRepository.CreateAsync(model, logger, mapper, idCardNo: this.GetIdentityCard()!).ConfigureAwait(false)
            , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("get"), SwaggerOperation(Summary = "Lấy thông tin giao dịch với {id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await BaseHandler.ExecuteAsync(async () => await transactionRepository.GetAsync(id, logger, mapper, idCardNo: this.GetIdentityCard()!).ConfigureAwait(false)
            , logger).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("init/{loaiPhieu}"), SwaggerOperation(Summary = "Khởi tạo dữ liệu khi đăng ký nhượng máu")]
        public async Task<IActionResult> GetAsync(LoaiPhieu loaiPhieu = LoaiPhieu.PhieuNhuongMau)
        {
            var response = new ReturnResponse<GiaoDichView>();

            var giaoDich = new GiaoDich
            {
                LoaiPhieu = loaiPhieu,
                GiaoDichChiTiets = [],
                IsLocked = false,
                TinhTrang = TinhTrangGiaoDich.ChoXacNhan,
                Ngay = DateTime.Now
            };

            foreach (var item in EnumExtension.ToValueList<LoaiSanPham>())
            {
                var giaoDichChiTiet = new GiaoDichChiTiet
                {
                    LoaiSanPham = item,
                    GiaoDichCons = []
                };

                foreach (var itemdetail in EnumExtension.ToValueList<NhomMau>())
                {
                    var giaoDichCon = new GiaoDichCon
                    {
                        MaNhomMau = itemdetail,
                        SoLuong = 0,
                        //DaDuyet = false
                    };

                    giaoDichChiTiet.GiaoDichCons.Add(giaoDichCon);
                }

                giaoDich.GiaoDichChiTiets.Add(giaoDichChiTiet);
            }

            await Task.Delay(1000);

            var view = mapper.Map<GiaoDichView>(giaoDich);

            response.Success(view, CommonResources.Ok);
            return Ok(response);
        }

        [HttpPost("load"), SwaggerOperation(Summary = "Tim kiem voi dieu kien")]
        public async Task<IActionResult> LoadAsync([FromBody] GiaoDichCriteria criteria)
        {
            var response =
                await BaseHandler.ExecuteAsync(async () => await transactionRepository.LoadAsync(criteria, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut("update/{id}/{isApprove}"), SwaggerOperation(Summary = "Cập nhật thay đổi giao dịch máu")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] GiaoDich model, bool isApprove)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await transactionRepository.UpdateAsync(id, model, logger, mapper, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            if (isApprove)
            {
                if (response.Status == HttpStatusCode.OK
                    && (response.Data is not null && response.Data.GiaoDichId > 0))
                {
                    response = await BaseHandler.ExecuteAsync(
                        async () => await transactionRepository.ApproveAsync(id, TinhTrangGiaoDich.ChoXacNhan, logger, this.GetIdentityCard()!, mapper).ConfigureAwait(false)
                        , logger).ConfigureAwait(false);
                }
            }

            return Ok(response);
        }

        [HttpDelete("delete/{id}"), SwaggerOperation(Summary = "Xoá giao dịch máu")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var response = await BaseHandler.ExecuteAsync(
                async () => await transactionRepository.DeleteAsync(id, logger, this.GetIdentityCard()!).ConfigureAwait(false)
                , logger).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
