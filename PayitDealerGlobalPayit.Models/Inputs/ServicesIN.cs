using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Inputs
{
    public class ServicesIN
    {
        public int AccountID { get; set; }
        public String usertype { get; set; }
        public GetActiveServices GetActiveServices { get; set; }
    }
    public class GetActiveServices
    {
        public string countryCode { get; set; }
        public string date { get; set; }
        public SecureHash secure { get; set; }
    }
    public class SecureHash
    {
        public string securehash { get; set; }
        public string data { get; set; }
    }
}
