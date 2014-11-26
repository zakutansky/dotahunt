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
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AuctionBids = new HashSet<AuctionBid>();
            this.GameOrders = new HashSet<GameOrder>();
            this.PlayerAuctions = new HashSet<PlayerAuction>();
        }
    
        public string Id { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AuctionBid> AuctionBids { get; set; }
        public virtual ICollection<GameOrder> GameOrders { get; set; }
        public virtual ICollection<PlayerAuction> PlayerAuctions { get; set; }
        public virtual Progamer Progamer { get; set; }
    }
}
