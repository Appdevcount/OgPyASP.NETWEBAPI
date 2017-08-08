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
    
    public partial class DealerTransaction
    {
        public long Id { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string DeviceIMEI { get; set; }
        public Nullable<int> DealerId { get; set; }
        public string DealerName { get; set; }
        public Nullable<int> AgentId { get; set; }
        public string AgentName { get; set; }
        public Nullable<int> AccountId { get; set; }
        public string AccountName { get; set; }
        public Nullable<int> AccountUserId { get; set; }
        public string AccountUser { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Pin { get; set; }
        public string Serial { get; set; }
        public string TrackID { get; set; }
        public string ServiceRefference { get; set; }
        public string OperatorReference { get; set; }
        public string PaymentReference { get; set; }
        public string PaymentChannel { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string info1 { get; set; }
        public string info2 { get; set; }
        public string info3 { get; set; }
        public Nullable<double> DeductedAmount { get; set; }
        public Nullable<double> Commission { get; set; }
    }
}
