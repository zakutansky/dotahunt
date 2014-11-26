using System.Text.RegularExpressions;

namespace Steam
{
    public static class SteamHelpers
    {
        /// <summary>
        /// The open identifier regex
        /// </summary>
        private static readonly Regex OpenIdRegex = new Regex(@"^http://steamcommunity\.com/openid/id/(7[0-9]{15,25})$", RegexOptions.Compiled);

        /// <summary>
        /// The dotabuff identifier regex
        /// </summary>
        private static readonly Regex DotabuffIdRegex = new Regex(@"^http://steamcommunity\.com/profiles/(7[0-9]{15,25})$", RegexOptions.Compiled);

        /// <summary>
        /// Gets the steam identifier from open identifier.
        /// </summary>
        /// <param name="claimedId">The claimed identifier.</param>
        /// <returns></returns>
        public static string GetSteamIdFromOpenId(string claimedId)
        {
            var accountIdMatch = OpenIdRegex.Match(claimedId);
            if (accountIdMatch.Success)
            {
                return accountIdMatch.Groups[1].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the steam identifier from dotabuff.
        /// </summary>
        /// <param name="claimedId">The claimed identifier.</param>
        /// <returns></returns>
        public static string GetSteamIdFromDotabuff(string claimedId)
        {
            var accountIdMatch = DotabuffIdRegex.Match(claimedId);
            if (accountIdMatch.Success)
            {
                return accountIdMatch.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}
