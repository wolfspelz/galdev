using System;

namespace n3q.Tools
{
    public static class DateTimeExtensions
    {
        public static DateTime MinDataTime(this DateTime self)
        {
            return new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public static long TicksOfSecond(this DateTime self)
        {
            return self.Ticks % 10_000_000;
        }
    }
}
