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
    
    public partial class Tbl_Account_WalletBalance_History
    {
        public long AccountWalletHistoryId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<double> Commision { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Tbl_Accounts Tbl_Accounts { get; set; }
    }
}
