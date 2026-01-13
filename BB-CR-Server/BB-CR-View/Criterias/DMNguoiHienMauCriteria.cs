namespace BB.CR.Views.Criterias
{
    public class DMNguoiHienMauCriteria : BaseCriteria
    {
        public List<int>? NguoiHienMauIds { get; set; }
        public string? HoTen { get; set; }
        public int? NamSinh { get; set; }
        public string? CMND { get; set; }
        public string? Xa { get; set; }
        public string? Huyen { get; set; }
        public string? Tinh { get; set; }
        public string? SoDT { get; set; }
        public string? Email { get; set; }

        /// <summary>
        /// Phục vụ cho local
        /// </summary>
        public DateTime? UpdatedTime { get; set; }
    }
}
