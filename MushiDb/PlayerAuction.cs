//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MushiDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayerAuction
    {
        public PlayerAuction()
        {
            this.AuctionBids = new HashSet<AuctionBid>();
        }
    
        public string AuctionId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public decimal StartPrice { get; set; }
        public string SellerId { get; set; }
        public Nullable<decimal> EndPrice { get; set; }
        public string BuyerId { get; set; }
        public int StateId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<AuctionBid> AuctionBids { get; set; }
        public virtual AuctionState AuctionState { get; set; }
        public virtual Progamer Progamer { get; set; }
    }
}
