using BB.CR.Models;
using BB.CR.Views.Criterias;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories.Filters
{
    internal class CapMatKhauFilter
    {
        internal static IQueryable<CapMatKhau> Seek(CapMatKhauCriteria criteria, IQueryable<CapMatKhau> model)
        {
            if (criteria is null)
                return model;

            if (criteria.TuNgay is not null)
                model = model.Where(i => i.CreatedDate.Date >= criteria.TuNgay.Value.Date);

            if (criteria.DenNgay is not null)
                model = model.Where(i => i.CreatedDate.Date <= criteria.DenNgay.Value.Date);

            if (!string.IsNullOrWhiteSpace(criteria.SoDT))
                model = model.Where(i => EF.Functions.Like(i.SoDT, $"%{criteria.SoDT}%"));

            if (!string.IsNullOrWhiteSpace(criteria.CMND))
                model = model.Where(i => EF.Functions.Like(i.CMND, $"%{criteria.CMND}%"));

            return model;
        }
    }
}
