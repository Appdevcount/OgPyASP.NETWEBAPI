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
    
    public partial class UserRegistration
    {
        public int UserId { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string company { get; set; }
        public string description { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> EntityId { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Address { get; set; }
        public string DealerName { get; set; }
    }
}