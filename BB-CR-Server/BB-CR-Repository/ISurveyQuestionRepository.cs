using BB.CR.Views;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ISurveyQuestionRepository
    {
        Task<List<SurveyQuestionView>?> LoadAsync(ILogger logger
            , IMapper mapper);
    }
}
