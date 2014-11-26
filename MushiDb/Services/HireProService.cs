using System;
using System.Collections.Generic;
using System.Linq;
using MushiDataTypes;
using MushiDataTypes.Enums;

namespace MushiDb.Services
{
    public class HireProService : DisposableService
    {
        /// <summary>
        /// To the player.
        /// </summary>
        /// <param name="user">The player.</param>
        /// <returns></returns>
        private static Player ToPlayer(UsersView user)
        {
            if (user != null)
            {
                var result = new Player
                {
                    PlayerId = user.Id,
                    AvatarUrl = user.AvatarUrl,
                    NickName = user.UserName,
                    PlayerType = PlayerTypesEnum.Amateur,
                    Status = PlayerStatus.Offline,
                    SteamId = user.ProviderKey
                };
                return result;
            }
            return null;
        }

        /// <summary>
        /// Getalphabeticallies the pro players.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Player> GetProgamersAsc()
        {
            var players = Entities.ProgamersViews
                .OrderByDescending(s => s.Status)
                .ThenBy(u => u.UserName)
                .Select(pro => new Player
                    {
                        PlayerId = pro.Id,
                        AvatarUrl = pro.AvatarUrl,
                        NickName = pro.UserName,
                        Price = pro.Price,
                        AuctionStartPrice = pro.AuctionStartPrice,
                        AuctionLimit = pro.AuctionLimit,
                        PlayerType = PlayerTypesEnum.Pro,
                        Status = (PlayerStatus)pro.Status,
                        PayPalEmail = pro.PayPalEmail,
                        ProfileUrl = pro.ProfileUrl,
                        SteamId = pro.ProviderKey
                    }
                );

            return players;
        }

        /// <summary>
        /// Gets the progamer.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public Player GetProgamer(string playerId)
        {
            var progamer = Entities.ProgamersViews.SingleOrDefault(o => o.Id == playerId);
            if (progamer != null)
            {
                return new Player
                {
                    PlayerId = progamer.Id,
                    AvatarUrl = progamer.AvatarUrl,
                    NickName = progamer.UserName,
                    Price = progamer.Price,
                    AuctionStartPrice = progamer.AuctionStartPrice,
                    AuctionLimit = progamer.AuctionLimit,
                    PlayerType = PlayerTypesEnum.Pro,
                    Status = (PlayerStatus)progamer.Status,
                    PayPalEmail = progamer.PayPalEmail,
                    ProfileUrl = progamer.ProfileUrl,
                    SteamId = progamer.ProviderKey
                };
            }
            return null;
        }

        /// <summary>
        /// Gets the player account.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public Player GetPlayer(string playerId)
        {
            var player = Entities.UsersViews.SingleOrDefault(o => o.Id == playerId);
            var isPro = Entities.Progamers.Any(o => o.UserId == playerId);
            if (isPro)
            {
                return GetProgamer(playerId);
            }
            return player != null ? ToPlayer(player) : null;
        }

        /// <summary>
        /// Gets the players by ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public IEnumerable<Player> GetAmatersByIds(IEnumerable<string> ids)
        {
            return Entities.UsersViews
                .Where(o => ids.Contains(o.Id))
                .Select(ToPlayer);
        }

        /// <summary>
        /// Updates the player information.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerAvatarUrl">The player avatar URL.</param>
        /// <param name="playerName">Name of the player.</param>
        public void UpdatePlayerInfo(string playerId, string playerAvatarUrl, string playerName)
        {
            var player = Entities.AspNetUsers.SingleOrDefault(o => o.Id == playerId);
            if (player != null)
            {
                player.AvatarUrl = playerAvatarUrl;
                player.UserName = playerName;
                SaveChanges();
            }
        }

        /// <summary>
        /// Updates the player information.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerAvatarUrl">The player avatar URL.</param>
        public void UpdatePlayerInfo(string playerId, string playerAvatarUrl)
        {
            var player = Entities.AspNetUsers.SingleOrDefault(o => o.Id == playerId);
            if (player != null)
            {
                player.AvatarUrl = playerAvatarUrl;
                SaveChanges();
            }
        }

