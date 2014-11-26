using System;
using System.Collections.Generic;
using MushiDataTypes.Enums;

namespace MushiDataTypes
{
    [Serializable]
    public class Player : ICloneable
    {
        public string PlayerId { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }
        public PlayerTypesEnum PlayerType { get; set; }
        public decimal Price { get; set; }
        public decimal? AuctionStartPrice { get; set; }
        public decimal? AuctionLimit { get; set; }
        public PlayerStatus Status { get; set; }
        public string RoomId { get; set; }
        public string PayPalEmail { get; set; }
        public string  ProfileUrl { get; set; }
        public string SteamId { get; set; }

        public string StatusName
        {
            get { return Status.ToString(); }
        }

        public string TypeName
        {
            get { return PlayerType.ToString(); }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
