using System;
namespace SmearAdmin.Helpers
{
    public class Utilities
    {
        public static DateTime FormatDateTimeByZone(DateTime inputDate)
        {
            if(inputDate != null)
            {
                //TimeZoneInfo tz = TimeZoneInfo.Local;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                inputDate = TimeZoneInfo.ConvertTimeFromUtc(inputDate, tz);
            }
            return inputDate;
        }
    }
}
