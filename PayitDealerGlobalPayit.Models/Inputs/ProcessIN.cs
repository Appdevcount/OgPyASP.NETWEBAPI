using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Inputs
{
    public class ProcessIN
    {
        public int DeviceID { get; set; }
        public int ServiceId { get; set; }
        public int AccountId { get; set; }
        public int AccountUserId { get; set; }

        public String msisdn { get; set; }
        public String seriviceCode { get; set; }
        public String serviceName { get; set; }
        public String countryCode { get; set; }
        public String PaymentChannel { get; set; }
        public String transactionId { get; set; }

        public Double ServiceAmount { get; set; }
        public Double PaymentAmount { get; set; }
        
    }
}
