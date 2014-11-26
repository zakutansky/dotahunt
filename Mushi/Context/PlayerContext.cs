using System.Collections.Generic;
using System.Linq;
using Mushi.Extensions;
using MushiDb.Services;
using MushiDataTypes;
using MushiDb;
using MushiDataTypes.Enums;

namespace Mushi.Context
{
    public class PlayerContext
    {
        /// <summary>
        /// Gets the player account by player identifier.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public Player GetPlayer(string playerId)
        {
            using (var service = new HireProService())
            {
                return service.GetPlayer(playerId);
            }
        }

        /// <summary>
        /// Gets the players by ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public IEnumerable<Player> GetAmatersByIds(string[] ids)
        {
            using (var service = new HireProService())
            {
                return service.GetAmatersByIds(ids).ToList();
            }
        }

        /// <summary>
        /// Updates the player information.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerAvatarUrl">The player avatar URL.</param>
        /// <param name="playerName">Name of the player.</param>
        public void UpdatePlayerInfo(string playerId, string playerAvatarUrl, string playerName)
        {
            using (var service = new HireProService())
            {
                service.UpdatePlayerInfo(playerId, playerAvatarUrl, playerName);
            }
        }

        
        

        /// <summary>
        /// Updates the player information.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerAvatarUrl">The player avatar URL.</param>
        public void UpdatePlayerInfo(string playerId, string playerAvatarUrl)
        {
            using (var service = new HireProService())
            {
                service.UpdatePlayerInfo(playerId, playerAvatarUrl);
            }
        }

        /// <summary>
        /// Sets the status to players by game orders.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <param name="orders">The orders.</param>
        public void SetStatusesToPlayersAccordingToOrders(List<Player> players, List<GameOrder> orders)
        {
            if (players == null || orders == null || players.Count == 0 || orders.Count == 0) return;
            var type = players.First().PlayerType;
            if (type == PlayerTypesEnum.Pro)
            {
                foreach (var player in players)
                {
                    var order = orders.SingleOrDefault(o => o.SellerId == player.PlayerId);
                    if (order == null) continue;
                    var associatedStatus = ((GameOrderStatesEnum)order.StateId).GetAssociatedStatus();
                    if (associatedStatus == null) continue;
                    player.Status = associatedStatus.Value;
                    player.Price = order.OrderPrice;
                    if (player.Status == PlayerStatus.InGame)
                    {
                        player.RoomId = order.RoomId;
                    }
                }
            }
            else
            {
                foreach (var player in players)
                {
                    var order = orders.SingleOrDefault(o => o.BuyerId == player.PlayerId);
                    if (order == null) continue;
                    var associatedStatus = ((GameOrderStatesEnum)order.StateId).GetAssociatedStatus();
                    if (associatedStatus == null) continue;
                    player.Status = associatedStatus.Value;
                    player.Price = order.OrderPrice;
                    if (player.Status == PlayerStatus.InGame)
                    {
                        player.RoomId = order.RoomId;
                    }
                }
            }
        }
    }

}
