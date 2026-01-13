using BB.CR.Models;
using BB.CR.Views;
using Mapster;

namespace BB.CR.Rest.Profiles
{
    public class SurveyQuestionAdapt
    {
        public static void ConfigureMapping()
        {
            TypeAdapterConfig<SurveyQuestion, SurveyQuestionView>
                .NewConfig();

            TypeAdapterConfig<SurveyQuestionView, TraLoiCauHoiChiTiet>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.YesAnswer, src => src.Yes)
                .Map(dest => dest.NoAnswer, src => src.No)
                .Map(dest => dest.GhiChu, src => src.Notes)
                .Map(dest => dest.SurveyQuestionId, src => src.Id);
        }
    }
}
