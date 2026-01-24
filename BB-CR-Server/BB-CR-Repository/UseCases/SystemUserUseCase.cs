using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Repositories.Resources;
using BB.CR.Views.Otp;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;

namespace BB.CR.Repositories.UseCases
{
    public static class SystemUserUseCase
    {
        const string _defaultPassword = "123456";
        const string _defaultOtp = "999999";

        public static string? UpdateByChangePassword(SystemUser systemUser, string oldPassword)
        {
            var oldHashed = AlgorisMd5.GetMd5Hash(oldPassword);

            if (!string.Equals(systemUser.Password, oldHashed, StringComparison.OrdinalIgnoreCase))
                return BzLogicResource.PasswordUnMatched;

            return null;
        }

        public static async Task<ReturnResponse<SystemUser>> UpdateAsync(string code, SystemUser model, BloodBankContext context, IMapper mapper)
        {
            ReturnResponse<SystemUser> response = new();

            // NOTE:
            // UserCode là PRIMARY KEY của bảng SystemUser (BloodBankContext config).
            // EF Core không "update" được primary key bằng context.Update() thông thường.
            // Nếu cần đổi CCCD và đồng thời đổi UserCode = CCCD mới thì phải "rename" account:
            // tạo record mới (UserCode mới) + xóa record cũ trong cùng transaction.
            SystemUser? systemUser = await context.SystemUser.FirstOrDefaultAsync(i => i.UserCode == code).ConfigureAwait(false);
            if (systemUser is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                // Lưu CCCD cũ TRƯỚC KHI update để có thể update các bảng liên quan
                var oldIdCardNr = systemUser.IdCardNr;
                
                if (model.IdCardNr != systemUser.IdCardNr)
                {
                    model.IdCardNr = (model.IdCardNr ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(model.IdCardNr))
                    {
                        response.Error(HttpStatusCode.Conflict, string.Format(BzLogicResource.SystemUserCheckIdCard, model.IdCardNr));
                        goto ReturnResponse;
                    }

                    var isExistedSystemUser = await context.SystemUser.AsNoTracking().Where(i => i.IdCardNr == model.IdCardNr).ToListAsync().ConfigureAwait(false);

                    if (isExistedSystemUser.Count != 0)
                    {
                        response.Error(HttpStatusCode.Conflict, string.Format(BzLogicResource.SystemUserCheckIdCard, model.IdCardNr));
                        goto ReturnResponse;
                    }
                    else
                    {
                        var isExistedDMNguoiHienMau = await context.DMNguoiHienMau.AsNoTracking().Where(i => i.CMND == model.IdCardNr).ToListAsync().ConfigureAwait(false);
                        if (isExistedDMNguoiHienMau.Count == 2)
                        {
                            response.Error(HttpStatusCode.Conflict, string.Format(BzLogicResource.DMNguoiHienMauDuplicateCMND, Settings.Hospital?.Phone));
                            goto ReturnResponse;
                        }

                        var dmNguoiHienMau = isExistedDMNguoiHienMau.FirstOrDefault();
                        if (dmNguoiHienMau is not null)
                            model.IdCardNr = dmNguoiHienMau?.CMND;
                    }
                }

                // Apply updates on tracked entity (preserve password/otp flags/etc.)
                systemUser.Name = model.Name;
                systemUser.PhoneNumber = model.PhoneNumber;
                systemUser.AppRole = model.AppRole;
                systemUser.Active = model.Active;
                systemUser.DeviceId = model.DeviceId;
                systemUser.FireBaseToken = model.FireBaseToken;
                systemUser.IdCardNr = model.IdCardNr;

                // Kiểm tra xem CCCD có thay đổi không
                var newIdCardNr = (systemUser.IdCardNr ?? string.Empty).Trim();
                var idCardChanged = !string.IsNullOrWhiteSpace(oldIdCardNr) && 
                                   !string.IsNullOrWhiteSpace(newIdCardNr) && 
                                   !string.Equals(oldIdCardNr, newIdCardNr, StringComparison.OrdinalIgnoreCase);

                // If IdCardNr changed and your business rule is UserCode = CCCD/CMND,
                // then migrate (rename) UserCode to new CCCD.
                var newUserCode = newIdCardNr;
                
                if (!string.IsNullOrWhiteSpace(newUserCode) && !string.Equals(newUserCode, code, StringComparison.OrdinalIgnoreCase))
                {
                    // Prevent duplicate key
                    var existedByUserCode = await context.SystemUser.AsNoTracking()
                        .AnyAsync(i => i.UserCode == newUserCode)
                        .ConfigureAwait(false);
                    if (existedByUserCode)
                    {
                        response.Error(HttpStatusCode.Conflict, string.Format(BzLogicResource.AccountExisted, newUserCode));
                        goto ReturnResponse;
                    }

                    var newSystemUser = new SystemUser
                    {
                        UserCode = newUserCode,
                        Name = systemUser.Name,
                        Password = systemUser.Password, // preserve hashed password
                        AppRole = systemUser.AppRole,
                        Active = systemUser.Active,
                        DeviceId = systemUser.DeviceId,
                        PhoneNumber = systemUser.PhoneNumber,
                        OtpCode = systemUser.OtpCode,
                        ExpiredOn = systemUser.ExpiredOn,
                        AcceptedOtp = systemUser.AcceptedOtp,
                        IsDataQLMau = systemUser.IsDataQLMau,
                        IdCardNr = systemUser.IdCardNr,
                        CreatedOn = systemUser.CreatedOn,
                        FireBaseToken = systemUser.FireBaseToken,
                    };

                    await context.SystemUser.AddAsync(newSystemUser).ConfigureAwait(false);
                    context.SystemUser.Remove(systemUser);
                    
                    // Update tất cả các bảng liên quan khi đổi CCCD
                    if (idCardChanged)
                    {
                        // Update DangKyHienMau
                        var dangKyHienMauList = await context.DangKyHienMau
                            .Where(i => i.CMND == oldIdCardNr)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        foreach (var dkhm in dangKyHienMauList)
                        {
                            dkhm.CMND = newUserCode;
                        }

                        // Update DMNguoiHienMau (CMND được encrypt, EF Core sẽ tự động xử lý)
                        var dmNguoiHienMauList = await context.DMNguoiHienMau
                            .Where(i => i.CMND == oldIdCardNr)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        foreach (var dm in dmNguoiHienMauList)
                        {
                            dm.CMND = newUserCode;
                        }

                        // Update CapMatKhau
                        var capMatKhauList = await context.CapMatKhaus
                            .Where(i => i.CMND == oldIdCardNr)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        foreach (var cmk in capMatKhauList)
                        {
                            cmk.CMND = newUserCode;
                        }
                    }
                    
                    int count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                        response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                    else
                        response.Success(newSystemUser, CommonResources.Ok);
                }
                else
                {
                    // Nếu chỉ đổi CCCD mà không đổi UserCode, vẫn cần update các bảng liên quan
                    if (idCardChanged)
                    {
                        // Update DangKyHienMau
                        var dangKyHienMauList = await context.DangKyHienMau
                            .Where(i => i.CMND == oldIdCardNr)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        foreach (var dkhm in dangKyHienMauList)
                        {
                            dkhm.CMND = newIdCardNr;
                        }

                        // Update DMNguoiHienMau (CMND được encrypt, EF Core sẽ tự động xử lý)
                        var dmNguoiHienMauList = await context.DMNguoiHienMau
                            .Where(i => i.CMND == oldIdCardNr)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        foreach (var dm in dmNguoiHienMauList)
                        {
                            dm.CMND = newIdCardNr;
                        }

                        // Update CapMatKhau
                        var capMatKhauList = await context.CapMatKhaus
                            .Where(i => i.CMND == oldIdCardNr)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        foreach (var cmk in capMatKhauList)
                        {
                            cmk.CMND = newIdCardNr;
                        }
                    }
                    
                    context.SystemUser.Update(systemUser);
                    int count = await context.SaveChangesAsync().ConfigureAwait(false);
                    if (count == 0)
                        response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                    else
                        response.Success(systemUser, CommonResources.Ok);
                }
            }

        ReturnResponse:
            return response;
        }

