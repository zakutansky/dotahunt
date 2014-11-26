using MushiDataTypes;
using MushiDataTypes.Enums;
using PortableSteam;
using System.Linq;

namespace Steam
{
    public class SteamContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamContext"/> class.
        /// </summary>
        public SteamContext()
        {
            SteamWebAPI.SetGlobalKey("C5CB43665FEF395BBE191CAA264F8947");
        }

        /// <summary>
        /// Gets the player steam information.
        /// </summary>
        /// <param name="steamId">The steam identifier.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <returns></returns>
        public Player GetPlayerSteamInfo(long steamId, SteamRequestsEnum requestType)
        {
            var response = SteamWebAPI.General()
                            .ISteamUser()
                            .GetPlayerSummaries(GetSteamIdentity(steamId))
                            .GetResponse();
            if (response.Data == null) return null;
            var player = response.Data
                .Players
                .FirstOrDefault();
            RequestCounter.Increment(requestType);
            if (player != null)
            {
                return new Player
                {
                    NickName = player.PersonaName,
                    AvatarUrl = player.AvatarMedium,
                };
            }
            return null;
        }

        /// <summary>
        /// Determines whether [has exposed public match data] [the specified steam identifier].
        /// </summary>
        /// <param name="steamId">The steam identifier.</param>
        /// <returns></returns>
        public bool IsEnabledPublicMatchData(long steamId, SteamRequestsEnum requestType)
        {
            var matchHistory = SteamWebAPI.Game()
                .Dota2()
                .IDOTA2Match()
                .GetMatchHistory()
                .MatchesRequested(1)
                .Account(GetSteamIdentity(steamId))
                .GetResponse();
            RequestCounter.Increment(requestType);
            if (matchHistory.Data == null) return false;
            return matchHistory.Data.Status != GetMatchHistoryResponseStatus.Private;
        }


        public void Test()
        {
            var response = SteamWebAPI.General()
                .ISteamUser()
                .GetPlayerSummaries(GetSteamIdentity(76561198030654385))
                .GetResponse();
            var result = GetSteamIdentity(76561198030654385);
        }

        /// <summary>
        /// Gets the steam identity.
        /// </summary>
        /// <param name="steamId">The steam identifier.</param>
        /// <returns></returns>
        private SteamIdentity GetSteamIdentity(long steamId)
        {
            return SteamIdentity.FromSteamID(steamId);
        }
    }
}
