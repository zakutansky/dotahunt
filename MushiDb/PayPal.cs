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
    
    public partial class PayPal
    {
        public string OrderId { get; set; }
        public string PayKey { get; set; }
        public int Ack { get; set; }
        public string Status { get; set; }
    
        public virtual GameOrder GameOrder { get; set; }
    }
}
