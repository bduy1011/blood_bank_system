using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    internal class DangKyHienMauUseCase
    {
        public static async Task<ReturnResponse<DangKyHienMauView>> GetAsync(long id, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<DangKyHienMauView>();
            var data = await context.DangKyHienMau.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                var view = mapper.Map<DangKyHienMauView>(data);

                // Lấy danh sách câu hỏi
                var answerQuestion = await context.TraLoiCauHoi.AsNoTracking()
                    .Include(i => i.TraLoiCauHoiChiTiets)
                    .FirstOrDefaultAsync(i => i.Id == data.TraLoiCauHoiId).ConfigureAwait(false);

                if (answerQuestion is not null
                    && answerQuestion.TraLoiCauHoiChiTiets?.Count > 0)
                {
                    var surveyQuestion = await context.SurveyQuestion.AsNoTracking().ToListAsync().ConfigureAwait(false);
                    var surveyQuestions = new List<SurveyQuestionView>();
                    answerQuestion.TraLoiCauHoiChiTiets.ForEach(i =>
                    {
                        var _ = surveyQuestion.FirstOrDefault(o => o.Id == i.SurveyQuestionId);
                        var survey = new SurveyQuestionView
                        {
                            Id = i.SurveyQuestionId.GetValueOrDefault(),
                            Content = _?.Content ?? string.Empty,
                            Yes = i.YesAnswer,
                            No = i.NoAnswer,
                            Attribute = _?.Attribute,
                            Notes = i.GhiChu,
                            OnDate = i.OnDate
                        };
                        surveyQuestions.Add(survey);
                    });

                    view.SurveyQuestions = surveyQuestions;
                }

                response.Success(view, CommonResources.Ok);
            }
            return response;
        }

        public static async Task<ReturnResponse<DangKyHienMauView>> CreateAsync(DangKyHienMau model
            , BloodBankContext context
            , IMapper mapper
            , string? userCode
            , string? identityCard)
        {
            var response = new ReturnResponse<DangKyHienMauView>();
            var data = await context.DangKyHienMau
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.CMND == identityCard && i.DotLayMauId == model.DotLayMauId && i.TinhTrang != TinhTrangDangKyHienMau.Huy)
                .ConfigureAwait(false); // Người dùng đã đăng ký đợt lấy máu

            var configure = await context.SystemConfig.AsNoTracking().FirstOrDefaultAsync(i => i.Key == BaseVariable.SO_NGAY_CHO_HIEN_MAU_LAI).ConfigureAwait(false);

            if (data is not null)
            {
                // Duplicate Ngay register blood bank
                if (data.Ngay == model.Ngay)
                {
                    response.Error(HttpStatusCode.Conflict, string.Format(BzLogicResource.DaDangKyHienMau, data.CMND, data.SoDT, data.Ngay.ToString(DateFormatter.DDMMYYYY_VN)));
                    return response;
                }
            }
            
            if (data is null || data.Ngay != model.Ngay)
            {
                // Kiểm tra số lượng đợt lấy máu
                var slDotLayMauDangKy = await context.DangKyHienMau
                    .AsNoTracking()
                    .Where(i => i.DotLayMauId == model.DotLayMauId && i.TinhTrang != TinhTrangDangKyHienMau.Huy)
                    .Select(i => i.Id)
                    .CountAsync()
                    .ConfigureAwait(false);
                var slDuKienDotLayMau = await context.DotLayMau
                    .AsNoTracking()
                    .Where(i => i.DotLayMauId == model.DotLayMauId)
                    .Select(i => i.SoLuongDuKien)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                // Số lượng đăng ký hiện có  + 1 (đang nhấn nút lưu đăng ký)
                if ((slDotLayMauDangKy + 1) > slDuKienDotLayMau)
                    response.Error(HttpStatusCode.Conflict, BzLogicResource.VuotSoLuongDangKyDotLayMau);
                else
                {
                    data = model.Adapt<DangKyHienMau>();
                    data.HoVaTen = model.HoVaTen.ToUpper();
                    data.CreatedBy = identityCard;
                    data.CreatedOn = DateTime.Now;

                    // Arrange data DMNguoiHienMau by NguoiHienMauId get first
                    var dmNguoiHienMau = await context.DMNguoiHienMau
                        .AsNoTracking()
                        .Where(i => i.CMND == identityCard)
                        .OrderByDescending(i => i.NguoiHienMauId)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);
                    data.NguoiHienMauId = dmNguoiHienMau?.NguoiHienMauId ?? 0;

                    await context.DangKyHienMau.AddAsync(data).ConfigureAwait(false);
                    int count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                        response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                    else
                    {
                        data = await context.DangKyHienMau.AsNoTracking()
                            .FirstOrDefaultAsync(i => i.Id == data.Id).ConfigureAwait(false);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        var include = await context.TraLoiCauHoi.AsNoTracking()
                            .Include(i => i.TraLoiCauHoiChiTiets)
                            .FirstOrDefaultAsync(i => i.Id == data.TraLoiCauHoiId).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8604 // Possible null reference argument.
                        var view = mapper.Map<DangKyHienMauView>(data);
                        // Lấy danh sách câu hỏi
                        var answerQuestion = await context.TraLoiCauHoi.AsNoTracking()
                            .Include(i => i.TraLoiCauHoiChiTiets)
                            .FirstOrDefaultAsync(i => i.Id == data.TraLoiCauHoiId).ConfigureAwait(false);

                        if (answerQuestion is not null
                            && answerQuestion.TraLoiCauHoiChiTiets?.Count > 0)
                        {
                            var surveyQuestion = await context.SurveyQuestion.AsNoTracking().ToListAsync().ConfigureAwait(false);
                            var surveyQuestions = new List<SurveyQuestionView>();
                            answerQuestion.TraLoiCauHoiChiTiets.ForEach(i =>
                            {
                                var _ = surveyQuestion.FirstOrDefault(o => o.Id == i.SurveyQuestionId);
                                var survey = new SurveyQuestionView
                                {
                                    Id = i.SurveyQuestionId.GetValueOrDefault(),
                                    Content = _?.Content ?? string.Empty,
                                    Yes = i.YesAnswer,
                                    No = i.NoAnswer,
                                    Attribute = _?.Attribute,
                                    Notes = i.GhiChu,
                                    OnDate = i.OnDate
                                };
                                surveyQuestions.Add(survey);
                            });

                            view.SurveyQuestions = surveyQuestions;
                        }
                        response.Success(view, CommonResources.Ok);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }
            return response;
        }

        public static async Task<ReturnResponse<DangKyHienMauView>> UpdateAsync(long id, DangKyHienMau model, BloodBankContext context, IMapper mapper)
        {
            var response = new ReturnResponse<DangKyHienMauView>();
            var data = await context.DangKyHienMau.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                data = model.Adapt<DangKyHienMau>();
                context.DangKyHienMau.Update(data);
                int count = await context.SaveChangesAsync();
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                {
                    var view = mapper.Map<DangKyHienMauView>(data);
                    response.Success(view, CommonResources.Ok);
                }
            }
            return response;
        }

        public static async Task<ReturnResponse<List<DangKyHienMauView>>> LoadAsync(DangKyHienMauCriteria criteria
            , BloodBankContext context
            , IMapper mapper
            , string? userCode
            , bool isLocal = false)
        {
            var response = new ReturnResponse<List<DangKyHienMauView>>();
            var queries = context.DangKyHienMau.AsNoTracking();

            if (!isLocal)
                queries = queries.Where(i => i.CMND == userCode);

            if (!string.IsNullOrEmpty(criteria.CMND)) queries = queries.Where(i => i.CMND != null && criteria.CMND.Contains(i.CMND));
            if (criteria.Ids?.Count > 0) queries = queries.Where(i => criteria.Ids.Contains(i.Id));
            if (criteria.NgayTu is not null) queries = queries.Where(i => criteria.NgayTu.Value.Date <= i.Ngay.Date);
            if (criteria.NgayDen is not null) queries = queries.Where(i => criteria.NgayDen.Value.Date >= i.Ngay.Date);
            if (criteria.NguoiHienMauIds?.Count > 0) queries = queries.Where(i => criteria.NguoiHienMauIds.Contains(i.NguoiHienMauId.GetValueOrDefault()));
            if (criteria.TinhTrangs?.Count > 0) queries = queries.Where(i => criteria.TinhTrangs.Contains(i.TinhTrang));
            if (!string.IsNullOrEmpty(criteria.HoTen)) queries = queries.Where(i => EF.Functions.Like(i.HoVaTen, $"%{criteria.HoTen}%"));
            if (criteria.NamSinh is not null) queries = queries.Where(i => criteria.NamSinh == i.NamSinh);
            if (!string.IsNullOrEmpty(criteria.Xa)) queries = queries.Where(i => criteria.Xa == i.MaXa);
            if (!string.IsNullOrEmpty(criteria.Huyen)) queries = queries.Where(i => criteria.Huyen == i.MaHuyen);
            if (!string.IsNullOrEmpty(criteria.Tinh)) queries = queries.Where(i => criteria.Tinh == i.MaTinh);

            if (criteria.DotLayMauIds?.Count > 0) queries = queries.Where(i => criteria.DotLayMauIds.Contains(i.DotLayMauId));

            queries = queries.OrderByDescending(i => i.Ngay).ApplyPagination(criteria.PageIndex, criteria.PageSize);

            var data = await queries.ToListAsync().ConfigureAwait(false) ?? [];
            var view = mapper.Map<List<DangKyHienMauView>>(data);

            response.Success(view, CommonResources.Ok);
            return response;
        }
    }
}
