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
    
    public partial class DealerDevice
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsAssigned { get; set; }
        public Nullable<bool> IsDisabled { get; set; }
    
        public virtual Dealer Dealer { get; set; }
        public virtual Device Device { get; set; }
    }
}
