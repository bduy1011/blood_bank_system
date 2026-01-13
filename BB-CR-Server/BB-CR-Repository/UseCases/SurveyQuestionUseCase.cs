using BB.CR.Views;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.UseCases
{
    internal class SurveyQuestionUseCase
    {
        public static async Task<List<SurveyQuestionView>> LoadAsync(BloodBankContext context
            , IMapper mapper)
        {
            return mapper.Map<List<SurveyQuestionView>>(
                await context.SurveyQuestion
                    .AsNoTracking()
                    .Where(i => i.IsEnabled == true)
                    .OrderBy(i => i.OrderIndex)
                    .ToListAsync().ConfigureAwait(false));
        }
    }
}
