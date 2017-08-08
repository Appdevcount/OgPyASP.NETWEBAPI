using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models
{
    public class Status
    {
        public string statuscode { get; set; }
        public string statusdescription { get; set; }
        public string info1 { get; set; }
        public string info2 { get; set; }
        public Translations translations { get; set; }


        public Status(int i)
        {
            switch (i)
            {
                case 0 :
                    {
                        statuscode = "0";
                        statusdescription = "SUCCESS";
                        break;
                    }
                case 1:
                    {
                        statuscode = "1";
                        statusdescription = "FAILED";
                        break;
                    }
                case 101:
                    {
                        statuscode = "101";
                        statusdescription = "ACCOUNT USER INACTIVE";
                        break;
                    }
                case 102:
                    {
                        statuscode = "102";
                        statusdescription = "ACCOUNT INACTIVE";
                        break;
                    }
                case 103:
                    {
                        statuscode = "103";
                        statusdescription = "AGENT INACTIVE";
                        break;
                    }
                case 104:
                    {
                        statuscode = "104";
                        statusdescription = "DEALER INACTIVE";
                        break;
                    }
                case 105:
                    {
                        statuscode = "105";
                        statusdescription = "ACCOUNT DEVICE INACTIVE";
                        break;
                    }
                case 106:
                    {
                        statuscode = "106";
                        statusdescription = "AGENT DEVICE INACTIVE";
                        break;
                    }
                case 107:
                    {
                        statuscode = "107";
                        statusdescription = "DEALER DEVICE INACTIVE";
                        break;
                    }
                case 404:
                    {
                        statuscode = "404";
                        statusdescription = "UNAUTHORIZED";
                        break;
                    }
                default:
                    {
                        statuscode = "";
                        statusdescription = "";
                        break;
                    }
            }     

                    
               
            
        }
    }
}
