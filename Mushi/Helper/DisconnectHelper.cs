using System.Collections.Generic;
using Mushi.Context;
using MushiDataTypes;
using MushiDataTypes.Enums;
using MushiDb.Services;
using Newtonsoft.Json;
using System.Linq;
using MushiDb;

namespace Mushi.Helper
{
    public static class DisconnectHelper
    {

        /// <summary>
        /// Does the on offline.
        /// </summary>
        /// <param name="player">The player.</param>
        public static void DoOnOffline(Player player)
        {
            if (player == null) return;
            var playerId = player.PlayerId;
            using (var service = new HireProService())
            {
                player.Status = PlayerStatus.Offline;
                service.UpdatePlayerStatus(playerId, PlayerStatus.Offline);
                var orders = service.GetProOrdersByState(playerId, GameOrderStatesEnum.PendingInvitation);
                var gameOrderses = orders as IList<GameOrder> ?? orders.ToList();
                if (gameOrderses.Count() != 0)
                {
                    var buyerIds = gameOrderses.Select(o => o.BuyerId).ToArray();
                    service.RemoveOrdersInStates(playerId, GameOrderStatesEnum.PendingInvitation);
                    HubContext.Instance.RemoveOrdersAsync(player.PlayerId, JsonConvert.SerializeObject(buyerIds));
                }
                HubContext.Instance.UpdatePlayer(JsonConvert.SerializeObject(player));
            }
        }
    }
}