        /// <summary>
        /// Updates the player price.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="price">The price.</param>
        public void UpdatePlayerPrice(string playerId, decimal price)
        {
            var player = Entities.Progamers.SingleOrDefault(o => o.UserId == playerId);

            if (player != null)
            {
                player.Price = price;
                SaveChanges();
            }
        }

        /// <summary>
        /// Updates the player status.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdatePlayerStatus(string playerId, PlayerStatus status)
        {
            var player = Entities.Progamers.SingleOrDefault(o => o.UserId == playerId);

            if (player != null)
            {
                player.Status = (int)status;
                SaveChanges();
            }
        }

        /// <summary>
        /// Creates the game order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="stateId"></param>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="orderTime">The order time.</param>
        /// <param name="price">The price.</param>
        public string CreateGameOrder(string orderId, int stateId, string buyerId, string sellerId, DateTime orderTime, decimal price)
        {
            Entities.GameOrders.Add(
                new GameOrder
                {
                    OrderId = orderId,
                    StateId = stateId,
                    BuyerId = buyerId,
                    SellerId = sellerId,
                    OrderTime = orderTime,
                    OrderPrice = price
                });
            return SaveChanges() == false ? string.Empty : orderId;
        }

        /// <summary>
        /// Creates the steam request.
        /// </summary>
        /// <param name="result">The result.</param>
        public void CreateSteamRequest(SteamRequestResult result)
        {
            Entities.SteamRequests.Add(
                new SteamRequest
                {
                    Day = result.Day,
                    SignUp = result.SignUp,
                    SignIn = result.SignIn,
                    ProLoad = result.ProLoad,
                    HireAPro = result.HireAPro,
                    GameCheck = result.GameCheck,
                    PreGame = result.PreGame
                });
            SaveChanges();
        }

        /// <summary>
        /// Updates the steam request.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool UpdateSteamRequest(SteamRequestResult result)
        {
            var item = Entities.SteamRequests.SingleOrDefault(o => o.Day == result.Day);
            if (item != null)
            {
                item.SignUp += result.SignUp;
                item.SignIn += result.SignIn;
                item.ProLoad += result.ProLoad;
                item.HireAPro += result.HireAPro;
                item.GameCheck += result.GameCheck;
                item.PreGame += result.PreGame;
                return SaveChanges();
            }
            return false;
        }

