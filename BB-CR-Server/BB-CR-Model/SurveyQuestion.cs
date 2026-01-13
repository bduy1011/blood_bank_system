using BB.CR.Providers.Bases;

namespace BB.CR.Models
{
    public class SurveyQuestion
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required int Year { get; set; }
        public required string Content { get; set; }
        public bool? IsEnabled { get; set; }
        public byte? OrderIndex { get; set; }
        public SurveyQuestionAttribute? Attribute { get; set; }
        public bool? MaleSkip { get; set; }
    }
}
