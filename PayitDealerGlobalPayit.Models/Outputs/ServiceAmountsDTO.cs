using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Outputs
{
   public class ServiceAmountsDTO
    {
       public String  AgentName { get; set; }
       public String AccountName { get; set; }
       public String AccountWalletBalence { get; set; }
       public String AccountWalletCurrencyCode { get; set; }
       public List<ServiceAmounts> ServiceAmounts { get; set; }
    }

   public class ServiceAmounts
   {
       public int ServiceID { get; set; }
       public String ServiceName { get; set; }
       public Double Amount { get; set; }
       public List<TranslatedText> translations { get; set; }

   }
}
