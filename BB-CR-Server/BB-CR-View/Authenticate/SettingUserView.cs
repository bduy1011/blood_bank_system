namespace BB.CR.Views.Authenticate
{
    public class SettingUserView
    {
        public string? HoTen { get; set; }
        public int? NamSinh { get; set; }
        public string? SoDT { get; set; }

        /// <summary>
        /// Input full character CMND
        /// </summary>
        public string? CMND { get; set; }
        /// <summary>
        /// MAC Address of device
        /// </summary>
        public string? MACAddress { get; set; }
    }
}
