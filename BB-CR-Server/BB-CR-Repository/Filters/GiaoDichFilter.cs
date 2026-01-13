using BB.CR.Models;
using BB.CR.Providers;
using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using Mapster;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BB.CR.Repositories.Filters
{
    internal class GiaoDichFilter
    {
        public static IQueryable<GiaoDichView> ExecuteFiltering(GiaoDichCriteria criteria, IQueryable<GiaoDich> giaoDichs,
            IQueryable<SystemUser> users,
            IQueryable<DMDonViCapMau> donCungCaps)
        {
            if (criteria is not null)
            {
                if (criteria.NgayTu is not null)
                    giaoDichs = giaoDichs.Where(i => i.Ngay.Date >= criteria.NgayTu.Value.Date);

                if (criteria.NgayDen.HasValue)
                    giaoDichs = giaoDichs.Where(i => i.Ngay.Date <= criteria.NgayDen.Value.Date);

                if (criteria.GiaoDichIds?.Count > 0)
                    giaoDichs = giaoDichs.Where(i => i.GiaoDichId > 0 && criteria.GiaoDichIds.Contains(i.GiaoDichId));

                if (criteria.LoaiPhieus?.Count > 0)
                    giaoDichs = giaoDichs.Where(i => criteria.LoaiPhieus.Contains(i.LoaiPhieu));

                if (criteria.TinhTrangs?.Count > 0)
                    giaoDichs = giaoDichs.Where(i => criteria.TinhTrangs.Contains(i.TinhTrang));

                if (!string.IsNullOrWhiteSpace(criteria.MaDonViCapMau))
                    giaoDichs = giaoDichs.Where(i => EF.Functions.Like(i.MaDonViCapMau, $"%{criteria.MaDonViCapMau}%"));
            }

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference return.
            var rs = from giaoDich in giaoDichs
                     join donViCungCap in donCungCaps on giaoDich.MaDonViCapMau equals donViCungCap.MaDonVi into _donViCungCaps
                     from donViCungCap in _donViCungCaps.DefaultIfEmpty()
                     join creator in users on giaoDich.CreatedBy equals creator.IdCardNr into _creators
                     from creator in _creators.DefaultIfEmpty()
                     join updator in users on giaoDich.UpdatedBy equals updator.IdCardNr into _updators
                     from updator in _updators.DefaultIfEmpty()
                     select new GiaoDichView
                     {
                         GiaoDichId = giaoDich.GiaoDichId,
                         LoaiPhieu = giaoDich.LoaiPhieu,
                         TinhTrang = giaoDich.TinhTrang,
                         Ngay = giaoDich.Ngay,
                         MaDonViCapMau = giaoDich.MaDonViCapMau,
                         DonViCapMauName = donViCungCap.TenDonVi,
                         GhiChu = giaoDich.GhiChu,
                         CreatedBy = giaoDich.CreatedBy,
                         CreatedDate = giaoDich.CreatedDate,
                         UpdatedBy = giaoDich.UpdatedBy,
                         UpdatedDate = giaoDich.UpdatedDate,
                         IsLocked = giaoDich.IsLocked,
                         LoaiPhieuDescription = giaoDich.LoaiPhieu.GetDescription(),
                         TinhTrangDescription = giaoDich.TinhTrang.GetDescription(),
                         CreatorName = creator.Name,
                         UpdatorName = updator.Name,
                         TotalQuantity = giaoDich.GiaoDichChiTiets.SelectMany(o => o.GiaoDichCons).Sum(j => j.SoLuong),
                         TotalApproveQuantity = giaoDich.GiaoDichChiTiets.SelectMany(o => o.GiaoDichCons).Sum(j => j.SoLuongDuyet),
                     };
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8604 // Possible null reference argument.

            return rs;
        }
    }
}
