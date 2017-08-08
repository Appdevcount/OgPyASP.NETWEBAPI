using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Outputs
{
   

    public class ServiceDetailsDTO2
    {
        public string ServiceCategory { get; set; }
        public ServiceDetails ServiceDetails { get; set; }     
        public StatusInfo StatusInfo { get; set; }
        public List<Validations> Validations { get; set; }
        public String ConversionAmount { get; set; }
    }
    public class ServiceDetails
    {

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyNameAR { get; set; }
        public service Service { get; set; }       
        public images Images { get; set; }

    }

    public class service
    {
        public int      ServiceID                   { get; set; }
        public int      ServiceConfigID             { get; set; }
        public string   ServiceCode                 { get; set; }
        public string   ServiceName                 { get; set; }
        public string   ServiceNameAR               { get; set; }
        public string   ServiceType                 { get; set; }       
        public int      CurrencyID                  { get; set; }
        public int      ServiceCurrencyID           { get; set; }
        public string   ServiceCurrency             { get; set; }
        public string   ServiceTwoCurrencyCode      { get; set; }
        public string   ServiceThreeCurrencyCode    { get; set; }

        // For International TopUp Starts
        public int CountryID { get; set; }
        public string ServiceCountry { get; set; }
        public string ServiceTwoCountryCode { get; set; }
        public string ServiceThreeCountryCode { get; set; }
        public string ServiceMobileCountryCode { get; set; }
        // For International TopUp Ends

        public String isEnabled { get; set; }
        public List<servicePrepaidDenomination> PrepaidDenomincations { get; set; } // For Prepaid 
        public List<voucherDenomination> VoucherDenominations { get; set; } // For Vouchers and International TopUp
        public List<TranslatedText> Translations { get; set; }

    }

}
