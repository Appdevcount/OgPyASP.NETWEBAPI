using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Outputs
{
    public class ProcessDTO
    {
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string operatorRef { get; set; }
        public string currentBalance { get; set; }
        public string rechargedAmount { get; set; }
        public string customMessage { get; set; }
        public string customMessageAr { get; set; }
        public string serial { get; set; }
        public string pin { get; set; }
        public string UserBalance { get; set; }
    }
}
