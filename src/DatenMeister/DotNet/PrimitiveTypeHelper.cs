using System;

namespace DatenMeister.DotNet
{
    public static class PrimitiveTypeHelper
    {
        /// <summary>
        /// Truncates the given date time to the given timespan
        /// </summary>
        /// <param name="dateTime">The value to be truncated</param>
        /// <param name="timeSpan">The resolution for truncation</param>
        /// <returns>The truncated date time</returns>
        public static DateTime Truncate(DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime; // do not modify "guard" values
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static DateTime TruncateToSecond(DateTime dateTime)
        {
            return Truncate(dateTime, TimeSpan.FromSeconds(1));
        }

        public static DateTime TruncateToMinute(DateTime dateTime)
        {
            return Truncate(dateTime, TimeSpan.FromMinutes(1));
        }
    }
}