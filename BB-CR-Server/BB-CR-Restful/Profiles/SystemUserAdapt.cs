using BB.CR.Models;
using BB.CR.Views.Authenticate;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class SystemUserAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<SystemUser, SystemUser>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.UserCode, src => src.UserCode)
                .Map(dest => dest.Active, src => src.Active)
                .Map(dest => dest.AppRole, src => src.AppRole)
                .Map(dest => dest.IdCardNr, src => (src.IdCardNr ?? string.Empty).Trim())
                .Map(dest => dest.DeviceId, src => src.DeviceId)
                .Map(dest => dest.FireBaseToken, src => src.FireBaseToken)
                .Ignore(dest => dest.Password);

            TypeAdapterConfig<RegisterView, SystemUser>
                .NewConfig()
                .Ignore(dest => dest.DeviceId!);
        }
    }
}
