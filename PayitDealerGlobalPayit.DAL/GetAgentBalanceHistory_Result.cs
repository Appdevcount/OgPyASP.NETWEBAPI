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
    
    public partial class GetAgentBalanceHistory_Result
    {
        public Nullable<int> AgentId { get; set; }
        public Nullable<int> DealerId { get; set; }
        public string AgentName { get; set; }
        public string BalanceType { get; set; }
        public string BlanceFor { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<double> Commission { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
    }
}
