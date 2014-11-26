using System;
using MushiDataTypes.Enums;
using MushiServices.Context;
using MushiServices.DbServices;
using Steam;
using System.Linq;

namespace MushiServicesHost
{

    public class AvatarUpdater
    {
        /// <summary>
        /// The player context
        /// </summary>
        private readonly PlayerContext _playerContext = new PlayerContext();

        public void Run()
        {
            var localPlayers = _playerContext.GetProgamers().ToList();
            using (var service = new MushiDbService())
            {
                foreach (var player in localPlayers)
                {
                    long steamId;
                    if (long.TryParse(player.ProviderKey, out steamId))
                    {

                        var playerInfo = new SteamContext().GetPlayerSteamInfo(steamId, SteamRequestsEnum.SignIn);
                        if (playerInfo != null)
                        {
                            service.UpdatePlayerAvatar(player.UserId, playerInfo.AvatarUrl);
                        }
                        else
                        {
                            Console.WriteLine("Unsuccessfull load info of: {0}", player.ProviderKey);
                        }
                    }
                }
            }
        }
    }
}
