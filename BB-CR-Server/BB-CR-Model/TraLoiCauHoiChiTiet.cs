namespace BB.CR.Models
{
    public class TraLoiCauHoiChiTiet
    {
        public long Id { get; set; }

        public int? SurveyQuestionId { get; set; }
        public bool? YesAnswer { get; set; }
        public bool? NoAnswer { get; set; }

        /// <summary>
        /// Ngày chọn câu số 1
        /// </summary>
        public DateTime? OnDate { get; set; }
        public string? GhiChu { get; set; }

        public long? TraLoiCauHoiId { get; set; }
        public TraLoiCauHoi? TraLoiCauHoi { get; set; }
    }
}
