namespace Bussiness_Manager.Utility
{
    public static class indiaTimeZone
    {
        public static DateTime DateTimeIndia()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indiaTimeZone);
            return localTime;
        }
    }
}
