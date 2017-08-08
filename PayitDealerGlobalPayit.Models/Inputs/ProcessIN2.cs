using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace PayitDealerGlobalPayit.Models.Inputs
{
    public class ProcessIN2
    {
        public int AccountId { get; set; }

        public int AccountUserId { get; set; }

        public string countryCode { get; set; }

        public string countryid { get; set; }

        public string customerid { get; set; }

        public int DeviceID { get; set; }

        public PayitDealerGlobalPayit.Models.Inputs.DonationReq DonationReq { get; set; }

        public string info2 { get; set; }

        public string infotext { get; set; }

        public string msisdn { get; set; }

        public string param1 { get; set; }

        public string param2 { get; set; }

        public string paymentamount { get; set; }

        public string paymentChannelCode { get; set; }

        public string paymentconfigid { get; set; }

        public string paymentcurrencycode { get; set; }

        public string paymentReference { get; set; }

        public string paymenttype { get; set; }

        public string receiptno { get; set; }

        public SecureHash secure { get; set; }

        public string serviceamount { get; set; }

        public string serviceconfigID { get; set; }

        public string servicecurrencyCode { get; set; }

        public int ServiceId { get; set; }

        public string serviceName { get; set; }

        public string servicepaymentid { get; set; }

        public string servicetype { get; set; }

        public string transactionId { get; set; }

        public string usertype { get; set; }

        public String ConversionAmount { get; set; }
    }
}
