namespace BB.CR.Models
{
    public class DMHuyen
    {
        public required string MaHuyen { get; set; }
        public string? MaQuocGia { get; set; }
        public required string MaTinh { get; set; }
        public required string Ten { get; set; }
        public required bool Active { get; set; }

        public DMTinh? DMTinh { get; set; }
        public List<DMXa>? DMXas { get; set; }
    }
}
