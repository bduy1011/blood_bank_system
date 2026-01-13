namespace BB.CR.Models
{
    public class DMXa
    {
        public required string MaXa { get; set; }
        public string? MaQuocGia { get; set; }
        public required string MaHuyen { get; set; }
        public required string Ten { get; set; }
        public required bool Active { get; set; }

        public DMHuyen? DMHuyen { get; set; }
    }
}
