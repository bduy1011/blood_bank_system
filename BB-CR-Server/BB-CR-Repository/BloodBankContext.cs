using BB.CR.Providers;
using BB.CR.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace BB.CR.Repositories
{
    public class BloodBankContext : DbContext
    {
        const string _smallDateTime = "smalldatetime";

        public BloodBankContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Step to run migrations database enviroment develop
            if (string.IsNullOrEmpty(Settings.ConnectionString))
                Settings.ConnectionString = @"Server=.;Database=NganHangMau;Encrypt=False;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(Settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.DotLayMau>(o =>
            {
                o.HasKey(i => i.DotLayMauId);
                o.Property(i => i.DotLayMauId).ValueGeneratedNever(); // Local push data
                o.Property(i => i.Ten).HasMaxLength(200);
                o.Property(i => i.TenDonViPhoiHop).HasMaxLength(200);
                o.Property(i => i.DiaDiemToChuc).HasMaxLength(500);
                o.Property(i => i.GhiChu).HasMaxLength(500);
                o.Property(i => i.NguoiLienHe).HasMaxLength(200);
                o.Property(i => i.MaXa).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenXa).HasMaxLength(100);
                o.Property(i => i.MaHuyen).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenHuyen).HasMaxLength(100);
                o.Property(i => i.MaTinh).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenTinh).HasMaxLength(100);
                o.Property(i => i.GoogleMapLink).HasMaxLength(100).IsUnicode(false);
                o.HasIndex(i => i.NgayGio, "IX_LichLayMau_Ngay");
                o.Property(i => i.DonViPhoiHopId).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.IsDuocDangKy).HasDefaultValue(1);

                o.HasMany<Models.LichSuHienMau>(d => d.LichSuHienMaus).WithOne(s => s.DotLayMau).HasForeignKey(s => s.DotLayMauId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Models.DMNguoiHienMau>(o =>
            {
                o.Property(i => i.NguoiHienMauId).ValueGeneratedNever();
                o.HasKey(i => i.NguoiHienMauId);
                o.Property(i => i.HoVaTen).HasMaxLength(100);
#pragma warning disable CS8604 // Possible null reference argument.
                o.Property(i => i.NgaySinh).HasMaxLength(50).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val));
                o.Property(i => i.NamSinh).HasMaxLength(32).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val));
                o.Property(i => i.HoVaTenNoDiacritics).HasMaxLength(100);
                o.Property(i => i.CMND).HasMaxLength(50).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val)).IsUnicode(false);
                o.Property(i => i.HoVaTen).HasMaxLength(100);
                o.Property(i => i.MaXa).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenXa).HasMaxLength(100);
                o.Property(i => i.MaHuyen).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenHuyen).HasMaxLength(100);
                o.Property(i => i.MaTinh).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenTinh).HasMaxLength(100);
                o.Property(i => i.DiaChiLienLac).HasMaxLength(200).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val));
                o.Property(i => i.DiaChiTamTru).HasMaxLength(200).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val));
                o.Property(i => i.NgheNghiep).HasMaxLength(100);
                o.Property(i => i.Email).HasMaxLength(100).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val)).IsUnicode(false);
                o.Property(i => i.SoDT).HasMaxLength(100).HasConversion(val => Algoris.Encrypt(Settings.KeySecurity, val), val => Algoris.Decrypt(Settings.KeySecurity, val)).IsUnicode(false);
                o.Property(i => i.MaNhomMau).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenNhomMau).HasMaxLength(100);
                o.Property(i => i.GhiChu).HasMaxLength(500);
                o.HasIndex(i => i.CMND, "IX_DMNguoiHienMau_CMND");
                o.HasIndex(i => i.SoDT, "IX_DMNguoiHienMau_SoDT");
                o.HasMany<Models.LichSuHienMau>(d => d.LichSuHienMaus).WithOne(s => s.DMNguoiHienMau).HasForeignKey(s => s.NguoiHienMauId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_LichSuHienMau_NguoiHienMauId");
                o.HasMany<Models.GopY>(d => d.GopYs).WithOne(s => s.DMNguoiHienMau).HasForeignKey(s => s.NguoiHienMauId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_GopY_NguoiHienMauId");
            });

            modelBuilder.Entity<Models.LichSuHienMau>(o =>
            {
                o.HasKey(i => i.Id);
                o.Property(i => i.MaVachId).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaSanPham).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenSanPham).HasMaxLength(100);
                o.Property(i => i.CapPhatChoDonVi).HasMaxLength(200);
                o.Property(i => i.GhiChuTuiMau).HasMaxLength(200);
                //o.Property(i => i.KetQuaNhomMau).HasMaxLength(20);
                o.Property(i => i.KetQuaKTBT).HasMaxLength(20);
                o.Property(i => i.KetQuaHbsAg).HasMaxLength(20);
                o.Property(i => i.KetQuaHIV).HasMaxLength(20);
                o.Property(i => i.KetQuaHCV).HasMaxLength(20);
                o.Property(i => i.KetQuaGiangMai).HasMaxLength(20);
                o.Property(i => i.KetQuaSotRet).HasMaxLength(20);

                o.HasIndex(i => i.NguoiHienMauId, "IX_LichSuHienMau_NguoiHienMauId");
            });

            modelBuilder.Entity<Models.DMTinh>(o =>
            {
                o.HasKey(i => i.MaTinh);
                o.Property(i => i.MaTinh).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaQuocGia).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.Ten).HasMaxLength(100);
                o.Property(i => i.UuTien).HasDefaultValue(0);
                o.HasMany<Models.DMHuyen>(s => s.DMHuyens).WithOne(d => d.DMTinh).HasForeignKey(d => d.MaTinh).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_DMHuyen_MaTinh");
            });

            modelBuilder.Entity<Models.DMHuyen>(o =>
            {
                o.HasKey(i => i.MaHuyen);
                o.Property(i => i.MaHuyen).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaQuocGia).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaTinh).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.Ten).HasMaxLength(100);
                o.HasIndex(i => i.MaTinh, "IX_DMHuyen_MaTinh");
                o.HasMany<Models.DMXa>(s => s.DMXas).WithOne(d => d.DMHuyen).HasForeignKey(d => d.MaHuyen).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_DMXa_MaHuyen");
            });

            modelBuilder.Entity<Models.DMXa>(o =>
            {
                o.HasKey(i => i.MaXa);
                o.Property(i => i.MaXa).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaQuocGia).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaHuyen).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.Ten).HasMaxLength(100);
                o.HasIndex(i => i.MaHuyen, "IX_DXa_MaHuyen");
            });

            modelBuilder.Entity<Models.GopY>(o =>
            {
                o.HasKey(i => i.Id);
                o.Property(i => i.HoVaTen).HasMaxLength(100);
                o.Property(i => i.LoaiGopY).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.NoiDung).HasMaxLength(500);
                o.Property(i => i.Email).HasMaxLength(100).IsUnicode(false);
                o.Property(i => i.SoDT).HasMaxLength(50).IsUnicode(false);
                o.Property(i => i.HinhAnh1).HasMaxLength(100);
                o.Property(i => i.HinhAnh2).HasMaxLength(100);
                o.Property(i => i.HinhAnh3).HasMaxLength(100);
                o.Property(i => i.CreatedBy).HasMaxLength(200).IsUnicode(false);
                o.Property(i => i.UpdatedBy).HasMaxLength(200).IsUnicode(false);
                o.Property(i => i.CreatedDate).HasColumnType(_smallDateTime);
                o.Property(i => i.UpdatedDate).HasColumnType(_smallDateTime);
                o.HasIndex(i => i.Ngay, "IX_GopY_Ngay");
            });

            modelBuilder.Entity<Models.CauHinhTonKho>(o =>
            {
                o.HasKey(i => i.Id);
                o.HasIndex(i => i.Ngay, "IX_CauHinhTonKho_Ngay");
                o.HasMany<Models.CauHinhTonKhoChiTiet>(s => s.CauHinhTonKhoChiTiets).WithOne(d => d.CauHinhTonKho).HasForeignKey(d => d.CauHinhTonKhoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_CauHinhTonKhoChiTiet_CauHinhTonKhoId");
            });

            modelBuilder.Entity<Models.CauHinhTonKhoChiTiet>(o =>
            {
                o.HasKey(i => i.Id);
                o.HasMany<Models.CauHinhTonKhoChiTietCon>(s => s.CauHinhTonKhoChiTietCons).WithOne(d => d.CauHinhTonKhoChiTiet).HasForeignKey(d => d.CauHinhTonKhoChiTietId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_CauHinhTonKhoChiTietCon_CauHinhTonKhoChiTietId");
            });

            modelBuilder.Entity<Models.CauHinhTonKhoChiTietCon>(o =>
            {
                o.HasKey(i => i.Id);
            });

            modelBuilder.Entity<Models.DMDonViCapMau>(o =>
            {
                o.HasKey(i => i.MaDonVi);
                o.Property(i => i.MaDonVi).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenDonVi).HasMaxLength(200);
                o.Property(i => i.DiaChi).HasMaxLength(200);
                o.Property(i => i.DienThoai).HasMaxLength(50).IsUnicode(false);
                o.HasMany<Models.GiaoDich>(s => s.GiaoDichs).WithOne(d => d.DonViCapMau).HasForeignKey(d => d.MaDonViCapMau).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_GiaoDich_MaDonViCapMau");
            });

            modelBuilder.Entity<Models.GiaoDich>(o =>
            {
                o.HasKey(i => i.GiaoDichId);
                o.Property(i => i.MaDonViCapMau).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.GhiChu).HasMaxLength(200);
                o.Property(i => i.CreatedBy).HasMaxLength(200).IsUnicode(false);
                o.Property(i => i.UpdatedBy).HasMaxLength(200).IsUnicode(false);
                o.Property(i => i.CreatedDate).HasColumnType(_smallDateTime);
                o.Property(i => i.UpdatedDate).HasColumnType(_smallDateTime);
                o.Property(i => i.Ngay).HasColumnType(_smallDateTime);
                o.HasIndex(i => i.Ngay, "IX_GiaoDich_Ngay");
                o.HasMany<Models.GiaoDichChiTiet>(s => s.GiaoDichChiTiets).WithOne(d => d.GiaoDich).HasForeignKey(d => d.GiaoDichId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_GiaoDichChiTiet_GiaoDichId");
            });

            modelBuilder.Entity<Models.GiaoDichChiTiet>(o =>
            {
                o.HasKey(i => i.GiaoDichChiTietId);
                o.Property(i => i.LoaiSanPham).HasMaxLength(50);
                o.Property(i => i.DienGiai).HasMaxLength(100);

                o.HasMany<Models.GiaoDichCon>(s => s.GiaoDichCons).WithOne(d => d.GiaoDichChiTiet).HasForeignKey(d => d.GiaoDichChiTietId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Models.GiaoDichCon>(o =>
            {
                o.HasKey(i => i.Id);
                //o.Property(i => i.ApprovedOn).HasColumnName("ApprovedOn").HasColumnType(_smallDateTime);
            });

            modelBuilder.Entity<Models.DangKyHienMau>(o =>
            {
                o.HasKey(i => i.Id);
                o.Property(i => i.HoVaTen).HasMaxLength(100);
                o.Property(i => i.Ngay).HasColumnType(_smallDateTime);
                o.Property(i => i.CMND).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.MaXa).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenXa).HasMaxLength(100);
                o.Property(i => i.MaHuyen).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenHuyen).HasMaxLength(100);
                o.Property(i => i.MaTinh).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.TenTinh).HasMaxLength(100);
                o.Property(i => i.DiaChiLienLac).HasMaxLength(200);
                o.Property(i => i.DiaChiThuongTru).HasMaxLength(200);
                o.Property(i => i.NgheNghiep).HasMaxLength(100);
                o.Property(i => i.Email).HasMaxLength(100).IsUnicode(false);
                o.Property(i => i.SoDT).HasMaxLength(50).IsUnicode(false);
                o.Property(i => i.CoQuan).HasMaxLength(200);

                o.Property(i => i.CreatedBy).HasMaxLength(50).IsUnicode(false);
                o.Property(i => i.CreatedOn).HasColumnType(_smallDateTime);
            });

            modelBuilder.Entity<Models.SystemUser>(o =>
            {
                o.HasKey(i => i.UserCode);
                o.Property(i => i.UserCode).HasMaxLength(200).IsUnicode(false);
                o.Property(i => i.Name).HasMaxLength(100);
                o.Property(i => i.Password).HasMaxLength(255).IsUnicode(false);
                o.Property(i => i.PhoneNumber).HasMaxLength(15).IsUnicode(false);
                o.Property(i => i.OtpCode).HasMaxLength(6).IsUnicode(false);
                o.Property(i => i.IdCardNr).HasMaxLength(50).IsUnicode(false);
                o.Property(i => i.CreatedOn).HasColumnType(_smallDateTime);
                o.Property(i => i.FireBaseToken).HasMaxLength(4000).IsUnicode(false);
                o.Property(i => i.DeviceId).HasMaxLength(255).IsUnicode(false);

                o.HasIndex(i => new { i.DeviceId, i.CreatedOn });
            });
