using System;
using System.Globalization;

namespace n3q.Tools
{
    public static class IntervalDate
    {
        public static DateTime StartOfDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind);
        }

        public static DateTime StartOfWeek(DateTime dt)
        {
            var days = (int)dt.DayOfWeek -1;
            days = days < 0 ? days + 7 : days;
            var startOfWeek = dt.AddDays(-days);
            return new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day, 0, 0, 0, dt.Kind);
        }

        public static DateTime StartOfMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind);
        }

        public static DateTime StartOfYear(DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1, 0, 0, 0, dt.Kind);
        }

        public static string FormatAsDay(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string FormatAsWeek(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string FormatAsMonth(DateTime dt)
        {
            return dt.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        }

        public static string FormatAsYear(DateTime dt)
        {
            return dt.ToString("yyyy", CultureInfo.InvariantCulture);
        }

        public static string DayName(DateTime dt)
        {
            return dt.DayOfWeek.ToString();
        }

        public static string FormatStartOfDay(DateTime dt)
        {
            return FormatAsDay(new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind));
        }

        public static string FormatStartOfWeek(DateTime dt)
        {
            var days = (int)dt.DayOfWeek -1;
            days = days < 0 ? days + 7 : days;
            var startOfWeek = dt.AddDays(-days);
            return FormatAsWeek(new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day, 0, 0, 0, dt.Kind));
        }

        public static string FormatStartOfMonth(DateTime dt)
        {
            return FormatAsMonth(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind));
        }

        public static string FormatStartOfYear(DateTime dt)
        {
            return FormatAsYear(new DateTime(dt.Year, 1, 1, 0, 0, 0, dt.Kind));
        }

        public static string MonthName(DateTime dt)
        {
            return dt.ToString("MMMM", CultureInfo.InvariantCulture);
        }
    }
}
