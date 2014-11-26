using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mushi.Helper
{
    public class GuidHelper
    {
        /// <summary>
        /// Gets the room key.
        /// </summary>
        /// <returns></returns>
        public static string GetNoDashGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Converts to no dash unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static string ConvertToNoDashGuid(Guid guid)
        {
            return guid.ToString("N");
        }
    }
}