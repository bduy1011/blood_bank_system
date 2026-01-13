using System.ComponentModel;

namespace BB.CR.Providers.Bases
{
    public enum TinhTrangDangKyHienMau : byte
    {
        [Description("Đã đăng ký")]
        DaDangKy = 1,
        [Description("Huỷ")]
        Huy = 2,
        [Description("Đã tiếp nhận")]
        DaTiepNhan = 3
    }

    /// <summary>
    /// Nhóm máu.
    /// </summary>
    public enum NhomMau
    {
        [Description("A Rh-")]
        Atru = 0600,
        [Description("A Rh+")]
        Acong = 6200,
        [Description("B Rh-")]
        Btru = 1700,
        [Description("B Rh+")]
        Bcong = 7300,
        [Description("AB Rh-")]
        ABtru = 2800,
        [Description("AB Rh+")]
        ABcong = 8400,
        [Description("O Rh-")]
        Otru = 9500,
        [Description("O Rh+")]
        Ocong = 5100,
        [Description("Chưa xác định")]
        KXD = -1
    }

    /// <summary>
    /// Loại sản phẩmm
    /// </summary>
    public enum LoaiSanPham : byte
    {
        //[Description("Khác")]
        //Others = 0,
        [Description("Khối bạch cầu")]
        BachCau = 1,
        [Description("Huyết tương tươi đông lạnh")]
        Ffp = 2,
        [Description("Khối hồng cầu lắng")]
        HongCauLang = 3,
        [Description("Khối hồng cầu rửa")]
        HongCauRua = 4,
        //[Description("Máu toàn phần")]
        //MauToanPhan = 5,
        //[Description("Huyết tương đông lạnh")]
        //PlasmaLt = 6,
        //[Description("Tiểu cầu")]
        //TieuCau = 7,
        [Description("Khối tiểu cầu apheresis")]
        TieuCauApheresis = 8,
        [Description("Yếu tố VIII")]
        YeuToViii = 9,
        [Description("Khối hồng cầu Phenotype")]
        Phenotype = 12
    }

    /// <summary>
    /// Tình trạng giao dịch
    /// </summary>
    public enum TinhTrangGiaoDich : short
    {
        [Description("Chờ xác nhận")]
        ChoXacNhan = 1,
        [Description("Duyệt một phần")]
        DuyetMotPhan = 2,
        [Description("Đã duyệt")]
        DaDuyet = 3,
        [Description("Huỷ")]
        Huy = 4,
        [Description("Từ chối")]
        TuChoi = 5
    }

    /// <summary>
    /// Tình trạng lịch lấy máu
    /// </summary>
    public enum TinhTrangDotLayMau : byte
    {
        [Description("Tạo đợt mới")]
        TaoDotMoi = 10,
        [Description("Đã chốt")]
        DaChot = 20,
        [Description("Huỷ")]
        Huy = 100
    }

    /// <summary>
    /// Kết quả xét nghiệm
    /// </summary>
    public enum KetQuaXetNghiem : byte
    {
        [Description("Chưa xét nghiệm")]
        CHUA_XET_NGHIEM = 10,
        [Description("Không phản ứng")]
        AM_TINH = 20,
        [Description("Phản ứng")]
        DUONG_TINH = 30,
        [Description("Phản ứng")]
        CHUA_XAC_DINH = 40,
        [Description("Chờ kết quả")]
        CHO_KET_QUA = 90
    }

    /// <summary>
    /// Loại đợt lấy máu
    /// </summary>
    public enum LoaiDotLayMau : byte
    {
        [Description("Lưu động")]
        LuuDong = 10,
        [Description("Tại chỗ")]
        TaiCho = 20
    }

    /// <summary>
    /// Loại máu đợt lấy máu
    /// </summary>
    public enum LoaiMauDotLayMau : byte
    {
        [Description("Mẫu máu")]
        MauMau = 10,
        [Description("Tiểu cầu")]
        TieuCau = 20
    }

    public enum AppRole : byte
    {
        [Description("Admin")]
        Admin = 10,
        [Description("Quản lý")]
        Manager = 20,
        [Description("User thường / người hiến máu")]
        User = 30,
        [Description("Đăng ký mua máu")]
        DangKyMuaMau = 40,
    }

    public enum SurveyQuestionAttribute : byte
    {
        InputDate = 1,
        InputText = 2,
    }

    public enum LoaiPhieu : byte
    {
        [Description("Phiếu nhượng máu")]
        PhieuNhuongMau = 1,
    }

    public enum LoaiTinTuc : byte
    {
        [Description("Mới")]
        Moi = 1,
        [Description("Quan trọng")]
        QuanTrong = 2
    }

    public enum ModuleTinTuc : byte
    {
        [Description("Trung tâm")]
        TrungTam = 1,
        [Description("Bệnh viện")]
        BenhVien = 2
    }
}
