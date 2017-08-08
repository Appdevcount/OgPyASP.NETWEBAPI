using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayitDealerGlobalPayit.Models;

namespace PayitDealerGlobalPayit.Models.Outputs
{
    public class LoginDTO
    {
        public string loginstatus { get; set; }
        public int LoginStatusCode { get; set; }
        public int CountryID { get; set; }
        public string CountryCode { get; set; }
        public int  AccountID { get; set; }
        public int  AccountUserID  { get; set; }
        public int DeviceID { get; set; }
        public String UserName { get; set; }
        public String MobileNumber { get; set; }
        public String EmailID { get; set; }
        public String UserType { get; set; }
        public String CustomerCareNumber { get; set; }
        public List<PaymentChannel> PaymentChannels { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string Balance { get; set; }
    }

    public class PaymentChannel
    {
        public Nullable<int> PaymentChannelID { get; set; }
        public String PaymentChannelCode { get; set; }
        public String PaymentChannelName { get; set; }
        public String CurrencyCode  { get; set; }
        public String Image { get; set; }
    }
    public class PaymentChannelResp
    {
        public Nullable<int> PaymentChannelID { get; set; }
        public Nullable<int> PaymentChannelConfigID { get; set; }
        public Nullable<int> ServicePaymentID { get; set; }
        public string PaymentChannelName { get; set; }
        public string PaymentChannelNameAR { get; set; }
        public string PaymentChannelLink { get; set; }
        public string PaymentChannelCode { get; set; }


        public Nullable<int> PaymentChannelCurrencyID { get; set; }
        public string PaymentChannelCurrency { get; set; }
        public string PaymentChannelTwoCurrencyCode { get; set; }
        public string PaymentChannelThreeCurrencyCode { get; set; }
        public Nullable<double> PaymentChannelConversionRate { get; set; }

        public Nullable<int> Priority { get; set; }
        public List<paymentChannelsCommission> PaymentChannelsCommission { get; set; }
        public images Images { get; set; }

    }
    public class paymentChannelsCommission
    {
        public Nullable<double> PaymentChannelCommission { get; set; }
        public string PaymentChannelCommissionType { get; set; }
        public Nullable<double> PaymentChannelTreshold { get; set; }
        public Nullable<int> PaymentChannelTresholdType { get; set; }
    }
   
}
