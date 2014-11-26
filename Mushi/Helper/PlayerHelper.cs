using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mushi.Context;
using MushiDataTypes;
using MushiDataTypes.Enums;
using Steam;


namespace Mushi.Helper
{
    public class PlayerHelper
    {
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="requestType">Type of the request.</param>
        public static void UpdateAvatar(Player player, SteamRequestsEnum requestType )
        {
            if (player == null) return;
            long steamId;
            if (long.TryParse(player.SteamId, out steamId))
            {
                var steamUser = new SteamContext().GetPlayerSteamInfo(steamId, requestType);
                if (steamUser != null)
                {
                    if (player.PlayerType == PlayerTypesEnum.Amateur)
                    {
                        player.NickName = steamUser.NickName;
                        player.AvatarUrl = steamUser.AvatarUrl;
                        new PlayerContext().UpdatePlayerInfo(player.PlayerId, steamUser.AvatarUrl, steamUser.NickName);
                    }
                    else
                    {
                        player.AvatarUrl = steamUser.AvatarUrl;
                        new PlayerContext().UpdatePlayerInfo(player.PlayerId, steamUser.AvatarUrl);
                    }
                }
            }
        }
    }
}