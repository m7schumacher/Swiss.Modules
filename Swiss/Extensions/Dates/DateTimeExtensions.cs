using System;

namespace Swiss
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Method prunes the resolution of a DateTime object
        /// </summary>
        public static DateTime Truncate(this DateTime time, long resolution)
        {
            return new DateTime(time.Ticks - (time.Ticks % resolution), time.Kind);
        }

        /// <summary>
        /// Method gets the number of milliseconds between this date and another
        /// </summary>
        public static double MillisecondsBetween(this DateTime start, DateTime end)
        {
            return (end - start).TotalMilliseconds;
        }

        /// <summary>
        /// Method gets the number of seconds between this date and another
        /// </summary>
        public static double SecondsBetween(this DateTime start, DateTime end)
        {
            return (end - start).TotalSeconds;
        }

        /// <summary>
        /// Method gets the number of hours between this date and another
        /// </summary>
        public static double HoursBetween(this DateTime start, DateTime end)
        {
            return (end - start).TotalHours;
        }

        /// <summary>
        /// Method gets the number of days between this date and another
        /// </summary>
        public static double DaysBetween(this DateTime start, DateTime end)
        {
            return (end - start).TotalDays;
        }

        /// <summary>
        /// Method converts seconds since epoch to a DateTime object
        /// </summary>
        private static DateTime ConvertSecondsSinceEpochToTime(long seconds)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime time = epoch.AddSeconds(seconds);

            return time;
        }
    }
}
