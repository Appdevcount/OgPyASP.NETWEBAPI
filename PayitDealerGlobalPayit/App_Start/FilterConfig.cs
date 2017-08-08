using System.Web;
using System.Web.Mvc;

namespace PayitDealerGlobalPayit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}