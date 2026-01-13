using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class SurveyQuestionRepository : ISurveyQuestionRepository
    {
        public async Task<List<SurveyQuestionView>?> LoadAsync(ILogger logger
            , IMapper mapper)
        {
            using var context = new BloodBankContext();
            return await BaseUseCase.ReadAsync(
                async () => await SurveyQuestionUseCase.LoadAsync(context, mapper).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);
        }
    }
}
