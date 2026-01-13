using BB.CR.Models;
using BB.CR.Views;
using MapsterMapper;

namespace BB.CR.Repositories.Mappers
{
    internal class GiaoDichMapper
    {
        public static GiaoDichView ExecuteMapper(IMapper mapper, GiaoDich model)
        {
            var view = mapper.Map<GiaoDichView>(model);
            return view;
        }
    }
}
