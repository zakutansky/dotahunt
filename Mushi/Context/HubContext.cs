using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Mushi.Hubs;

namespace Mushi.Context
{
    public class HubContext
    {
        //TODO checknyc jesli to nima multithreading
        /// <summary>
        /// The _instance
        /// </summary>
        private static readonly Lazy<HubContext> HubContextInstance = new Lazy<HubContext>(
                    () => new HubContext(GlobalHost.ConnectionManager.GetHubContext<MushiHub>().Clients)
                );

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>
        /// The clients.
        /// </value>

        private IHubConnectionContext<dynamic> Clients { get; set; }
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static HubContext Instance
        {
            get { return HubContextInstance.Value; }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="HubContext"/> class from being created.
        /// </summary>
        /// <param name="clients"></param>
        private HubContext(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        /// <summary>
        /// Updates the pro player.
        /// </summary>
        public void UpdatePlayer(string serializedPlayer)
        {
            Clients.All.updatePlayer(serializedPlayer);
        }

        /// <summary>
        /// Updates the player orders.
        /// </summary>
        /// <param name="userId">Name of the player general user.</param>
        /// <param name="serializedPlayer">The serialized player.</param>
        public void UpdatePlayerOrdersAsync(string userId, string serializedPlayer)
        {
            Task.Run(() =>
            {
                var key = Guid.NewGuid().ToString();
                MushiHub.ExecuteWithAck(
                    (o, k, p) => Clients.User(o).updatePlayerOrders(k, p),
                    userId,
                    serializedPlayer,
                    key);
            });
        }

        /// <summary>
        /// Accepts the order.
        /// </summary>
        /// <param name="serializedPlayer">The serialized player.</param>
        /// <param name="userId">Name of the amater general user.</param>
        public void AcceptOrderAsync(string serializedPlayer, string userId)
        {
            Task.Run(() =>
            {
                var key = Guid.NewGuid().ToString();
                MushiHub.ExecuteWithAck(
                    (o, k, p) => Clients.User(o).acceptOrder(k, p),
                    userId,
                    serializedPlayer,
                    key);
            });
        }

        /// <summary>
        /// Aborts the order.
        /// </summary>
        /// <param name="serializedPlayer">The serialized player.</param>
        /// <param name="userId">Name of the player general user.</param>
        public void AbortOrderAsync(string serializedPlayer, string userId)
        {
            Task.Run(() =>
            {
                var key = Guid.NewGuid().ToString();
                MushiHub.ExecuteWithAck(
                    (o, k, p) => Clients.User(o).abortOrder(k, p),
                    userId,
                    serializedPlayer,
                    key);
            });
        }

        /// <summary>
        /// Orders the complete.
        /// </summary>
        /// <param name="serializedPlayer"></param>
        /// <param name="userId">Name of the general user.</param>
        public void OrderCompleteAsync(string serializedPlayer, string userId)
        {
            Task.Run(() =>
            {
                var key = Guid.NewGuid().ToString();
                MushiHub.ExecuteWithAck(
                    (o, k, p) => Clients.User(o).orderComplete(k, p),
                    userId,
                    serializedPlayer,
                    key);
            });
        }

        /// <summary>
        /// Removes the orders.
        /// </summary>
        /// <param name="userId">Name of the general user.</param>
        /// <param name="playerIds">The player ids.</param>
        public void RemoveOrdersAsync(string userId, string playerIds)
        {
            Task.Run(() =>
            {
                var key = Guid.NewGuid().ToString();
                MushiHub.ExecuteWithAck(
                    (o, k, p) => Clients.User(o).removeOrders(k, p),
                    userId,
                    playerIds,
                    key);
            });
        }
    }
}