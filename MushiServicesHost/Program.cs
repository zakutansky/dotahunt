using System.Threading;
using Notifications;
using System;

namespace MushiServicesHost
{
    internal class Program
    {
        /// <summary>
        /// The _day
        /// </summary>
        private static DateTime _day;

        /// <summary>
        /// The _timer
        /// </summary>
        private static Timer _timer;

        private static void Main(string[] args)
        {
            try
            {
                new AvatarUpdater().Run();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += Environment.NewLine;
                    msg += Environment.NewLine;
                    msg += ex.InnerException.Message;
                }
                Console.WriteLine(msg); 
                Email.SendEmail(
                    "Mushi - Dotabuff service, internal error",
                    ex.Message,
                    "adam.zakutansky@seznam.cz",
                    "csepcsar@gmail.com"
                    );
            }
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
