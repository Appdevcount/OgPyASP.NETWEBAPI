using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Outputs
{
    public class ServicesDTO
    {
        public List<AccountServices> AccountServices { get; set; }
        public mainServices mainServices { get; set; }
        //public Status status { get; set; }

    }
    public class mainServices
    {
        public List<CategoryService> ServiceCategories { get; set; }
        public StatusInfo StatusInfo { get; set; }
    }
    public class CategoryServices
    {
        public List<CategoryService> ServiceCategories { get; set; }
        public StatusInfo StatusInfo { get; set; }
    }

    public class CategoryService 
    {
        public int ServiceCategoryID { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceCategoryNameAR { get; set; }
        public string ServiceCategoryCode { get; set; }       
        public List<ServiceGroup> ServiceGroups { get; set; }
        public List<TranslatedText> Translations { get; set; }
      

    }

    public class companies
    {
        public int companyID { get; set; }
        public string companyName { get; set; }
        public string companyNameAR { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceCategoryNameAR { get; set; }
        public string companyLogo { get; set; }
        public string companyCode { get; set; }
        public List<TranslatedText> Translations { get; set; }
        public images Images { get; set; }
        public string isEnabled { get; set; }
    }

    public class images
    {
        public string ImageURL1 { get; set; }
        public string ImageURL2 { get; set; }
        public string ImageURL3 { get; set; }
        public string ImageURL4 { get; set; }
        public string ImageURL5 { get; set; }
    }
    public class TranslatedText
    {
        public string sourceText { get; set; }
        public string langCode { get; set; }
        public string translatedText { get; set; }
    }
    public class StatusInfo
    {
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public Infos Infos { get; set; }
        public List<TranslatedText> Translations { get; set; }
    }
    public class Infos
    {
        public string Info1 { get; set; }
        public string Info2 { get; set; }
    }
    public class AccountServices
    {
        public int CompanyID;
        public List<int> ServiceID{get;set;}
    }

    public interface IObjectSortByPriority
    {
        int Priority { get; set; }
    }

  
    public class ServiceGroup : IObjectSortByPriority
    {
        public string companyCode { get; set; }

        public int companyID { get; set; }

        public images CompanyImages { get; set; }

        public string companyName { get; set; }

        public string companyNameAR { get; set; }

        public string isEnabled { get; set; }

        public int Priority { get; set; }

        public string ServiceCategoryCode { get; set; }

        public int ServiceCategoryID { get; set; }

        public string ServiceCategoryName { get; set; }

        public string ServiceCategoryNameAR { get; set; }

        public string ServiceCode { get; set; }

        public images ServiceGroupImages { get; set; }

        public string ServiceGroupName { get; set; }

        public string ServiceGroupType { get; set; }

        public List<ServiceForCategory> Services { get; set; }

        public List<TranslatedText> Translations { get; set; }

    }
    public class ServiceForCategory : IObjectSortByPriority//IEquatable<ServiceForCategory>, IComparable<ServiceForCategory>
    {

        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public images ServiceImages { get; set; }
        public string ServiceType { get; set; }
        public int ServiceTypeID { get; set; }
        public string ServiceCountryCode { get; set; }
        public string ServiceCommissionDetails { get; set; }
        public List<TranslatedText> Translations { get; set; }
        public int Priority { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ServiceForCategory objAsSer = obj as ServiceForCategory;
            if (objAsSer == null) return false;
            else if (objAsSer.Priority == 0)
                return false;
            else return Equals(objAsSer);
        }
        public int CompareTo(ServiceForCategory other)
        {
            if (other == null)//|| other.Priority == 0)
                return 1;
            if (other.Priority == 0)
                return -1;
            int si = this.Priority.CompareTo(other.Priority);
            return si;
        }
        public override int GetHashCode()
        {
            return Priority;
        }
        public bool Equals(ServiceForCategory other)
        {
            if (other == null) return false;
            else if (other.Priority == 0) return false;
            return (this.Priority.Equals(other.Priority));
        }
    }

}
