using System;
using System.Text;
using System.Threading;
using MushiDataTypes;
using MushiDataTypes.Enums;
using MushiDb.Services;

namespace Steam
{
    public static class RequestCounter
    {
        /// <summary>
        /// The _sign up
        /// </summary>
        private static int _signUp;

        /// <summary>
        /// The _sign in
        /// </summary>
        private static int _signIn;

        /// <summary>
        /// The _pro load
        /// </summary>
        private static int _proLoad;

        /// <summary>
        /// The _hire a pro
        /// </summary>
        private static int _hireAPro;

        /// <summary>
        /// The _pre game
        /// </summary>
        private static int _preGame;

        /// <summary>
        /// The _game check
        /// </summary>
        private static int _gameCheck;

        /// <summary>
        /// The _day
        /// </summary>
        private static DateTime _day;

        /// <summary>
        /// The _timer
        /// </summary>
        private static Timer _timer;

        /// <summary>
        /// Increments the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        internal static void Increment(SteamRequestsEnum type)
        {
            switch (type)
            {
                case SteamRequestsEnum.SignIn:
                    Interlocked.Increment(ref _signIn);
                    break;
                case SteamRequestsEnum.SignUp:
                    Interlocked.Increment(ref _signUp);
                    break;
                case SteamRequestsEnum.ProLoad:
                    Interlocked.Increment(ref _proLoad);
                    break;
                case SteamRequestsEnum.HireAPro:
                    Interlocked.Increment(ref _hireAPro);
                    break;
                case SteamRequestsEnum.GameCheck:
                    Interlocked.Increment(ref _gameCheck);
                    break;
                case SteamRequestsEnum.PreGame:
                    Interlocked.Increment(ref _preGame);
                    break;
            }
        }

        /// <summary>
        /// Runs the timer.
        /// </summary>
        public static void Run()
        {
            _day = DateTime.Now.Date;
            _timer = new Timer(SaveCurrentResults, null, 10800000, Timeout.Infinite);
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public static void Flush()
        {
            SaveCurrentResults(null);
        }

        /// <summary>
        /// Saves the current results.
        /// </summary>
        /// <param name="o">The o.</param>
        private static void SaveCurrentResults(object o)
        {
            var result = new SteamRequestResult
            {
                Day = _day,
                SignUp = _signUp,
                SignIn = _signIn,
                ProLoad = _proLoad,
                HireAPro = _hireAPro,
                PreGame = _preGame,
                GameCheck = _gameCheck
            };
            using (var service = new HireProService())
            {
                var isChange = service.UpdateSteamRequest(result);
                if (!isChange)
                    service.CreateSteamRequest(result);
            }
            Run();
        }

        public static string GetCurrentResult()
        {
            var result = new StringBuilder();
            result.Append(string.Format("SignUp: {0}", _signUp));
            result.Append(Environment.NewLine);
            result.Append(string.Format("SignIn: {0}", _signIn));
            result.Append(Environment.NewLine);
            result.Append(string.Format("Pro Page Load: {0}", _proLoad));
            result.Append(Environment.NewLine);
            result.Append(string.Format("Hire a Pro: {0}", _hireAPro));
            result.Append(string.Format("After payment(pre game): {0}", _preGame));
            result.Append(Environment.NewLine);
            result.Append(string.Format("Game check: {0}", _gameCheck));
            result.Append(Environment.NewLine);
            result.Append(Environment.NewLine);
            result.Append(string.Format("TOTAL: {0}", _signUp + _signIn + _proLoad + _hireAPro + _preGame + _gameCheck));

            return result.ToString();
        }
    }
}
