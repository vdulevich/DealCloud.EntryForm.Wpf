using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime EXCEL_MIN_DATE = new DateTime(1900, 3, 1);
        private static readonly double EXCEL_MIN_OA_DATE = EXCEL_MIN_DATE.ToOADate();

        /// <summary>
        /// Number of minutes since 1899/12/31
        /// </summary>
        public static int ToIntDateTime(this DateTime date)
        {
            return (int)Math.Floor((date - Constants.MIN_DATE).TotalMinutes);
        }

        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        public static DateTime FromIntDateTime(this int dt)
        {
            return Constants.MIN_DATE.AddMinutes(dt);
        }

        public static double ToExcelDate(this DateTime date)
        {
            double serialDate = date.ToOADate();
            if (date < EXCEL_MIN_DATE)
                --serialDate;

            return serialDate;
        }

        public static DateTime FromExcelDate(this double excelDate)
        {
            if (excelDate < EXCEL_MIN_OA_DATE)
                ++excelDate;

            return DateTime.FromOADate(excelDate);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static string ToSortableString(this DateTime dt)
        {
            return dt.ToString("s");
        }
    }
}
