using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Inputs
{
    public class ServiceDetailsIN
    {
        public string countryCode { get; set; }
        public int companyID { get; set; }
        public int categoryID { get; set; }
        public string ServiceCode { get; set; }
        public string RandomNumber { get; set; }
        public SecureHash secure { get; set; }
        public string usercountryCode { get; set; }


    }
}
