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
    
    public partial class ComplaintHistory
    {
        public int ID { get; set; }
        public Nullable<int> ComplaintID { get; set; }
        public string StatusDesc { get; set; }
        public string EditedBy { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> CretedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public bool Status { get; set; }
    }
}
