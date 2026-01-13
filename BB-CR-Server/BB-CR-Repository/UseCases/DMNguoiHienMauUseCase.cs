using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Extensions;
using BB.CR.Views;
using BB.CR.Views.Authenticate;
using BB.CR.Views.Criterias;
using BB.CR.Views.Otp;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    public class DMNguoiHienMauUseCase
    {
        public static async Task<ReturnResponse<List<DMNguoiHienMau>>> LoadAsync(DMNguoiHienMauCriteria criteria, BloodBankContext context)
        {
            var response = new ReturnResponse<List<DMNguoiHienMau>>();
            var queryable = context.DMNguoiHienMau.AsNoTracking();

            if (criteria.NguoiHienMauIds?.Count > 0) // Danh sách người hiến máu (id)
                queryable = queryable.Where(i => i.NguoiHienMauId.HasValue && criteria.NguoiHienMauIds.Contains(i.NguoiHienMauId.Value));

            if (!string.IsNullOrEmpty(criteria.HoTen)) // Họ tên
                queryable = queryable.Where(i => EF.Functions.Like(i.HoVaTen, $"%{criteria.HoTen}%"));

#pragma warning disable CS8604 // Possible null reference argument.
            if (!string.IsNullOrEmpty(criteria.CMND)) // CMND
                queryable = queryable.Where(i => EF.Functions.Like(i.CMND, $"%{criteria.CMND}%"));

            if (!string.IsNullOrEmpty(criteria.Email)) // Email
                queryable = queryable.Where(i => EF.Functions.Like(i.Email, $"%{criteria.Email}%"));

            if (!string.IsNullOrEmpty(criteria.SoDT)) // Số điện thoại
                queryable = queryable.Where(i => EF.Functions.Like(i.SoDT, $"%{criteria.SoDT}%"));

            if (criteria.NamSinh is not null)
                queryable = queryable.Where(i => i.NamSinh == criteria.NamSinh.ToString());

            if (!string.IsNullOrEmpty(criteria.Xa))
                queryable = queryable.Where(i => i.MaXa == criteria.Xa);

            if (!string.IsNullOrEmpty(criteria.Huyen))
                queryable = queryable.Where(i => i.MaHuyen == criteria.Huyen);

            if (!string.IsNullOrEmpty(criteria.Tinh))
                queryable = queryable.Where(i => i.MaTinh == criteria.Tinh);

            /// Điều kiện lọc cho ngày cập nhật để chuyển dữ liệu về local
            if (criteria.UpdatedTime is not null)
                queryable = queryable.Where(i => i.UpdatedTime.HasValue && i.UpdatedTime.Value.Date == criteria.UpdatedTime.Value.Date);
#pragma warning restore CS8604 // Possible null reference argument.

            queryable = queryable.OrderBy(i => i.HoVaTenNoDiacritics).ApplyPagination(criteria.PageIndex, criteria.PageSize);

            if (!queryable.Any())
                response.Success([]
                , CommonResources.NotFound);
            else
                response.Success(await queryable.ToListAsync().ConfigureAwait(false)
                    , CommonResources.Ok);

            return response;
        }

        public static async Task<ReturnResponse<DMNguoiHienMau>> GetAsync(int id, BloodBankContext context)
        {
            ReturnResponse<DMNguoiHienMau> response = new();

            DMNguoiHienMau? data = await context.DMNguoiHienMau.AsNoTracking().FirstOrDefaultAsync(i => i.NguoiHienMauId == id).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
                response.Success(data, CommonResources.Ok);

            return response;
        }

        public static async Task<ReturnResponse<List<DMNguoiHienMau>>> UpdateAsync(List<DMNguoiHienMau> model, BloodBankContext context)
        {
            ReturnResponse<List<DMNguoiHienMau>> response = new();

            List<DMNguoiHienMau> data = await context.DMNguoiHienMau.AsNoTracking().Where(i => model.Select(o => o.NguoiHienMauId).Contains(i.NguoiHienMauId)).ToListAsync().ConfigureAwait(false);
            if (data is null || data.Count == 0)
            {
                model.ForEach(i => i.UpdatedTime = DateTime.Now);
                data = model;
                await context.DMNguoiHienMau.AddRangeAsync(data).ConfigureAwait(false);
            }
            else
            {
                data = model.Where(i => data.Select(o => o.NguoiHienMauId).Contains(i.NguoiHienMauId)).Adapt<List<DMNguoiHienMau>>();
                context.DMNguoiHienMau.UpdateRange(data);

                // Đối với dữ liệu mới.
                List<DMNguoiHienMau> unexistsData = model.Where(i => !data.Select(o => o.NguoiHienMauId).Contains(i.NguoiHienMauId)).ToList();
                if (unexistsData?.Count > 0)
                    await context.DMNguoiHienMau.AddRangeAsync(unexistsData).ConfigureAwait(false);
            }

            int count = await context.SaveChangesAsync().ConfigureAwait(false);
            if (count == 0)
                response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
            else
                response.Success(data, CommonResources.Ok);

            return response;
        }

        public static async Task<ReturnResponse<DMNguoiHienMau>> CreateAsync(DMNguoiHienMau model,
            BloodBankContext context)
        {
            ReturnResponse<DMNguoiHienMau> response = new();

            DMNguoiHienMau? data = await context.DMNguoiHienMau
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.CMND == model.CMND && i.SoDT == model.SoDT).ConfigureAwait(false);
            if (data is not null)
                response.Error(HttpStatusCode.Conflict, string.Format(BzLogicResource.DMNguoiHienMauExists, data.HoVaTen, data.CMND, data.SoDT));
            else
            {
                data = model.Adapt<DMNguoiHienMau>();
                await context.DMNguoiHienMau.AddAsync(data).ConfigureAwait(false);
                int count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(data, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<DMNguoiHienMau?>> GetAsync(string hoTen, int? namSinh, string cmnd, BloodBankContext context)
        {
            var response = new ReturnResponse<DMNguoiHienMau?>();

            var query = context.DMNguoiHienMau.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(hoTen))
                query = query.Where(i => i.HoVaTen.Contains(hoTen));
            if (namSinh is not null)
                query = query.Where(i => i.NamSinh == namSinh.ToString());
            if (!string.IsNullOrWhiteSpace(cmnd))
                query = query.Where(i => !string.IsNullOrWhiteSpace(i.CMND) && i.CMND.Contains(cmnd));

            if (query.Count() > 1)
                response.Error(HttpStatusCode.Conflict, BzLogicResource.ExistsInfoPeopleCategory);
            else
            {
                var data = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                response.Success(data, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<DMNguoiHienMau?>> UpdateAsync(SettingUserView view, BloodBankContext context)
        {
            var response = new ReturnResponse<DMNguoiHienMau?>();
            var query = context.DMNguoiHienMau.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(view.CMND))
                query = query.Where(i => i.CMND == view.CMND);
            if (!string.IsNullOrWhiteSpace(view.HoTen))
                query = query.Where(i => i.HoVaTen != null && EF.Functions.Like(i.HoVaTen, $"%{view.HoTen}%"));
            if (view.NamSinh is not null)
            {
                var strNamSinh = view.NamSinh.ToString();
                query = query.Where(i => i.NamSinh == strNamSinh);
            }
            if (!string.IsNullOrWhiteSpace(view.SoDT))
                query = query.Where(i => i.SoDT != null && EF.Functions.Like(i.SoDT, $"%{view.SoDT}%"));

            var list = await query.ToListAsync().ConfigureAwait(false);

            if (list?.Count == 0)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else if (list?.Count >= 2)
                response.Error(HttpStatusCode.Conflict, BzLogicResource.ExistsInfoPeopleCategory);
            else
            {
                var data = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                if (data is not null)
                {
                }
            }

            return response;
        }

        public static async Task<DMNguoiHienMau?> GetByClientIdAsync(string clientId, BloodBankContext context)
        {
            await Task.Delay(100);
            return null;
        }

        public static async Task<ReturnResponse<DMNguoiHienMau>> GetByIdCardAsync(string idCard,
            string? phoneNumber,
            BloodBankContext context)
        {
            var response = new ReturnResponse<DMNguoiHienMau>();
            var dmNguoiHienMau = await context.DMNguoiHienMau
                .AsNoTracking()
                .OrderByDescending(i => i.NguoiHienMauId)
                .FirstOrDefaultAsync(i => i.CMND != null && i.CMND.Trim() == idCard).ConfigureAwait(false); // Lấy thông tin gần hiến máu gần nhất để xử lý
            if (dmNguoiHienMau is null)
                response.Error(HttpStatusCode.NotFound, string.Format(BzLogicResource.DMNguoiHienMauNotFoundByIdCard, idCard));
            else
            {
                //// Chỉ là bước đệm để phục vụ cho login lấy được thông tin người dùng.
                //var nguoiHMSystemUser = await context.NguoiHMSystemUser
                //    .AsNoTracking()
                //    .FirstOrDefaultAsync(i => i.PhoneNumber == phoneNumber && i.DMNguoiHienMauId == dmNguoiHienMau.NguoiHienMauId).ConfigureAwait(false);
                //if (nguoiHMSystemUser is null)
                //{
                //    nguoiHMSystemUser = new NguoiHMSystemUser
                //    {
                //        PhoneNumber = phoneNumber,
                //        DMNguoiHienMauId = dmNguoiHienMau.NguoiHienMauId,
                //        CreatedBy = phoneNumber,
                //        CreatedOn = DateTime.Now,
                //    };
                //    await context.NguoiHMSystemUser.AddAsync(nguoiHMSystemUser).ConfigureAwait(false);
                //}

                //// Cập nhật cột Name cho SystemUser
                //var systemUser = await context.SystemUser
                //    .AsNoTracking()
                //    .FirstOrDefaultAsync(i => i.UserCode == phoneNumber).ConfigureAwait(false);
                //if (systemUser is not null
                //    && string.IsNullOrWhiteSpace(systemUser.Name))
                //{
                //    systemUser.Name = dmNguoiHienMau.HoVaTen;
                //    context.SystemUser.Update(systemUser);
                //}

                //await context.SaveChangesAsync().ConfigureAwait(false);

                response.Success(dmNguoiHienMau, CommonResources.Ok);
            }
            return response;
        }

        public static async Task<ReturnResponse<DMNguoiHienMau>> CreateRegisterAsync(PersonalInfo view,
            BloodBankContext context,
            IMapper mapper)
        {
            var response = new ReturnResponse<DMNguoiHienMau>();

            var model = await context.DMNguoiHienMau.AsNoTracking()
                .OrderByDescending(i => i.NguoiHienMauId)
                .FirstOrDefaultAsync(i => i.CMND == view.IdCard && i.SoDT == view.PhoneNr).ConfigureAwait(false);
            if (model is not null)
                response.Success(model, CommonResources.Ok);
            else
            {
                //var id = await context.DMNguoiHienMau.AsNoTracking().CountAsync(i => i.NguoiHienMauId.HasValue).ConfigureAwait(false);
                //model = mapper.Map<DMNguoiHienMau>(view);
                //model.NguoiHienMauId = id + 1;
                //await context.DMNguoiHienMau.AddAsync(model).ConfigureAwait(false);

                //await AddNguoiHienMauSystemUserAsync(view.PhoneNr, model.NguoiHienMauId, context).ConfigureAwait(false);

                //await context.SaveChangesAsync().ConfigureAwait(false);

                //response.Success(model, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ResultView> GetAsync(string idCardNr
            , BloodBankContext context)
        {
            var view = new ResultView();

            DateTime? ngayHienMauGanNhat = null;
            bool duongTinhGanNhat = false;

            var nguoiHienMau = await context.DMNguoiHienMau.AsNoTracking().OrderBy(i => i.NguoiHienMauId).LastOrDefaultAsync(i => i.CMND == idCardNr).ConfigureAwait(false);
            if (nguoiHienMau is not null)
            {
                var lichSuHienMau = await context.LichSuHienMau.AsNoTracking().OrderBy(i => i.Id).LastOrDefaultAsync(i => i.NguoiHienMauId == nguoiHienMau.NguoiHienMauId).ConfigureAwait(false);
                if (lichSuHienMau is not null)
                {
                    ngayHienMauGanNhat = lichSuHienMau.NgayThu;
                    duongTinhGanNhat = !(lichSuHienMau.KetQuaGiangMai == KetQuaXetNghiem.AM_TINH
                        && lichSuHienMau.KetQuaHbsAg == KetQuaXetNghiem.AM_TINH
                        && lichSuHienMau.KetQuaHCV == KetQuaXetNghiem.AM_TINH
                        && lichSuHienMau.KetQuaHIV == KetQuaXetNghiem.AM_TINH
                        && lichSuHienMau.KetQuaSotRet == KetQuaXetNghiem.AM_TINH
                        && (lichSuHienMau.KetQuaKTBT.GetValueOrDefault(KetQuaXetNghiem.AM_TINH) == KetQuaXetNghiem.AM_TINH));
                }

                view.NguoiHienMau = nguoiHienMau.Adapt<DMNguoiHienMauView>();
                view.NgayHienMauGanNhat = ngayHienMauGanNhat;
                view.DuongTinhGanNhat = duongTinhGanNhat;
            }

            return view;
        }
    }
}
