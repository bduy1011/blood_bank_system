using BB.CR.Providers.Bases;

namespace BB.CR.Providers
{
    public class Helpers
    {
        public static string RandomOTP(int length = 6)
        {
            var numbers = new int[length];
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                numbers[i] = random.Next(0, 9);
            }

            string otpCode = string.Join(string.Empty, numbers);
            return otpCode!;
        }

        public static string GetThoiGianGapMat(DateTime? ngayThu)
        {
            if (ngayThu is null)
                return string.Empty;

            string thoiGianGapMat = (object)ngayThu.Value.AddMonths(1).Date.DayOfWeek switch
            {
                DayOfWeek.Sunday => ngayThu.Value.AddMonths(1).AddDays(1).ToString(DateFormatter.DDMMYYYY_VN),
                DayOfWeek.Saturday => ngayThu.Value.AddMonths(1).AddDays(2).ToString(DateFormatter.DDMMYYYY_VN),
                _ => ngayThu.Value.AddMonths(1).ToString(DateFormatter.DDMMYYYY_VN),
            };

            return thoiGianGapMat;
        }
    }
}
