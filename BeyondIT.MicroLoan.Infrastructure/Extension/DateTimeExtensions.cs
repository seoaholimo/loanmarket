using System;

namespace MoiFleet.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToZaLongDateString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy");
        }

        public static string ToZaLongDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? ToZaLongDateString(dateTime.Value) : string.Empty;
        }

        public static string ToZaLongDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy HH:mm");
        }

        public static string ToIsoCompleteDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToIsoCompleteDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? ToIsoCompleteDateString(dateTime.Value) : string.Empty;
        }

        public static string ToIsoCompleteDateWithTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static string ToIsoCompleteDateWithTimeString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? ToIsoCompleteDateWithTimeString(dateTime.Value) : string.Empty;
        }

        public static DateTime ToMidNightHour(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static DateTime ToLastDayHour(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        public static DateTime? ToLastDayHour(this DateTime? dateTime)
        {
            return dateTime.HasValue ? ToLastDayHour(dateTime.Value) : new DateTime?();
        }

        public static DateTime ToFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime ToLastDayOfMonth(this DateTime dateTime)
        {
            int lastDay = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year, dateTime.Month, lastDay).ToLastDayHour();
        }

        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsCurrentMonth(this DateTime dateTime)
        {
            return dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month;
        }

        public static bool IsToday(this DateTime dateTime)
        {
            return dateTime.Year == DateTime.Now.Year &&
                   dateTime.Month == DateTime.Now.Month &&
                   dateTime.Day == DateTime.Now.Day;
        }

        public static bool IsDefault(this DateTime dateTime)
        {
            return dateTime == default(DateTime);
        }

        public static bool IsNullOrDefault(this DateTime? dateTime)
        {
            return !dateTime.HasValue || IsDefault(dateTime.Value);
        }

        public static bool IsSameDayAs(this DateTime dateTime, DateTime comparedDateTime)
        {
            return dateTime.Year == comparedDateTime.Year &&
                   dateTime.Month == comparedDateTime.Month &&
                   dateTime.Day == comparedDateTime.Day;
        }

        public static bool IsMidNightHour(this DateTime dateTime)
        {
            return dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Minute == 0;
        }

    }
}