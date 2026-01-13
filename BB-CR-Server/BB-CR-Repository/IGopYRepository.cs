using BB.CR.Models;
using BB.CR.Repositories.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;

namespace BB.CR.Repositories
{
    public interface IGopYRepository : IBaseRepository<GopYView, GopYCriteria, GopY>
    {
    }
}
