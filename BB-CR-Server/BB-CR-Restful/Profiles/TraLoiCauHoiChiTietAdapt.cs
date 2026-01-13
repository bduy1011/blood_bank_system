using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class TraLoiCauHoiChiTietAdapt
    {
        public static void ConfigureMapping()
        {
#pragma warning disable CS8603 // Possible null reference return.
            TypeAdapterConfig<TraLoiCauHoiChiTiet, TraLoiCauHoiChiTietView>
                .NewConfig()
                .Ignore(dest => dest.TraLoiCauHoi);
#pragma warning restore CS8603 // Possible null reference return.

            TypeAdapterConfig<TraLoiCauHoiChiTiet, SurveyQuestionView>
                .NewConfig()
                .Map(dest => dest.Id, src => src.SurveyQuestionId)
                .Map(dest => dest.Yes, src => src.YesAnswer)
                .Map(dest => dest.No, src => src.NoAnswer)
                .Map(dest => dest.TraLoiCauHoiId, src => src.TraLoiCauHoiId);
        }
    }
}
