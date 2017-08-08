using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayitDealerGlobalPayit.DAL;
using PayitDealerGlobalPayit.Models.Outputs;
using PayitDealerGlobalPayit.Common;

namespace PayitDealerGlobalPayit.BL
{
    public class GetAccountServices
    {
        PayitGlobalDealersDBEntities db = new PayitGlobalDealersDBEntities();
        GlobalPayitEntities db2 = new GlobalPayitEntities();
        public List<AccountServices> GetAccountService(int AccountID)
        {
            List<AccountServices> ACs= new List<AccountServices>();
            var AcSer=(from a in db.Tbl_Account_Balance where a.AccountId==AccountID select a.ServiceId).ToList();
            if (AcSer.Count()>0)
            {
                var companies = (from a in db2.Services where AcSer.Contains(a.ID) select a.CompanyID).ToList();
                if (companies.Count()>0)
                {
                   
                   
                    foreach (var item in companies)
                    {
                        AccountServices Ac = new AccountServices();
                        Ac.CompanyID = item.Value;
                        List<int> services = (from a in db2.Services where a.CompanyID == item.Value && AcSer.Contains(a.ID) select a.ID).ToList();
                        Ac.ServiceID = services;
                        ACs.Add(Ac);
                    }
                }
                

            }
            return ACs;
        }
        //public List<string> GetAccountService2(int AccountID)
        //{
        //    List<string> list = new List<string>();
        //    List<int?> AcSer = (from a in this.db.Tbl_Account_Balance
        //                        where a.AccountId == AccountID 
        //                        select a.ServiceId).ToList<int?>();
        //    if (AcSer.Count<int?>() > 0)
        //    {
        //        list = (from a in this.db2.Services
        //                where AcSer.Contains(a.ID)
        //                select a.ServiceCode).ToList<string>();
        //    }
        //    return list;
        //}


        // updated to display active services in app
        public List<string> GetAccountService2(int AccountID,string usertype)
        {
            List<string> list = new List<string>();
            var AcSer = new List<int?>();
            var Agser = new List<int?>();
            var Dlrser = new List<int>();
            if (usertype == GlobalDealerEnums.Role.AccountUser.ToString())
            {
                 AcSer = (from ab in this.db.Tbl_Account_Balance
                          join ac in this.db.Tbl_Accounts on ab.AccountId equals ac.AccountId
                            join  a in db.Tbl_Agent_Balance on ac.AgentId equals a.AgentId
                             join b in db.Agents on a.AgentId equals b.Id
                             join c in db.Tbl_Dealer_Balance on b.DealerId equals c.DealerId
                             where a.Status == true && c.Status == true && ac.AccountId == AccountID
                             select a.ServiceId).ToList();               

                if (AcSer.Count > 0)
                {
                    list = (from a in this.db2.Services
                            where AcSer.Contains(a.ID)
                            select a.ServiceCode).ToList<string>();
                }
            }
            else if (usertype == GlobalDealerEnums.Role.Agent.ToString())
            {

                (from a in db.Tbl_Dealer_Balance
                 join b in db.Dealers on a.DealerId equals b.Id
                 join c in db.Agents on b.Id equals c.DealerId
                 join d in db.Tbl_Accounts on c.Id equals d.AgentId
                 where Agser.Contains(a.ServiceId) && a.Status == true && d.AccountId == AccountID
                 select a.ServiceId).ToList();
                Agser = (from a in db.Tbl_Agent_Balance
                             join b in db.Agents on a.AgentId equals b.Id
                             join c in db.Tbl_Dealer_Balance on b.DealerId equals c.DealerId
                             where  a.Status == true && c.Status==true && a.AgentId == AccountID
                             select a.ServiceId).ToList();
                if (Agser.Count > 0)
                {
                    list = (from a in this.db2.Services
                            where Agser.Contains(a.ID)
                            select a.ServiceCode).ToList<string>();
                }
            }
            else
            {
                Dlrser = (from a in db.Tbl_Dealer_Balance
                         
                          where  a.Status == true && a.DealerId == AccountID
                          select a.ServiceId).ToList();

                if (Dlrser.Count > 0)
                {
                    list = (from a in this.db2.Services
                            where Dlrser.Contains(a.ID)
                            select a.ServiceCode).ToList<string>();
                }
            }

          



            


            
            return list;
        }


