using System;

namespace n3q.Tools
{
    public static class LongDateTimeExtensions
    {
        public static DateTime ToDateTime(this Int64 nDate)
        {
            return LongDateTime.FromLong(nDate);
        }

        public static long ToLong(this DateTime date)
        {
            return LongDateTime.ToLong(date);
        }
    }

    public static class LongDateTime
    {
        public const long Epoch = 100000000000000000L;
        public const long Year = 10000000000000L;
        public const long Month = 100000000000L;
        public const long Day = 1000000000L;
        public const long Hour = 10000000L;
        public const long Minute = 100000L;
        public const long Second = 1000L;
        public const long Millisecond = 1L;

        public static long ToLong(DateTime date)
        {
            if (date == DateTime.MinValue) {
                return 0;
            }

            const long nEpoch = Epoch * 1;
            long nYear = Year * date.Year;
            long nMonth = Month * date.Month;
            long nDay = Day * date.Day;
            long nHour = Hour * date.Hour;
            long nMinute = Minute * date.Minute;
            long nSecond = Second * date.Second;
            long nMillisecond = Millisecond * date.Millisecond;

            long nDate = nEpoch + nYear + nMonth + nDay + nHour + nMinute + nSecond + nMillisecond;

            return nDate;
        }

        public static DateTime FromLong(long nDate)
        {
            if (nDate == 0L) {
                return DateTime.MinValue;
            }

            if (nDate >= Epoch && nDate < 2 * Epoch) {
                nDate -= Epoch;

                long nMilisecond = nDate % 1000L;
                nDate = (nDate - nMilisecond) / 1000L;
                long nSecond = nDate % 100L;
                nDate = (nDate - nSecond) / 100L;
                long nMinute = nDate % 100L;
                nDate = (nDate - nMinute) / 100L;
                long nHour = nDate % 100L;
                nDate = (nDate - nHour) / 100L;
                long nDay = nDate % 100L;
                nDate = (nDate - nDay) / 100L;
                long nMonth = nDate % 100L;
                nDate = (nDate - nMonth) / 100L;
                long nYear = nDate % 10000L;
                _ = (nDate - nYear) / 10000L;

                DateTime date;
                if (nYear == 0) {
                    date = DateTime.MinValue;
                } else {
                    date = new DateTime(Convert.ToInt32(nYear), Convert.ToInt32(nMonth), Convert.ToInt32(nDay), Convert.ToInt32(nHour), Convert.ToInt32(nMinute), Convert.ToInt32(nSecond), Convert.ToInt32(nMilisecond));
                }

                return date;
            } else {
                throw new NotImplementedException($"{nDate}: Epoch != 1");
            }
        }
    }
}
