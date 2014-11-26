using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Mushi.Helper
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Gets the startDate from string.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static DateTime? GetDateFromString(string time, string format)
        {
            DateTime d;
            if(DateTime.TryParseExact(time, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            {
                return d;
            }
            return null;
        }

        /// <summary>
        /// Sets the hour for date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static DateTime SetHourForDate(DateTime date, int hours)
        {
            var t = new TimeSpan(hours, 0, 0);
            return date.Date + t;
        }
    }
}