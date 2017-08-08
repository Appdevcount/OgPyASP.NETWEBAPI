using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models.Inputs
{
    public class DonationReq
    {

        public string amount { get; set; }

        public string currency { get; set; }

        public string DonationCentre { get; set; }

        public string karats { get; set; }

        public string no_of_persons { get; set; }

        public int weightInGrams { get; set; }
    }
}
