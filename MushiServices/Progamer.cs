//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MushiServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class Progamer
    {
        public string UserId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> AuctionStartPrice { get; set; }
        public Nullable<decimal> AuctionLimit { get; set; }
        public Nullable<int> Status { get; set; }
        public string PayPalEmail { get; set; }
        public string ProfileUrl { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
