//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayitDealerGlobalPayit.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Agent_Balance
    {
        public int AgentBalanceId { get; set; }
        public Nullable<int> AgentId { get; set; }
        public Nullable<int> DealerId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<double> Commission { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual AgentServiceCommision AgentServiceCommision { get; set; }
        public virtual Dealer Dealer { get; set; }
    }
}
