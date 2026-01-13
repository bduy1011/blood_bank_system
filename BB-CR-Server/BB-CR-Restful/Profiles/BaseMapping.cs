namespace BB.CR.Rest.Profiles
{
    public static class BaseMapping
    {
        public static void ExecuteAllMapping()
        {
            DMTinhAdapt.ConfigureMapping();
            DMHuyenAdapt.ConfigureMapping();
            DMXaAdapt.ConfigureMapping();
            DangKyHienMauAdapt.ConfigureMapping();
            DMDonViCapMauAdapt.ConfigureMapping();
            SystemUserAdapt.ConfigureMapping();
            SurveyQuestionAdapt.ConfigureMapping();
            TraLoiCauHoiAdapt.ConfigureMapping();
            TraLoiCauHoiChiTietAdapt.ConfigureMapping();
            LichSuHienMauAdapt.ConfigureMapping();
            DMNguoiHienMauAdapt.ConfigureMapping();

            GiaoDichAdapt.ConfigureMapping();
            GiaoDichChiTietAdapt.ConfigureMapping();
            GiaoDichConAdapt.ConfigureMapping();

            CauHinhTonKhoAdapt.ConfigureMapping();
        }
    }
}