        public Models.Outputs.ServiceAmountsDTO GetServiceAmounts_old(string username, string password, String DeviceID)
        {
            Models.Outputs.ServiceAmountsDTO SDTO = new Models.Outputs.ServiceAmountsDTO();

            if (username.Equals(String.Empty) || password.Equals(String.Empty) || password.Equals(String.Empty))
            {
                // LDTO.loginstatus = "false";

            }
            else
            {
                var Validate = (from a in db.users where a.name == username && a.password == password && a.RoleID == 6 select a).FirstOrDefault();
                if (Validate != null)
                {
                    //DeviceID = DeviceID.Trim();
                    //int DevID = (from a in db.Devices where a.IMEI == DeviceID select a.Id).FirstOrDefault();

                    var Names = (from a in db.Tbl_Account_Users
                                 join b in db.Tbl_Accounts on a.AccountId equals b.AccountId
                                 join c in db.Agents on b.AgentId equals c.Id
                                 where a.AccountUserId == Validate.EntityId && a.UserId == Validate.UserId
                                 select new { b.AccountName, c.AgentName, a.AccountId }).FirstOrDefault();
                    if (Names != null)
                    {
                        SDTO.AccountName = Names.AccountName;
                        SDTO.AgentName = Names.AgentName;


                        List<ServiceAmounts> SAs = new List<ServiceAmounts>();

                        var AcSer = (from a in db.Tbl_Account_Balance where a.AccountId == Names.AccountId select new { a.ServiceId, a.Balance }).ToList();
                        if (AcSer.Count() > 0)
                        {
                            var companies = (from b in AcSer join a in db2.Services on b.ServiceId equals a.ID select new { b, a.ServiceName }).ToList();
                            if (companies.Count() > 0)
                            {
                                foreach (var item in companies)
                                {
                                    ServiceAmounts SA = new ServiceAmounts();

                                    SA.ServiceID = Convert.ToInt32(item.b.ServiceId);
                                    SA.ServiceName = item.ServiceName;
                                    SA.Amount = Convert.ToDouble(item.b.Balance);
                                    SAs.Add(SA);
                                }
                            }

                        }
                        SDTO.ServiceAmounts = SAs;

                    }
                }


            }
            return SDTO;
        }


        public ServiceAmountsDTO GetServiceAmounts(string username, string password, string DeviceID)
        {
            ServiceAmountsDTO sdto = new ServiceAmountsDTO();
            if ((!username.Equals(string.Empty) && !password.Equals(string.Empty)) && !password.Equals(string.Empty))
            {
                user Validate = (from a in this.db.users
                                 where ((a.name == username) && (a.password == password)) && (a.RoleID == 6)
                                 select a).FirstOrDefault<user>();
                if (Validate == null)
                {
                    return sdto;
                }
                var Names = (from a in db.Tbl_Account_Users
                             join b in db.Tbl_Accounts on a.AccountId equals b.AccountId 
                             join c in db.Agents on b.AgentId equals c.Id 
                            
                             where (a.AccountUserId == Validate.EntityId) && (a.UserId == Validate.UserId)
                             select new
                             {
                                 AccountName = b.AccountName,
                                 AgentName = c.AgentName,
                                 AccountId = a.AccountId
                             }).FirstOrDefault();
                if (Names == null)
                {
                    return sdto;
                }

                var Balance = (from a in db.Tbl_Account_Users
                               join b in db.Tbl_Account_WalletBalance on a.AccountId equals b.AccountId
                               where (a.AccountUserId == Validate.EntityId) && (a.UserId == Validate.UserId)
                               select new
                               {
                                   AccountWalletBalence = b.Balance,
                                   AccountWalletCurrencyCode = b.CurrencyCode
                               }).FirstOrDefault();
                                   

                sdto.AccountName = Names.AccountName;
                sdto.AgentName = Names.AgentName;
                sdto.AccountWalletBalence = Balance != null ? Balance.AccountWalletBalence.ToString() : "";
                sdto.AccountWalletCurrencyCode = Balance != null ? Balance.AccountWalletCurrencyCode : "";
                List<ServiceAmounts> list = new List<ServiceAmounts>();
                var source = (from a in db.Tbl_Account_Balance
                              where a.AccountId == Names.AccountId
                              select new
                              {
                                  ServiceId = a.ServiceId,
                                  Balance = a.Balance
                              }).ToList();
                if (source.Count() > 0)
                {
                    var list3 = (from b in source
                                 join a in this.db2.Services on b.ServiceId equals a.ID
                                 select new
                                 {
                                     b = b,
                                     ServiceName = a.ServiceName
                                 }).ToList();
                    if (list3.Count() > 0)
                    {
                        foreach (var type in list3)
                        {
                            ServiceAmounts item = new ServiceAmounts
                            {
                                ServiceID = Convert.ToInt32(type.b.ServiceId),
                                ServiceName = type.ServiceName,
                                Amount = Convert.ToDouble(type.b.Balance),
                                translations = getTranslationForText(type.ServiceName)
                            };
                            list.Add(item);
                        }
                    }
                }
                sdto.ServiceAmounts = list;
            }
            return sdto;
        }
        public List<TranslatedText> getTranslationForText(string sourceText)
        {
            GlobalPayitEntities entities = new GlobalPayitEntities();
            List<TranslatedText> list = new List<TranslatedText>();
            List<Translation> list2 = (from tbl in entities.Translations
                                       where tbl.EnglishText.ToLower().Equals(sourceText.ToLower()) && (tbl.Status == true)
                                       select tbl).ToList<Translation>();
            if (list2 != null)
            {
                foreach (Translation translation in list2)
                {
                    TranslatedText item = new TranslatedText
                    {
                        langCode = translation.LanguageCode,
                        sourceText = translation.EnglishText,
                        translatedText = translation.TranslatedText
                    };
                    list.Add(item);
                }
            }
            return list;
        }

    }
}
