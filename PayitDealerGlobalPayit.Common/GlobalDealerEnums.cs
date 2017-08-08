using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Common
{
    public class GlobalDealerEnums
    {
        public enum Role
        {
            Adminstrator=1,
            Dealer=2,
            Agent=3,
            CustomerSuport=4,
            Accountant=5,
            AccountUser=6,
            CountrySpecificAdministrator=7,
            CountrySpecificAccountant=8,
            CountrySpecificCustomerSupport=9
        }

        public enum LoginMessageCode
        {
            AccountUserInactive = 1,
            AccountInactive = 2,
            AgentInactive = 3,
            DealerInactive = 4,
            AccountDeviceInactive = 5,
            AgentDeviceInactive = 6,
            DealerDeviceInactive = 7,
            
        }
    }
}
