using System;
using System.Collections.Generic;
using MushiDataTypes;
using MushiDataTypes.Enums;
using MushiDb.Services;

namespace Mushi.Context
{
    public class PlayerAuctionContext
    {
        /// <summary>
        /// Initializes the auction hours.
        /// </summary>
        /// <returns></returns>
        public List<AuctionHour> InitAuctionHours()
        {
            var result = new List<AuctionHour>();
            var dateNow = DateTime.Now;
            for (int i = 0; i < 24; i++)
            {
                result.Add( 
                    new AuctionHour { GameTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, i, 0, 0) }
                );
            }
            return result;
        }

        public void CreateNewAuction(DateTime startTime, DateTime endTime, decimal startPrice, string sellerId)
        {
            var auctionId = Guid.NewGuid().ToString();
            var state = AuctionStatesEnum.InProgress;
            using (var service = new HireProService())
            {
                service.CreateAuction(auctionId, startTime, endTime, startPrice, sellerId, (int)state);
            }
        }

        /// <summary>
        /// Determines whether this instance [can create auction] the specified player identifier.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="auctionDate">The auction date.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public bool CanCreateAuction(string playerId, DateTime auctionDate, int p)
        {
            return false;
        }
    }
}