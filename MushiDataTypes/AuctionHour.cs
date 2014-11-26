using System;

namespace MushiDataTypes
{
    public class AuctionHour
    {
        public DateTime GameTime { get; set; }
        public decimal? StartPrice { get; set; }
        public int Hour
        {
            get{ return GameTime.Hour; }
        }
    }
}