        public static async Task<ReturnResponse<SystemUser>> CreateAsync(SystemUser model, BloodBankContext context)
        {
            ReturnResponse<SystemUser> response = new();

            SystemUser? data = await context.SystemUser.AsNoTracking().FirstOrDefaultAsync(i => i.UserCode == model.UserCode).ConfigureAwait(false);
            if (data is not null)
            {
                if (data.Active)
                    response.Error(HttpStatusCode.Conflict, BzLogicResource.AccountExisted);
                else
                    response.Error(HttpStatusCode.Conflict, BzLogicResource.AccountUnRegisterAdmin);
            }
            else
            {
                if (Settings.KioskDevices?.Count > 0 // Has value
                    && !string.IsNullOrWhiteSpace(model.DeviceId) // Device input not null or empty
                    && Settings.KioskDevices.Contains(model.DeviceId)) // Contains in list Kiosk
                    goto NextCheckThreeTimes;

                // Check DeviceId register 3 times.
                if (model.DeviceId is not null
                    && !string.IsNullOrWhiteSpace(model.DeviceId))
                {
                    var countdDevices = await context.SystemUser
                        .AsNoTracking()
                        .Where(i => i.DeviceId != null && i.DeviceId != string.Empty && i.DeviceId == model.DeviceId && i.CreatedOn.HasValue && i.CreatedOn.Value.Date == DateTime.Now.Date)
                        .CountAsync()
                        .ConfigureAwait(false);
                    if (countdDevices >= 3)
                    {
                        response.Error(HttpStatusCode.Conflict, SystemUserResources.OverThreeTimesToRegister);
                        goto NotNextStep; // Unbreak all next step return warning
                    }
                }

            NextCheckThreeTimes:
                model.AppRole = AppRole.User; // Đăng ký ở hàm cũ
                model.CreatedOn = DateTime.Now;

                await context.SystemUser.AddAsync(model).ConfigureAwait(false);
                int count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(model, CommonResources.Ok);
            }

        NotNextStep:
            return response;
        }

