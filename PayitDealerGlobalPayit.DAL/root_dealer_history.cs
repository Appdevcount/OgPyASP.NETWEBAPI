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
    
    public partial class root_dealer_history
    {
        public long id { get; set; }
        public Nullable<int> service_id { get; set; }
        public Nullable<double> amount { get; set; }
        public string tran_type { get; set; }
        public string track_id { get; set; }
        public Nullable<System.DateTime> tran_date { get; set; }
        public Nullable<int> dealer_id { get; set; }
        public Nullable<int> agent_id { get; set; }
        public Nullable<int> device_id { get; set; }
    }
}