        /// <summary>
        /// Exists the order.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        public bool ExistOrder(int stateId, string buyerId, string sellerId)
        {
            var order = Entities.GameOrders.SingleOrDefault(o => o.StateId == stateId
                                                                 && o.BuyerId == buyerId && o.SellerId == sellerId);
            return order != null;
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        public GameOrder GetOrder(int stateId, string buyerId, string sellerId)
        {
            return Entities.GameOrders.SingleOrDefault(o => o.StateId == stateId
                                                            && o.BuyerId == buyerId && o.SellerId == sellerId);
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        public GameOrder GetOrder(string buyerId, string sellerId)
        {
            return Entities.GameOrders.SingleOrDefault(o => o.BuyerId == buyerId && o.SellerId == sellerId);
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public GameOrder GetOrder(string orderId)
        {
            return Entities.GameOrders.SingleOrDefault(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Updates the state of the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="stateId">The p.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateOrderState(string orderId, int stateId)
        {
            var order = Entities.GameOrders.SingleOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.StateId = stateId;
                return SaveChanges();
            }
            return false;
        }

        /// <summary>
        /// Updates the state of the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="roomId">The room identifier.</param>
        /// <returns></returns>
        public bool UpdateOrderState(string orderId, int stateId, string roomId)
        {
            var order = Entities.GameOrders.SingleOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.StateId = stateId;
                order.RoomId = roomId;
                return SaveChanges();
            }
            return false;
        }


        /// <summary>
        /// Creates the auction.
        /// </summary>
        /// <param name="auctionId">The auction identifier.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="startPrice">The start price.</param>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="stateId">The state identifier.</param>
        public void CreateAuction(string auctionId, DateTime startTime, DateTime endTime, decimal startPrice, string sellerId, int stateId)
        {
            Entities.PlayerAuctions.Add(
                    new PlayerAuction
                    {
                        AuctionId = auctionId,
                        StartTime = startTime,
                        EndTime = endTime,
                        StartPrice = startPrice,
                        StateId = stateId,
                        SellerId = sellerId
                    }
                );
            SaveChanges();
        }

        /// <summary>
        /// Gets the state of the orders by.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public IEnumerable<GameOrder> GetAmaterOrdersByState(string buyerId, params GameOrderStatesEnum[] state)
        {
            return Entities.GameOrders.Where(o =>
                state.Contains((GameOrderStatesEnum)o.StateId)
                && o.BuyerId == buyerId);
        }

        /// <summary>
        /// Gets the state of the pro orders by.
        /// </summary>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public IEnumerable<GameOrder> GetProOrdersByState(string sellerId, params GameOrderStatesEnum[] state)
        {
            return Entities.GameOrders.Where(o =>
                state.Contains((GameOrderStatesEnum)o.StateId)
                && o.SellerId == sellerId);
        }

        /// <summary>
        /// Removes the order.
        /// </summary>
        /// <param name="order">The order identifier.</param>
        public void RemoveOrder(GameOrder order)
        {
            if (order != null)
            {
                var item = Entities
                    .GameOrders
                    .SingleOrDefault(o => o.OrderId == order.OrderId);
                if (item != null)
                {
                    Entities.GameOrders.Remove(item);
                    SaveChanges();
                }
            }
        }

        /// <summary>
        /// Removes the orders in states.
        /// </summary>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="states">The states.</param>
        public void RemoveOrdersInStates(string sellerId, params GameOrderStatesEnum[] states)
        {
            var orders = Entities.GameOrders
                .Where(o => states.Contains((GameOrderStatesEnum)o.StateId)
                            && o.SellerId == sellerId);
            Entities.GameOrders.RemoveRange(orders);
            SaveChanges();
        }


        /// <summary>
        /// Existses the state of the amater orders by.
        /// </summary>
        /// <param name="playerId">The buyer identifier.</param>
        /// <param name="states">The state.</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ExistsOrdersInStates(string playerId, PlayerTypesEnum type, IEnumerable<GameOrderStatesEnum> states)
        {
            if (type == PlayerTypesEnum.Amateur)
            {
                return Entities.GameOrders.Any(o =>
                    states.Contains((GameOrderStatesEnum)o.StateId)
                    && o.BuyerId == playerId);
            }
            return Entities.GameOrders.Any(o =>
                states.Contains((GameOrderStatesEnum)o.StateId)
                && o.SellerId == playerId);
        }

        /// <summary>
        /// Creates the pay pal transaction.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="ack">The ack.</param>
        /// <param name="paykey">The paykey.</param>
        /// <param name="status">The status.</param>
        public void CreatePayPalPayment(string orderId, int ack, string paykey, string status)
        {
            Entities.PayPals.Add(
                new PayPal
                {
                    OrderId = orderId,
                    Ack = ack,
                    PayKey = paykey,
                    Status = status
                }
            );
            SaveChanges();
        }

        /// <summary>
        /// Gets the pay pal payment.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public PayPal GetPayPalPayment(string orderId)
        {
            return Entities.PayPals.SingleOrDefault(o =>
                 o.OrderId == orderId);
        }

        /// <summary>
        /// Deletes the pay pal payment.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public void DeletePayPalPayment(string orderId)
        {
            var payement = Entities.PayPals.SingleOrDefault(o =>
                 o.OrderId == orderId);
            if (payement != null)
            {
                Entities.PayPals.Remove(payement);
                SaveChanges();
            }
        }

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdatePaymentStatus(string orderId, string status)
        {
            var payement = Entities.PayPals.SingleOrDefault(o =>
                 o.OrderId == orderId);
            if (payement != null)
            {
                payement.Status = status;
                SaveChanges();
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            try
            {
                Entities.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}