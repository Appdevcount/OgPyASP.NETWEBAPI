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
    
    public partial class Service
    {
        public int ID { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceHelp { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<double> IsysCommission { get; set; }
        public Nullable<bool> isCommissionFixed { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
