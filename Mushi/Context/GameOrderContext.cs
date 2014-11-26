using System;
using System.Collections.Generic;
using System.Linq;
using MushiDataTypes.Enums;
using MushiDb.Services;
using MushiDb;

namespace Mushi.Context
{
    public class GameOrderContext
    {

        /// <summary>
        /// Creates the order.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="price">The price.</param>
        public string TryCreateOrder(string buyerId, string sellerId, decimal price)
        {
            using (var service = new HireProService())
            {
                var orderId = Guid.NewGuid().ToString();
                var orderStateId = (int)GameOrderStatesEnum.PendingInvitation;
                var orderTime = DateTime.Now;
                var existOrder = ExistOrder(service, orderStateId, buyerId, sellerId);
                if (!existOrder) return service.CreateGameOrder(orderId, orderStateId, buyerId, sellerId, orderTime, price);
                return string.Empty;
            }
        }



        /// <summary>
        /// Exists the order.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        public bool ExistOrder(HireProService service, int stateId, string buyerId, string sellerId)
        {
            return service.ExistOrder(stateId, buyerId, sellerId);
        }

        /// <summary>
        /// Exists the order.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="currentGame">The current game.</param>
        /// <returns></returns>
        public bool ExistOrder(GameOrderStatesEnum stateId, string buyerId, string sellerId)
        {
            using (var service = new HireProService())
            {
                return service.ExistOrder((int)stateId, buyerId, sellerId);
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        public GameOrder GetOrder(GameOrderStatesEnum stateId, string buyerId, string sellerId)
        {
            using (var service = new HireProService())
            {
                return service.GetOrder((int)stateId, buyerId, sellerId);
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        public GameOrder GetOrder(string buyerId, string sellerId)
        {
            using (var service = new HireProService())
            {
                return service.GetOrder(buyerId, sellerId);
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public GameOrder GetOrder(string orderId)
        {
            using (var service = new HireProService())
            {
                return service.GetOrder(orderId);
            }
        }

        /// <summary>
        /// Removes the order.
        /// </summary>
        /// <param name="order">The order identifier.</param>
        public void RemoveOrder(GameOrder order)
        {
            using (var service = new HireProService())
            {
                service.RemoveOrder(order);
            }
        }

        /// <summary>
        /// Updates the state of the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="state"></param>
        public bool UpdateOrderState(string orderId, GameOrderStatesEnum state)
        {
            using (var service = new HireProService())
            {
                return service.UpdateOrderState(orderId, (int)state);
            }
        }

        /// <summary>
        /// Updates the state of the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="state">The state.</param>
        /// <param name="roomId">The room identifier.</param>
        /// <returns></returns>
        public bool UpdateOrderState(string orderId, GameOrderStatesEnum state, string roomId)
        {
            using (var service = new HireProService())
            {
                return service.UpdateOrderState(orderId, (int)state, roomId);
            }
        }

        /// <summary>
        /// Gets the state of the orders by.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public List<GameOrder> GetAmaterOrdersByStates(string buyerId, params GameOrderStatesEnum[] state)
        {
            using (var service = new HireProService())
            {
                return service.GetAmaterOrdersByState(buyerId, state).ToList();
            }
        }

        /// <summary>
        /// Existses the state of the amater orders by.
        /// </summary>
        /// <param name="progamerId">The buyer identifier.</param>
        /// <param name="type"></param>
        /// <param name="states">The state.</param>
        /// <returns></returns>
        public bool ExistsOrdersInStates(string progamerId, PlayerTypesEnum type, params GameOrderStatesEnum[] states)
        {
            using (var service = new HireProService())
            {
                
                return service.ExistsOrdersInStates(progamerId, type, states);
            }
        }

        /// <summary>
        /// Gets the state of the orders by.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="game">The game.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public List<GameOrder> GetProOrdersByStates(string sellerId, params GameOrderStatesEnum[] state)
        {
            using (var service = new HireProService())
            {
                return service.GetProOrdersByState(sellerId, state).ToList();
            }
        }
    }
}