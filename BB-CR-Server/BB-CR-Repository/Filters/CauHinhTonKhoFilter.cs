using BB.CR.Models;
using BB.CR.Views.Criterias;

namespace BB.CR.Repositories.Filters
{
    internal class CauHinhTonKhoFilter
    {
        internal static IQueryable<CauHinhTonKho> Seek(CauHinhTonKhoCriteria criteria, IQueryable<CauHinhTonKho> model)
        {
            if (criteria is null)
                return model;

            if (criteria.NgayTu is not null)
                model = model.Where(i => i.Ngay.Date >= criteria.NgayTu.Value.Date);

            if (criteria.NgayDen is not null)
                model = model.Where(i => i.Ngay.Date <= criteria.NgayDen.Value.Date);

            //if (criteria.LoaiSanPham is not null)
            //    model = model.Where(i => i.LoaiSanPham == criteria.LoaiSanPham);

            if (criteria.Ids?.Count > 0)
                model = model.Where(i => criteria.Ids.Contains(i.Id));

            return model;
        }
    }
}
