using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using BB.CR.Views;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    public class TraLoiCauHoiChiTietUseCase
    {
        public static async Task<ReturnResponse<List<SurveyQuestionView>>> GetAsync(long id
            , BloodBankContext context)
        {
            var response = new ReturnResponse<List<SurveyQuestionView>>();
            var model = await context.TraLoiCauHoiChiTiet
                .AsNoTracking()
                .Where(i => i.TraLoiCauHoiId == id)
                .ToListAsync()
                .ConfigureAwait(false);
            if (model is null)
                response.Error(System.Net.HttpStatusCode.NotFound, CommonResources.NotFound);
            else
            {
                var surveyQuestion = await context.SurveyQuestion.AsNoTracking().ToListAsync().ConfigureAwait(false);
                var surveyQuestions = new List<SurveyQuestionView>();
                model.ForEach(i =>
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
                response.Success(surveyQuestions, CommonResources.Ok);
            }

            return response;
        }
    }
}
