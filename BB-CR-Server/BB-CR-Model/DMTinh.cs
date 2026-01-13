namespace BB.CR.Models
{
    public class DMTinh
    {
        public required string MaTinh { get; set; }
        public string? MaQuocGia { get; set; }
        public required string Ten { get; set; }
        public required short UuTien { get; set; }
        public required bool Active { get; set; }

        public List<DMHuyen>? DMHuyens { get; set; }
    }
}
