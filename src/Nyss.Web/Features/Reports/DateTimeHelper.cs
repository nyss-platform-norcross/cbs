using System;
using System.Globalization;

namespace Nyss.Web.Features.Reports
{
    public static class DateTimeHelperExtensions
    {
        public static int GetIsoWeek(this DateTime dateTime)
        {
            if (dateTime.Month == 12 && dateTime.Day > 28)
            {
                var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dateTime);
                if (day >= DayOfWeek.Sunday && day <= DayOfWeek.Tuesday)
                {
                    dateTime = dateTime.AddDays(3);
                }
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }
    }
}
