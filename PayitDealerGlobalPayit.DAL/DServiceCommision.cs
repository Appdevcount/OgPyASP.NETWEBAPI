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
    
    public partial class DServiceCommision
    {
        public int ID { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public string ComType { get; set; }
        public Nullable<double> Threshold { get; set; }
        public Nullable<double> ComValue { get; set; }
        public bool Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string DealerName { get; set; }
        public string ServiceName { get; set; }
        public string ThresholdType { get; set; }
        public string Country { get; set; }
    }
}
