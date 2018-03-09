#region Imports

using System;

#endregion

namespace TodoApi.Utils.TimeUtils
{
    /// <summary>
    /// A static class for bulding dates.
    /// </summary>
    public static class QueryDateBuilder
    {
        /// <summary>
        /// Tries to convert given parameters to a DateTime instance.
        /// </summary>
        /// <param name="year">A year as a string</param>
        /// <param name="month">A month as a string</param>
        /// <param name="day">A day as a string</param>
        /// <returns>The corresponding nullable DateTime if succesful, null otherwise</returns>
        public static DateTime? CreateDate(string year, string month, string day)
        {
            if (!int.TryParse(year, out var y) ||
                !int.TryParse(month, out var m) ||
                !int.TryParse(day, out var d)) return null;
            try
            {
                return new DateTime(y, m, d);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
