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
    
    public partial class AuctionState
    {
        public AuctionState()
        {
            this.PlayerAuctions = new HashSet<PlayerAuction>();
        }
    
        public int StateId { get; set; }
        public string StateName { get; set; }
    
        public virtual ICollection<PlayerAuction> PlayerAuctions { get; set; }
    }
}