        public static async Task<ReturnResponse<List<SystemUser>>> UpdateLocalAsync(List<SystemUser> model, BloodBankContext context
            , IMapper mapper)
        {
            var response = new ReturnResponse<List<SystemUser>>();

            var data = await context.SystemUser.AsNoTracking().Where(i => model.Select(o => o.UserCode).Contains(i.UserCode)).ToListAsync().ConfigureAwait(false);
            if (data?.Count > 0)
            {
                // When local push data ignore fields
                data.ForEach(i =>
                {
                    var _model = model.FirstOrDefault(o => o.UserCode == i.UserCode);
                    if (_model is not null)
                    {
                        i.Password = _model.Password;
                        i.AppRole = _model.AppRole;
                        i.Active = _model.Active;
                        i.IsDataQLMau = _model.IsDataQLMau;
                    }
                });
                context.SystemUser.UpdateRange(data);

                var unInDatabase = model.Where(i => !data.Select(o => o.UserCode).Contains(i.UserCode)).ToList();
                if (unInDatabase?.Count > 0)
                {
                    unInDatabase.ForEach(i => i.CreatedOn = DateTime.Now);
                    await context.SystemUser.AddRangeAsync(unInDatabase).ConfigureAwait(false);
                }
            }
            else
            {
                model.ForEach(i => i.CreatedOn = DateTime.Now);
                await context.SystemUser.AddRangeAsync(model).ConfigureAwait(false);
            }

            var count = await context.SaveChangesAsync().ConfigureAwait(false);
            if (count == 0)
                response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
            else
            {
                data = await context.SystemUser.AsNoTracking().Where(i => model.Select(o => o.UserCode).Contains(i.UserCode)).ToListAsync().ConfigureAwait(false);
                response.Success(data, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<bool>> RegisterByCMNDAsync(string cmnd,
            string? deviceId,
            string? fullName,
            BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var data = await context.SystemUser
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IdCardNr != null && i.IdCardNr.Trim() != string.Empty && i.IdCardNr.Trim() == cmnd.Trim()).ConfigureAwait(false);
            if (data is not null)
            {
                if (data.Active == false)
                {
                }
                else
                {
                    if (data.AcceptedOtp)
                        response.Error(HttpStatusCode.Conflict, BzLogicResource.PhoneNumberAccepted);
                    else
                        response.Error(HttpStatusCode.Conflict, BzLogicResource.PhoneNumberNotAccepted);
                }
            }
            else
            {
                string otpCode = (Settings.BreakGenerate) ? _defaultOtp : Helpers.RandomOTP();
                data = new SystemUser
                {
                    UserCode = cmnd,
                    Name = fullName ?? string.Empty,
                    Password = AlgorisMd5.GetMd5Hash(_defaultPassword),
                    Active = true,
                    IdCardNr = cmnd,
                    AppRole = AppRole.User,
                    OtpCode = otpCode,
                    ExpiredOn = DateTime.Now,
                    DeviceId = deviceId,
                    CreatedOn = DateTime.Now,
                };
                await context.SystemUser.AddAsync(data).ConfigureAwait(false);
                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(true, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<SystemUser>> CheckOtpAsync(RegisterOtpView register
            , BloodBankContext context)
        {
            var response = new ReturnResponse<SystemUser>();

            var data = await context.SystemUser
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.PhoneNumber != null && i.PhoneNumber.Trim() != string.Empty /*&& i.PhoneNumber == register.PhoneNumber*/).ConfigureAwait(false);

            if (data is null)
                response.Error(HttpStatusCode.Conflict, BzLogicResource.AccountUnRegistered);
            else
            {
                // Check DeviceId
                if (data.DeviceId != register.DeviceId)
                    response.Error(HttpStatusCode.Conflict, BzLogicResource.DeviceOtherCheckOTP);
                else // Check OTP
                {
                    // Check expire time
                    var duration = DateTime.Now - data.ExpiredOn.GetValueOrDefault(DateTime.Now);
                    if (duration.TotalMinutes > Settings.ExpireTimeOtp)
                        response.Error(HttpStatusCode.Conflict, BzLogicResource.OTPExpiredTime);
                    else // Check OTP same
                    {
                        if (string.IsNullOrWhiteSpace(data.OtpCode))
                            response.Error(HttpStatusCode.Conflict, BzLogicResource.OTPExpiredTime);
                        else
                        {
                            if (!data.OtpCode.Equals(register.OtpCode, StringComparison.OrdinalIgnoreCase))
                                response.Error(HttpStatusCode.Conflict, BzLogicResource.OTPInvalid);
                            else
                            {
                                data.AcceptedOtp = true;
                                context.SystemUser.Update(data);

                                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                                if (count == 0)
                                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                                else
                                    response.Success(data, CommonResources.Ok);
                            }
                        }
                    }
                }
            }

            return response;
        }

        public static async Task<ReturnResponse<bool>> ResendOtpAsync(RegisterOtpView register
            , BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var data = await context.SystemUser
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.PhoneNumber != null && i.PhoneNumber.Trim() != string.Empty /*&& i.PhoneNumber == register.PhoneNumber*/).ConfigureAwait(false);
            if (data is null)
                response.Error(HttpStatusCode.Conflict, BzLogicResource.AccountUnRegistered);
            else
            {
                var otpCode = (Settings.BreakGenerate) ? _defaultOtp : Helpers.RandomOTP();
                data.OtpCode = otpCode;
                data.ExpiredOn = DateTime.UtcNow;
                context.SystemUser.Update(data);

                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(true, CommonResources.Ok);
            }

            return response;
        }

        public static async Task<ReturnResponse<bool>> ChangePasswordAsync(ChangePasswordOtpView view
            , bool isRegister
            , BloodBankContext context
            , bool isOnlyLocal = false)
        {
            var response = new ReturnResponse<bool>();

            var _ = context.SystemUser.AsNoTracking();
            if (isOnlyLocal) _ = _.Where(i => !i.IsDataQLMau);

            var model = await _.FirstOrDefaultAsync(i => i.UserCode == view.UserCode).ConfigureAwait(false);
            if (model is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                if (isRegister) // Đăng ký mới, Quên mật khẩu
                {
                    if (view.NewPassword != view.ConfirmedNewPassword)
                        response.Error(HttpStatusCode.Conflict, BzLogicResource.NewPasswordConfirmPasswordNotSame);
                    else
                    {
                        model.Password = AlgorisMd5.GetMd5Hash(view.NewPassword);
                        context.SystemUser.Update(model);

                        // Update reset password on
                        var subModels = await context.CapMatKhaus
                            .AsNoTracking()
                            .Where(i => i.CMND == model.IdCardNr && i.ResetPasswordOn == null)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        if (subModels?.Count > 0)
                        {
                            subModels.ForEach(i => i.ResetPasswordOn = DateTime.Now);
                            context.CapMatKhaus.UpdateRange(subModels);
                        }

                        await context.SaveChangesAsync().ConfigureAwait(false);
                        response.Success(true, CommonResources.Ok);
                    }
                }
                else // Thay đổi mật khẩu
                {
                    if (AlgorisMd5.GetMd5Hash(view.OldPassword!) != model.Password)
                        response.Error(HttpStatusCode.Conflict, BzLogicResource.OldPasswordWrong);
                    else if (view.NewPassword != view.ConfirmedNewPassword)
                        response.Error(HttpStatusCode.Conflict, BzLogicResource.NewPasswordConfirmPasswordNotSame);
                    else
                    {
                        model.Password = AlgorisMd5.GetMd5Hash(view.NewPassword);
                        context.SystemUser.Update(model);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        response.Success(true, CommonResources.Ok);
                    }
                }
            }


            return response;
        }

        public static async Task<ReturnResponse<SystemUser?>> DeleteAsync(string userCode
            , BloodBankContext context)
        {
            var response = new ReturnResponse<SystemUser?>();

            var model = await context.SystemUser.AsNoTracking().FirstOrDefaultAsync(i => i.UserCode == userCode).ConfigureAwait(false);
            if (model is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                // Xóa chữ ký của user trước khi xóa user
                if (!string.IsNullOrWhiteSpace(model.IdCardNr))
                {
                    DeleteUserSignatureFiles(model.IdCardNr);
                }

                context.SystemUser.Remove(model);
                var count = await context.SaveChangesAsync().ConfigureAwait(false);
                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(null, CommonResources.Ok);
            }

            return response;
        }

        internal static async Task<ReturnResponse<bool>> RemoveAsync(string userCode, BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var model = await context.SystemUser.AsNoTracking().FirstOrDefaultAsync(i => i.UserCode == userCode)
                .ConfigureAwait(false);

            if (model is null)
                response.Error(HttpStatusCode.NoContent, CommonResources.NotFound);
            else
            {
                // Xóa chữ ký của user trước khi xóa IdCardNr
                if (!string.IsNullOrWhiteSpace(model.IdCardNr))
                {
                    DeleteUserSignatureFiles(model.IdCardNr);
                }

                model.PhoneNumber = null;
                model.IdCardNr = null;
                context.SystemUser.Update(model);

                var count = await context.SaveChangesAsync()
                    .ConfigureAwait(false);

                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(true, CommonResources.Ok);
            }

            return response;
        }

        internal static async Task<ReturnResponse<bool>> RemoveIdCardAsync(string idCard, BloodBankContext context)
        {
            var response = new ReturnResponse<bool>();

            var models = await context.SystemUser
                .AsNoTracking()
                .Where(i => i.IdCardNr == idCard)
                .ToListAsync()
                .ConfigureAwait(false);

            if (models is null)
                response.Error(HttpStatusCode.NoContent, CommonResources.NotFound);
            else
            {
                // Xóa chữ ký của user trước khi xóa IdCardNr
                if (!string.IsNullOrWhiteSpace(idCard))
                {
                    DeleteUserSignatureFiles(idCard);
                }

                models.ForEach(i =>
                {
                    i.PhoneNumber = null;
                    i.IdCardNr = null;
                });
                context.SystemUser.UpdateRange(models);

                var count = await context.SaveChangesAsync()
                    .ConfigureAwait(false);

                if (count == 0)
                    response.Error(HttpStatusCode.NoContent, CommonResources.NoContent);
                else
                    response.Success(true, CommonResources.Ok);
            }

            return response;
        }

        /// <summary>
        /// Lấy đường dẫn root cho signature files
        /// </summary>
        private static string GetSignatureRootPath()
        {
            // NOTE:
            // Settings.PathActual in this project is configured as a URL (e.g. http://localhost:8090/images/)
            // for serving static files, NOT a filesystem folder path.
            // If we try to use it as a directory path, it will throw "Illegal characters in path".
            if (!string.IsNullOrWhiteSpace(Settings.PathActual))
            {
                if (Uri.TryCreate(Settings.PathActual, UriKind.Absolute, out var uri)
                    && (uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase)
                        || uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase)))
                {
                    // fall back to local folder
                }
                else
                {
                    return Settings.PathActual!;
                }
            }

            // Fallback when PathActual is not configured
            return Path.Combine(AppContext.BaseDirectory, "App_Data");
        }

        /// <summary>
        /// Xóa thư mục chữ ký của user dựa vào identityCard (CCCD/CMND)
        /// </summary>
        private static void DeleteUserSignatureFiles(string identityCard)
        {
            if (string.IsNullOrWhiteSpace(identityCard))
                return;

            try
            {
                // Sanitize identityCard để tránh lỗi path
                var safeIdentityCard = string.Join("_", identityCard.Split(Path.GetInvalidFileNameChars()));
                var signatureDir = Path.Combine(GetSignatureRootPath(), "signatures", "users", safeIdentityCard);

                if (Directory.Exists(signatureDir))
                {
                    Directory.Delete(signatureDir, recursive: true);
                }
            }
            catch
            {
                // Ignore errors khi xóa file (có thể file đã bị xóa hoặc không có quyền)
            }
        }

        /// <summary>
        /// Reload information end-user
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static async Task<ReturnResponse<SystemUser>> GetByIdCardNrAsync(string idCard, BloodBankContext context)
        {
            var response = new ReturnResponse<SystemUser>();

            var model = await context.SystemUser
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IdCardNr == idCard)
                .ConfigureAwait(false);

            if (model is null)
                response.Error(HttpStatusCode.NotFound, CommonResources.NotFound);
            else
                response.Success(model, CommonResources.Ok);

            return response;
        }
    }
}
