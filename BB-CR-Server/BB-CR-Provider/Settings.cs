namespace BB.CR.Providers
{
    public static class Settings
    {
        private static string? connectionString;
        private static string? keySecurity;

        public static string? ConnectionString { get => connectionString; set => connectionString = value; }
        public static string? KeySecurity { get => keySecurity; set => keySecurity = value; }

        public static string? PathTemp { get; set; }
        public static string? PathActual { get; set; }

        public static bool BreakGenerate { get; set; }

        public static int? ExpireTimeOtp { get; set; }

        public static bool DeleteAccount { get; set; }
        public static int DelayTime { get; set; }

        public static HospitalInformation? Hospital { get; set; }

        /// <summary>
        /// Kiosk devices (ID).
        /// </summary>
        public static List<string>? KioskDevices { get; set; }
    }

    public class HospitalInformation 
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
