using BB.CR.Repositories;
using BB.CR.Repositories.Implements;
using BB.CR.Rest.MessageQueues;
using BB.CR.Rest.Services;

namespace BB.CR.Rest.Extensions
{
    public static class ServicesExtensions
    {
        public static void SingletonReg(this IServiceCollection services)
        {
            services.AddSingleton<IDotLayMauRepository, DotLayMauRepository>();
            services.AddSingleton<ISystemUserRepository, SystemUserRepository>();
            services.AddSingleton<IDMNguoiHienMauRepository, DMNguoiHienMauRepository>();
            services.AddSingleton<IDMDonViCapMauRepository, DMDonViCapMauRepository>();
            services.AddSingleton<ILichSuHienMauRepository, LichSuHienMauRepository>();
            services.AddSingleton<IDMTinhRepository, DMTinhRepository>();
            services.AddSingleton<IDMChungRepository, DMChungRepository>();
            services.AddSingleton<IDangKyHienMauRepository, DangKyHienMauRepository>();
            services.AddSingleton<IGopYRepository, GopYRepository>();
            services.AddSingleton<IDMHuyenRepository, DMHuyenRepository>();
            services.AddSingleton<IDMXaRepository, DMXaRepository>();
            services.AddSingleton<ISystemConfigRepository, SystemConfigRepository>();
            services.AddSingleton<ISystemSlideRepository, SystemSlideRepository>();
            services.AddSingleton<ISurveyQuestionRepository, SurveyQuestionRepository>();
            services.AddSingleton<ITraLoiCauHoiChiTietRepository, TraLoiCauHoiChiTietRepository>();
            services.AddSingleton<ITinTucRepository, TinTucRepository>();
            services.AddSingleton<IGiaoDichRepository, GiaoDichRepository>();

            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<ICapMatKhauRepository, CapMatKhauRepository>();

            services.AddScoped<IMessageProducer, MessageProducer>();

            services.AddScoped<ICauHinhTonKhoRepository, CauHinhTonKhoRepository>();
        }
    }
}
