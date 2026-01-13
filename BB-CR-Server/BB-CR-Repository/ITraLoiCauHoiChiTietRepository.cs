using BB.CR.Providers.Bases;
using BB.CR.Views;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface ITraLoiCauHoiChiTietRepository
    {
        Task<ReturnResponse<List<SurveyQuestionView>>> GetAsync(long id, ILogger logger);
    }
}
