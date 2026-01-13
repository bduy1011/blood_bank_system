using BB.CR.Providers.Bases;
using BB.CR.Repositories.Bases;
using BB.CR.Repositories.UseCases;
using BB.CR.Views;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories.Implements
{
    public class TraLoiCauHoiChiTietRepository : ITraLoiCauHoiChiTietRepository
    {
        public async Task<ReturnResponse<List<SurveyQuestionView>>> GetAsync(long id
            , ILogger logger)
        {
            using var context = new BloodBankContext();

            var response = await BaseUseCase.ExecuteAsync(
                async () => await TraLoiCauHoiChiTietUseCase.GetAsync(id, context).ConfigureAwait(false)
                , logger
                , context).ConfigureAwait(false);

            return response;
        }
    }
}
