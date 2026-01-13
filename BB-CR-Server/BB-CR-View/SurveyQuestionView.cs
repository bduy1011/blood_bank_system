using BB.CR.Providers.Bases;

namespace BB.CR.Views
{
    public class SurveyQuestionView
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public SurveyQuestionAttribute? Attribute { get; set; }

        public bool? No { get; set; }
        public bool? Yes { get; set; }
        public DateTime? OnDate { get; set; }
        public string? Notes { get; set; }

        public long? TraLoiCauHoiId { get; set; }
        public bool? MaleSkip { get; set; }
    }
}
