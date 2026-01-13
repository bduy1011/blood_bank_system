namespace BB.CR.Views
{
    public class DangKyHienMauView : Models.DangKyHienMau
    {
        public string? TinhTrangDescription { get; set; }
        public List<SurveyQuestionView>? SurveyQuestions { get; set; }
    }
}