#pragma warning restore CS8604 // Possible null reference argument.

            modelBuilder.Entity<Models.TraLoiCauHoi>(o =>
            {
                o.HasKey(i => i.Id);
                o.HasMany<Models.TraLoiCauHoiChiTiet>(s => s.TraLoiCauHoiChiTiets).WithOne(d => d.TraLoiCauHoi).HasForeignKey(d => d.TraLoiCauHoiId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_TraLoiCauHoiChiTiet_TraLoiCauHoiId");
                o.HasOne<Models.DangKyHienMau>(s => s.DangKyHienMau).WithOne(d => d.TraLoiCauHoi).HasForeignKey<Models.DangKyHienMau>(d => d.TraLoiCauHoiId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_DangKyHienMau_TraLoiCauHoiId");
            });

            modelBuilder.Entity<Models.TraLoiCauHoiChiTiet>(o =>
            {
                o.HasKey(i => i.Id);
            });

            modelBuilder.Entity<Models.SystemConfig>(o =>
            {
                o.HasKey(i => i.Key);
                o.Property(i => i.Value).HasMaxLength(200);
                o.Property(i => i.Note).HasMaxLength(200);
            });

            modelBuilder.Entity<Models.SystemSlide>(o =>
            {
                o.HasKey(i => i.Id);
                o.Property(i => i.Url).HasMaxLength(100);
                o.Property(i => i.Path).HasMaxLength(100);
            });

            modelBuilder.Entity<Models.SurveyQuestion>(o =>
            {
                o.HasKey(i => i.Id);
                o.Property(i => i.Code).HasMaxLength(6);
                o.Property(i => i.Content).HasColumnType(ColumnType.NTEXT);
                o.Property(i => i.IsEnabled).HasDefaultValue(true);
            });
            modelBuilder.Entity<Models.SurveyQuestion>().HasIndex(i => new { i.Code, i.Year }).IsUnique();

            #region Tin tức (News)
            modelBuilder.Entity<Models.TinTuc>(o =>
            {
                o.HasKey(i => i.TinTucId);
                o.Property(i => i.TieuDe).HasMaxLength(200);
                o.Property(i => i.NoiDung1).HasMaxLength(4000);
                o.Property(i => i.NoiDung2).HasMaxLength(4000);
                o.Property(i => i.ThumbnailLink).HasMaxLength(200).IsRequired().IsUnicode(false);
                o.Property(i => i.CreatedBy).HasMaxLength(50).IsUnicode(false);
            });
            #endregion

            modelBuilder.Entity<Models.CapMatKhau>(o =>
            {
                o.HasKey(i => i.Id);
                o.Property(i => i.HoTen).HasMaxLength(100);
                o.Property(i => i.SoDT).HasMaxLength(50).IsUnicode(false);
                o.Property(i => i.CMND).HasMaxLength(20).IsUnicode(false);
                o.Property(i => i.DeviceId).HasMaxLength(100).IsUnicode(false);
                o.Property(i => i.ResetPasswordOn).HasColumnType(_smallDateTime);
            });
        }

        public DbSet<Models.DotLayMau> DotLayMau { get; set; }
        public DbSet<Models.DMNguoiHienMau> DMNguoiHienMau { get; set; }
        public DbSet<Models.LichSuHienMau> LichSuHienMau { get; set; }
        public DbSet<Models.DMTinh> DMTinh { get; set; }
        public DbSet<Models.DMHuyen> DMHuyen { get; set; }
        public DbSet<Models.DMXa> DMXa { get; set; }
        public DbSet<Models.GopY> GopY { get; set; }
        public DbSet<Models.CauHinhTonKho> CauHinhTonKho { get; set; }
        public DbSet<Models.DMDonViCapMau> DMDonViCapMau { get; set; }
        public DbSet<Models.GiaoDich> GiaoDich { get; set; }
        public DbSet<Models.GiaoDichChiTiet> GiaoDichChiTiet { get; set; }
        public DbSet<Models.DangKyHienMau> DangKyHienMau { get; set; }
        public DbSet<Models.SystemUser> SystemUser { get; set; }
        public DbSet<Models.TraLoiCauHoi> TraLoiCauHoi { get; set; }
        public DbSet<Models.TraLoiCauHoiChiTiet> TraLoiCauHoiChiTiet { get; set; }
        public DbSet<Models.SystemConfig> SystemConfig { get; set; }
        public DbSet<Models.SystemSlide> SystemSlide { get; set; }
        public DbSet<Models.SurveyQuestion> SurveyQuestion { get; set; }

        public DbSet<Models.TinTuc> TinTuc { get; set; }

        public DbSet<Models.CapMatKhau> CapMatKhaus { get; set; }

        public DbSet<Models.CauHinhTonKhoChiTiet> CauHinhTonKhoChiTiet { get; set; }
        public DbSet<Models.CauHinhTonKhoChiTietCon> CauHinhTonKhoChiTietCon { get; set; }
    }
}
