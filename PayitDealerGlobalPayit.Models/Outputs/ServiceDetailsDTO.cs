using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Outputs
{
    public class ServiceDetailsDTO
    {
        public string ServiceCategory { get; set; }
        public telecomServiceDetails TelecomServiceDetails { get; set; }
        public voucherServicesDetails VoucherServiceDetails { get; set; }
        public InternationalTopupServicesDetails InternationalTopupServicesDetails { get; set; }  
        public otherServiceDetails OtherServiceDetails { get; set; }
        public StatusInfo StatusInfo { get; set; }
        public List<Validations> Validations { get; set; }
        public String ConversionAmount { get; set; }
    }

   
    public class Validations
    {
        public string min { get; set; }
        public string max { get; set; }
        public string currencycode { get; set; }
        public string isAvailable { get; set; }
        public string validationType { get; set; }
        public string configID { get; set; }

    }

    

    public class telecomServiceDetails
    {

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyNameAR { get; set; }
        public servicePrepaid ServicePrepaid { get; set; }
        public servicePostpaid ServicePostpaid { get; set; }
        public images Images { get; set; }



    }
    public class voucherServicesDetails
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyNameAR { get; set; }
        public serviceVoucher ServiceVoucher { get; set; }
        public images Images { get; set; }
    }
   
    public class InternationalTopupServicesDetails
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyNameAR { get; set; }
        public serviceInternationalTopup serviceInternationalTopup { get; set; }
        public images Images { get; set; }
    }
    

   

    public class otherServiceDetails
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyNameAR { get; set; }
        public serviceOther ServiceOther { get; set; }
        public images Images { get; set; }
    }



    public class servicePrepaid
    {
        public int ServiceID { get; set; }
        public int ServiceConfigID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceType { get; set; }

        public int CurrencyID { get; set; }
        public int ServiceCurrencyID { get; set; }
        public string ServiceCurrency { get; set; }
        public string ServiceTwoCurrencyCode { get; set; }
        public string ServiceThreeCurrencyCode { get; set; }

        public String isEnabled { get; set; }

        public List<servicePrepaidDenomination> PrepaidDenomincations { get; set; }
      
        public List<TranslatedText> Translations { get; set; }

       
    }

    public class servicePostpaid
    {
        public int ServiceID { get; set; }
        public int ServiceConfigID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceType { get; set; }
        public int CurrencyID { get; set; }

        public int ServiceCurrencyID { get; set; }
        public string ServiceCurrency { get; set; }
        public string ServiceTwoCurrencyCode { get; set; }
        public string ServiceThreeCurrencyCode { get; set; }
       
        public String isEnabled { get; set; }
       
     
        public List<TranslatedText> Translations { get; set; }
        
    }

    public class serviceVoucher
    {
        public int ServiceID { get; set; }
        public int ServiceConfigID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceType { get; set; }
        public int CurrencyID { get; set; }
        public int ServiceCurrencyID { get; set; }
        public string ServiceCurrency { get; set; }
        public string ServiceTwoCurrencyCode { get; set; }
        public string ServiceThreeCurrencyCode { get; set; }       
        public String isEnabled { get; set; }
        public List<voucherDenomination> VoucherDenominations { get; set; }
      
        public List<TranslatedText> Translations { get; set; }
       

    }
  
    public class serviceInternationalTopup
    {
        public int ServiceID { get; set; }
        public int ServiceConfigID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceType { get; set; }
        public int CountryID { get; set; }
        public string ServiceCountry { get; set; }
        public string ServiceTwoCountryCode { get; set; }
        public string ServiceThreeCountryCode { get; set; }
        public string ServiceMobileCountryCode { get; set; }
        public int CurrencyID { get; set; }
        public int ServiceCurrencyID { get; set; }
        public string ServiceCurrency { get; set; }
        public string ServiceTwoCurrencyCode { get; set; }
        public string ServiceThreeCurrencyCode { get; set; }       
        public String isEnabled { get; set; }
        public List<voucherDenomination> VoucherDenominations { get; set; }
        public List<TranslatedText> Translations { get; set; }
       

    }
   

    
    public class serviceOther
    {
        public int ServiceID { get; set; }
        public int ServiceConfigID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceType { get; set; }
        public int CurrencyID { get; set; }
        public int ServiceCurrencyID { get; set; }
        public string ServiceCurrency { get; set; }
        public string ServiceTwoCurrencyCode { get; set; }
        public string ServiceThreeCurrencyCode { get; set; }
        public String isEnabled { get; set; }
        public string ServiceURL { get; set; }

        public List<voucherDenominationComments> voucherDenominationComments { get; set; }
      
        public List<TranslatedText> Translations { get; set; }
        
        public List<Area> Areas { get; set; }
    }

    public class Area
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string AreaNameAr { get; set; }
    }

    public class servicePrepaidDenomination
    {
        public int DenominationID { get; set; }
        public double DenominationAmount { get; set; }
        public string Currency { get; set; }

    }   

    public class voucherDenomination
    {
        public int VoucherDenominationID { get; set; }
        public string VoucherDenominationAmount { get; set; }
        public int VoucherCurrencyID { get; set; }
        public string VoucherTwoCurrencyCode { get; set; }
        public string VoucherThreeCurrencyCode { get; set; }
        public Nullable<long> VoucherDenominationCommentID { get; set; }
        public string VoucherComment { get; set; }
    }
  
    public class voucherDenominationComments
    {
        public int VoucherDenominationID { get; set; }
        public string VoucherDenominationAmount { get; set; }
        public int VoucherCurrencyID { get; set; }
        public string VoucherTwoCurrencyCode { get; set; }
        public string VoucherThreeCurrencyCode { get; set; }
        public Nullable<long> VoucherDenominationCommentID { get; set; }
        public string VoucherComment { get; set; }
     }
    
   

}
