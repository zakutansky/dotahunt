using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mushi.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// To the pay pal format.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static string ToPayPalFormat(this string amount)
        {
            var a = Math.Round(Convert.ToDecimal(amount), 2).ToString();
            var result = a.Replace(",", ".");
            return result;
        }
    }
}