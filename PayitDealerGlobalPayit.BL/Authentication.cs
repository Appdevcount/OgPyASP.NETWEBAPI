using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayitDealerGlobalPayit.DAL;
using PayitDealerGlobalPayit.Common;
using PayitDealerGlobalPayit.Models.Outputs;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace PayitDealerGlobalPayit.BL
{
    public class Authentication
    {
        PayitGlobalDealersDBEntities db = new PayitGlobalDealersDBEntities();
        GlobalPayitEntities db2 = new GlobalPayitEntities();
        public Models.Outputs.LoginDTO checkLoginwithDeviceID(string username, string password)
        {
            Models.Outputs.LoginDTO LDTO = new Models.Outputs.LoginDTO();

            if (username.Equals(String.Empty) || password.Equals(String.Empty) || password.Equals(String.Empty))
            {
                LDTO.loginstatus = "false";
                LDTO.LoginStatusCode = 404;
            }
            else
            {
                List<int> roleids = new List<int> {2,3,6};
                var Validate = (from a in db.users where a.name == username && a.password == password && roleids.Contains(a.RoleID) && a.status == 1  select a).FirstOrDefault();

                if (Validate != null)
                {
                   // DeviceID = DeviceID.Trim();
                  // 

                    if (Validate.RoleID==(int)GlobalDealerEnums.Role.AccountUser)
                    {
                                           var ValidDevice = (from tau in db.Tbl_Account_Users
                                           //join tad in db.Tbl_Account_Devices on tau.AccountId equals tad.AccountId
                                           join tac in db.Tbl_Accounts on tau.AccountId equals tac.AccountId
                                           join ag in db.Agents on tac.AgentId equals ag.Id
                                           join d in db.Dealers on ag.DealerId equals d.Id
                                           //join tagd in db.Tbl_Agent_Devices on ag.Id equals tagd.AgentId
                                           //join dd in db.DealerDevices on d.Id equals dd.DealerId
                                           where tau.UserId == Validate.UserId && tau.AccountUserId == Validate.EntityId 
                                          // && tad.DeviceId == DevID && tad.IsDisabled == false && tagd.IsAssigned == true 
                                          && d.Status == true && ag.Status == true && tac.Status == true
                                           select tau).FirstOrDefault();



                        if (ValidDevice != null)
                        {
                            LDTO.loginstatus = "true";
                            LDTO.AccountID = Convert.ToInt32(ValidDevice.AccountId);
                            var CountryInfo = (from a in db.Tbl_Accounts join c in db.countries on a.CountryId equals c.id where a.AccountId == LDTO.AccountID select new { c.two_letter_code, c.id ,c.CustomercareNumber}).FirstOrDefault();
                            LDTO.CountryID = Convert.ToInt32(CountryInfo.id);
                            LDTO.CountryCode = CountryInfo.two_letter_code;
                            LDTO.CustomerCareNumber = CountryInfo.CustomercareNumber;
                            LDTO.AccountUserID = ValidDevice.AccountUserId;
                            LDTO.UserName = Validate.name;
                            LDTO.MobileNumber = Validate.PhoneNumber;
                            LDTO.EmailID = Validate.Email;
                            LDTO.UserType = GlobalDealerEnums.Role.AccountUser.ToString();
                            int DevID = (from a in db.Devices where a.IMEI == "WEB"+ Validate.PhoneNumber select a.Id).FirstOrDefault();
                            LDTO.DeviceID = DevID;
                            LDTO.PaymentChannels = getPaymetChannels(CountryInfo.two_letter_code);
                        }
                        else
                        {
                            LDTO.loginstatus = "false";
                            LDTO.LoginStatusCode = 5;

                        }   
                    }
                    else if(Validate.RoleID == (int)GlobalDealerEnums.Role.Agent)
                    {
                        var ValidDevice = (from ag in db.Agents
                                           join d in db.Dealers on ag.DealerId equals d.Id                                         
                                           where ag.Id == Validate.EntityId                                            
                                           && d.Status == true && ag.Status == true
                                           select ag).FirstOrDefault();



                        if (ValidDevice != null)
                        {
                            LDTO.loginstatus = "true";
                            LDTO.AccountID = Convert.ToInt32(ValidDevice.Id);
                             var CountryInfo = (from ag in db.Agents join  a in db.Dealers on ag.DealerId equals a.Id join  c in db.CountryCurrencies on a.CountryID equals c.ID
                                                join d in db.countries on c.CountryID equals d.id
                                                where ag.Id == LDTO.AccountID select new { d.two_letter_code, d.id,d.CustomercareNumber }).FirstOrDefault();

                            LDTO.CountryID = Convert.ToInt32(CountryInfo.id);
                            LDTO.CountryCode = CountryInfo.two_letter_code;
                            LDTO.CustomerCareNumber = CountryInfo.CustomercareNumber;
                            LDTO.AccountUserID = 0;
                            LDTO.UserName = Validate.name;
                            LDTO.MobileNumber = Validate.PhoneNumber;
                            LDTO.EmailID = Validate.Email;
                            LDTO.UserType = GlobalDealerEnums.Role.Agent.ToString();
                            LDTO.Balance = db.Tbl_Agent_WalletBalance.Where(a => a.AgentId == LDTO.AccountID).FirstOrDefault().Balance.ToString();
                            int DevID = (from a in db.Devices where a.IMEI == "WEB" + Validate.PhoneNumber select a.Id).FirstOrDefault();

                            LDTO.DeviceID = DevID;
                            LDTO.PaymentChannels = getPaymetChannels(CountryInfo.two_letter_code);
                        }
                        else
                        {
                            LDTO.loginstatus = "false";
                            LDTO.LoginStatusCode = 5;

                        }
                    }
                    else if (Validate.RoleID == (int)GlobalDealerEnums.Role.Dealer)
                    {
                        var ValidDevice = (from  d in db.Dealers                                          
                                           //join dd in db.DealerDevices on d.Id equals dd.DealerId
                                           where d.Id == Validate.EntityId  
                                           //&& dd.DeviceId == DevID && dd.IsDisabled == false && dd.IsAssigned == true 
                                           && d.Status == true
                                           select d).FirstOrDefault();



                        if (ValidDevice != null)
                        {
                            LDTO.loginstatus = "true";
                            LDTO.AccountID = Convert.ToInt32(ValidDevice.Id);
                               var CountryInfo = (from a in db.Dealers
                                                  join c in db.CountryCurrencies on a.CountryID equals c.ID
                                                  join d in db.countries on c.CountryID equals d.id
                                                  where a.Id == LDTO.AccountID select new { d.two_letter_code, d.id,d.CustomercareNumber }).FirstOrDefault();
                            LDTO.CountryID = Convert.ToInt32(CountryInfo.id);
                            LDTO.CountryCode = CountryInfo.two_letter_code;
                            LDTO.CustomerCareNumber = CountryInfo.CustomercareNumber;
                            LDTO.AccountUserID = 0;
                            LDTO.UserName = Validate.name;
                            LDTO.MobileNumber = Validate.PhoneNumber;
                            LDTO.EmailID = Validate.Email;
                            LDTO.UserType = GlobalDealerEnums.Role.Dealer.ToString();
                            //var currency=db2
                            //LDTO.CurrencyID = 0;
                            //LDTO.CurrencyCode = "";
                            LDTO.Balance = db.Tbl_Dealer_WalletBalance.Where(a => a.DealerId == LDTO.AccountID).FirstOrDefault().Balance.ToString(); 
                            int DevID = (from a in db.Devices where a.IMEI == "WEB" + Validate.PhoneNumber select a.Id).FirstOrDefault();

                            LDTO.DeviceID = DevID;                           
                            
                            LDTO.PaymentChannels = getPaymetChannels(CountryInfo.two_letter_code);
                        }
                        else
                        {
                            LDTO.loginstatus = "false";
                            LDTO.LoginStatusCode = 5;

                        }
                    }
                }
                else
                {
                    LDTO.loginstatus = "false";
                    LDTO.LoginStatusCode = 1;
                }

            }
            return LDTO;
            
        }

         public  bool checkLogin(string username, string password)
        {
            if (username.Equals("PayitDealerWebV1Test") || password.Equals("PayitDealerWebV1Test"))
                return true;
            return false;

         }
        private List<PaymentChannel> getPaymetChannels(String CountryCode)
        {
            List<PaymentChannel> pcs = new List<PaymentChannel>();
           // PaymentChannel pc = new PaymentChannel { PaymentChannelCode = "ISYSWALLET", CurrencyCode = "KWD",PaymentChannelName="MyAccount" };
           // PaymentChannel pc2 = new PaymentChannel { PaymentChannelCode = "KWKNETDC", CurrencyCode = "KWD", PaymentChannelName = "Knet" };
            //pcs.Add(pc);

            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("DelaerWebV1", "dEAlerwEB170409");
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetPaymentChannels2");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = client.GetAsync("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetPaymentChannels2?CountryCode="+ CountryCode).Result;
            Task<string> ss = response.Content.ReadAsStringAsync();
            var pag = JsonConvert.DeserializeObject<List<PaymentChannelResp>>(ss.Result);
            if (pag!=null)
            {
                if (pag.Count>0)
                {
                    foreach (var item in pag)
                    {
                        PaymentChannel pc = new PaymentChannel { PaymentChannelCode = item.PaymentChannelCode, CurrencyCode = item.PaymentChannelThreeCurrencyCode, PaymentChannelName =item.PaymentChannelName,PaymentChannelID=item.PaymentChannelID,Image=item.Images!=null? item.Images.ImageURL1:null };
                        pcs.Add(pc);

                    }

                }

            }
            return pcs;
        }

    }
}
