using System.Collections.Generic;
using System.Linq;
using MushiDb.Services;
using MushiDataTypes;
using MushiDataTypes.Enums;

namespace Mushi.Context
{
    public class ProgamerContext
    {
        /// <summary>
        /// Gets the progamer.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public Player GetProgamer(string playerId)
        {
            using (var service = new HireProService())
            {
                return service.GetProgamer(playerId);
            }
        }

        /// <summary>
        /// Updates the player price.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="price">The price.</param>
        public void UpdatePrice(string playerId, decimal price)
        {
            using (var service = new HireProService())
            {
                service.UpdatePlayerPrice(playerId, price);
            }
        }

        /// <summary>
        /// Getalphabeticallies the pro players.
        /// </summary>
        /// <returns></returns>
        public List<Player> GetProgamersAsc()
        {
            using (var service = new HireProService())
            {
                return service.GetProgamersAsc().ToList();
            }
        }

        /// <summary>
        /// Updates the player status.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdateStatus(string playerId, PlayerStatus status)
        {
            using (var service = new HireProService())
            {
                service.UpdatePlayerStatus(playerId, status);
            }
        }

        /// <summary>
        /// Gets the players status.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public PlayerStatus GetProgamerStatus(string playerId)
        {
            using (var service = new HireProService())
            {
                var player = service.GetProgamer(playerId);
                if (player != null)
                {
                    return player.Status;
                }
                return default(PlayerStatus);
            }
        }
    }
}