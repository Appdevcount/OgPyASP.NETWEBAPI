using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayitDealerGlobalPayit.DAL;
using PayitDealerGlobalPayit.Models.Inputs;
using PayitDealerGlobalPayit.Models;
using PayitDealerGlobalPayit.Models.Outputs;
using System.Linq.Expressions;
using System.Reflection;
using PayitDealerGlobalPayit.Common;

namespace PayitDealerGlobalPayit.BL
{
    public class Process
    {
        PayitGlobalDealersDBEntities db = new PayitGlobalDealersDBEntities();

        public Double GetAccountBalance(int AccountID, int ServiceID, Double Balance)
        {
            Double bal = 0.0;

            var balance = (from a in db.Tbl_Account_Balance where a.AccountId == AccountID && a.ServiceId == ServiceID select a.Balance).FirstOrDefault();
            if (balance != null)
            {
                bal = Convert.ToDouble(balance);
            }

            return bal;
        }
        public List<Double> UpdateAccountBalance(String BalanceType, String Type, String Id, int AccountID, int ServiceID, Double Balance, Double ServiceBalance, Double ConverionAmount)
        {
            double DedutedAmount = Balance, Commission = 0.0;
            if (Type ==GlobalDealerEnums.Role.AccountUser.ToString())
            {
                if (BalanceType == "UpFront")
                {

                    var balance = (from a in db.Tbl_Account_Balance where a.AccountId == AccountID && a.ServiceId == ServiceID select a).FirstOrDefault();
                    if (balance != null)
                    {
                        balance.Balance = balance.Balance - Balance;
                        db.SaveChanges();
                    }
                }
                else
                {
                    var balance = (from a in db.Tbl_Account_WalletBalance where a.AccountId == AccountID && a.CurrencyCode == Id select a).FirstOrDefault();
                    if (balance != null)
                    {
                        balance.Balance = balance.Balance - Balance;
                        db.SaveChanges();
                    }
                }
            }
            else if (Type == GlobalDealerEnums.Role.Agent.ToString())
            {
                //  var Agid = (from a in db.Tbl_Accounts where a.AccountId == AccountID select a.AgentId).FirstOrDefault();

                var rows = (from a in db.Tbl_Agent_Balance
                            join b in db.AgentServiceCommisions on a.AgentBalanceId equals b.AgentBalanceID
                            orderby b.Threshold
                            where a.AgentId == AccountID && a.ServiceId == ServiceID && a.Status == true
                            select b).ToList();
               
                if (rows.Count > 0)
                {
                    Commission = GetAgentCommission(rows, ServiceBalance);
                    DedutedAmount = Balance - Commission;
                }
                if (BalanceType == "UpFront")
                {

                    var balance = (from a in db.Tbl_Agent_Balance where a.AgentId == AccountID && a.ServiceId == ServiceID select a).FirstOrDefault();
                    if (balance != null)
                    {
                        balance.Balance = balance.Balance - DedutedAmount;
                        db.SaveChanges();
                    }
                }
                else
                {
                    var balance = (from a in db.Tbl_Agent_WalletBalance where a.AgentId == AccountID && a.CurrencyCode == Id select a).FirstOrDefault();
                    if (balance != null)
                    {
                        balance.Balance = balance.Balance - DedutedAmount;
                        GlobalData.UserBalance = balance.Balance.ToString();

                        db.SaveChanges();
                    }
                }
            }
            else
            {
                if (Type == GlobalDealerEnums.Role.Dealer.ToString())
                {
                    

                    var rows = (from a in db.Tbl_Dealer_Balance join b in db.DealerServiceCommisions on a.DealerBalanceId equals b.DealerBalanceID
                                orderby b.Threshold
                                where a.DealerId == AccountID && a.ServiceId== ServiceID && a.Status==true && b.Status==true select b).ToList();
                    // var Dealerid = (from a in db.Tbl_Dealer_Balance where a.DealerId == AccountID select a.DealerId).FirstOrDefault();
                      if (rows.Count>0)
                    {
                        Commission= GetDealerCommission(rows, ServiceBalance);
                        //Commission = Commission * ConverionAmount;
                        DedutedAmount = Balance - Commission;
                    }
                    else
                    {
                        var defalutComs = (from a in db.DServiceCommisions where a.ServiceID == ServiceID && a.Status == true orderby a.Threshold select a).ToList();
                        if (defalutComs.Count>0)
                        {
                            Commission = GetServiceCommission(defalutComs, ServiceBalance);
                            if (ConverionAmount>0)
                            {
                                Commission = Commission * ConverionAmount;
                            }
                            
                            DedutedAmount = Balance - Commission;

                        }
                    }

                    if (BalanceType == "UpFront")
                    {

                        var balance = (from a in db.Tbl_Dealer_Balance where a.DealerId == AccountID && a.ServiceId == ServiceID select a).FirstOrDefault();
                        if (balance != null)
                        {
                            balance.Balance = balance.Balance-DedutedAmount;
                           // GlobalData.UserBalance = balance.Balance.ToString();
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var balance = (from a in db.Tbl_Dealer_WalletBalance where a.DealerId == AccountID && a.CurrencyCode == Id select a).FirstOrDefault();
                        if (balance != null)
                        {
                            balance.Balance = balance.Balance- DedutedAmount;
                            GlobalData.UserBalance = balance.Balance.ToString();
                            db.SaveChanges();
                        }
                    }

                }
            }
            return new List<Double> { DedutedAmount, Commission };
        }
             
        private Double GetServiceCommission(List<DServiceCommision> rows, Double Balance)
        {
            Double Commission = 0;
            foreach (var item in rows)
            {
                if (item.ThresholdType.Equals("LESSTHAN"))
                {
                    if (Balance < item.Threshold)
                    {
                        if (item.ComType == "FIXED")
                        {
                            Commission = item.ComValue ?? 0;
                        }
                        else
                        {
                            Commission = (Balance * item.ComValue ?? 0) / 100;

                        }
                        break;
                    }
                }
                else
                {
                    if (Balance > item.Threshold)
                    {
                        if (item.ComType == "FIXED")
                        {
                            Commission = item.ComValue ?? 0;
                        }
                        else
                        {
                            Commission = (Balance * item.ComValue ?? 0) / 100;

                        }

                    }
                }
            }
            return Commission;
        }
        private Double GetDealerCommission(List<DealerServiceCommision> rows,Double Balance)
        {
            Double Commission = 0;
            foreach (var item in rows)
            {
                if (item.ThresholdType.Equals("LESSTHAN"))
                {
                    if (Balance < item.Threshold)
                    {
                        if (item.ComType == "FIXED")
                        {
                            Commission = item.ComValue ?? 0;
                        }
                        else
                        {
                            Commission = (Balance * item.ComValue ?? 0) / 100;

                        }
                        break;
                    }
                }
                else
                {
                    if (Balance > item.Threshold)
                    {
                        if (item.ComType == "FIXED")
                        {
                            Commission = item.ComValue ?? 0;
                        }
                        else
                        {
                            Commission = (Balance * item.ComValue ?? 0) / 100;

                        }

                    }
                }
            }
            return Commission;
        }
        private Double GetAgentCommission(List<AgentServiceCommision> rows, Double Balance)
        {
            Double Commission = 0;
            foreach (var item in rows)
            {
                if (item.ThresholdType.Equals("LESSTHAN"))
                {
                    if (Balance < item.Threshold)
                    {
                        if (item.ComType == "FIXED")
                        {
                            Commission = item.ComValue ?? 0;
                        }
                        else
                        {
                            Commission = (Balance * item.ComValue ?? 0) / 100;

                        }
                        break;
                    }
                }
                else
                {
                    if (Balance > item.Threshold)
                    {
                        if (item.ComType == "FIXED")
                        {
                            Commission = item.ComValue ?? 0;
                        }
                        else
                        {
                            Commission = (Balance * item.ComValue ?? 0) / 100;

                        }

                    }
                }
            }
            return Commission;
        }
        public String InsertTransaction(String BalanceType, String Type, String Id, Models.Input<Models.Inputs.ProcessIN> obj, Models.Outputs.ProcessDTO rep, String FailureReason, String Status)
        {
            try
            {
                String Trackid = "";

                Random rnd = new Random();
                DealerTransaction tr = new DealerTransaction();

                var ACCDeatils = (from a in db.Tbl_Accounts
                                  join b in db.Agents on a.AgentId equals b.Id
                                  join c in db.Dealers on b.DealerId equals c.Id
                                  where a.AccountId == obj.input.AccountId
                                  select new { a.AccountName, a.AgentId, b.AgentName, b.DealerId, c.DealerName }).FirstOrDefault();
                if (ACCDeatils != null)
                {
                    tr.AgentId = Convert.ToInt32(ACCDeatils.AgentId);
                    tr.DealerId = ACCDeatils.DealerId;
                    tr.AgentName = ACCDeatils.AgentName;
                    tr.DealerName = ACCDeatils.DealerName;
                    tr.AccountName = ACCDeatils.AccountName;

                }
                tr.AccountId = obj.input.AccountId;
                tr.AccountUser = obj.param.username;
                tr.AccountUserId = obj.input.AccountUserId;
                tr.ServiceId = obj.input.ServiceId;
                tr.ServiceCode = obj.input.seriviceCode;
                tr.ServiceRefference = obj.input.serviceName;
                tr.Amount = obj.input.PaymentAmount;
                tr.CountryCode = obj.input.countryCode;
                tr.DeviceId = obj.input.DeviceID;
                tr.DeviceIMEI = (from a in db.Devices where a.Id == obj.input.DeviceID select a.IMEI).FirstOrDefault();
                tr.MobileNumber = obj.input.msisdn;
                tr.PaymentChannel = obj.input.PaymentChannel;
                tr.TrackID = obj.input.transactionId;

                if (rep != null)
                {

                    tr.OperatorReference = rep.operatorRef;
                    tr.PaymentReference = "";
                    tr.Pin = rep.pin;
                    tr.Serial = rep.serial;
                    if (rep.status != null)
                    {
                        tr.Status = rep.status;
                        tr.Details = rep.statusDescription;
                    }
                    else
                    {
                        tr.Status = Status;
                        tr.Details = FailureReason;
                    }
                }
                else
                {
                    tr.Status = Status;
                    tr.Details = FailureReason;
                }


                tr.TransactionDate = DateTime.Now;
                tr.UpdatedOn = DateTime.Now;
                tr.info1 = BalanceType;
                tr.info2 = Type;
                tr.info3 = Id;

                db.DealerTransactions.Add(tr);
                db.SaveChanges();

                Trackid = tr.Id.ToString();



                return Trackid;
            }
            catch (Exception ex)
            {
                return "ERROR" + ex.Message;
            }
        }

        public string InsertTransaction2(string BalanceType, string Type, string Id, Input<ProcessIN2> obj, ProcessDTO rep, string FailureReason, string Status,List<Double> Balances)
        {
            try
            {
                ParameterExpression expression12;
                new Random();
                DealerTransaction entity = new DealerTransaction();
                if (obj.input.usertype==GlobalDealerEnums.Role.AccountUser.ToString())
                {
                    var type = (from a in db.Tbl_Accounts  join b in db.Agents on a.AgentId equals (int?)b.Id join c in db.Dealers on b.DealerId equals c.Id
                                where a.AccountId == obj.input.AccountId
                                select new
                                { AccountName = a.AccountName,   AgentId = a.AgentId,   AgentName = b.AgentName, DealerId = b.DealerId, DealerName = c.DealerName }).FirstOrDefault();
                    if (type != null)
                    {
                        entity.AgentId = new int?(Convert.ToInt32(type.AgentId));
                        entity.DealerId = new int?(type.DealerId);
                        entity.AgentName = type.AgentName;
                        entity.DealerName = type.DealerName;
                        entity.AccountName = type.AccountName;
                    }
                    entity.AccountId = new int?(obj.input.AccountId);
                    entity.AccountUser = (from a in this.db.users where (a.EntityId == obj.input.AccountUserId) && (a.RoleID == 6)  select a.name).FirstOrDefault();
                    entity.AccountUserId = new int?(obj.input.AccountUserId);
                   
                }
                else if (obj.input.usertype == GlobalDealerEnums.Role.Agent.ToString())
                {
                    var type2 = (from b in db.Agents 
                                join c in db.Dealers on b.DealerId equals c.Id
                                where b.Id == obj.input.AccountId
                                select new
                                { AccountName = "", AgentId =b.Id, AgentName = b.AgentName, DealerId = b.DealerId, DealerName = c.DealerName }).FirstOrDefault();
                    if (type2 != null)
                    {
                        entity.AgentId = new int?(Convert.ToInt32(type2.AgentId));
                        entity.DealerId = new int?(type2.DealerId);
                        entity.AgentName = type2.AgentName;
                        entity.DealerName = type2.DealerName;
                        entity.AccountName = type2.AccountName;
                    }
                    //entity.AccountId = new int?(obj.input.AccountId);
                    //entity.AccountUser = (from a in this.db.users where (a.EntityId == obj.input.AccountUserId) && (a.RoleID == 6) select a.name).FirstOrDefault();
                    //entity.AccountUserId = new int?(obj.input.AccountUserId);
                   
                }
                else 
                {
                    var type2 = (from  c in db.Dealers 
                                 where c.Id == obj.input.AccountId
                                 select new
                                 { AccountName = "", AgentId = 0, AgentName = "", DealerId = c.Id, DealerName = c.DealerName }).FirstOrDefault();
                    if (type2 != null)
                    {
                        entity.AgentId = new int?(Convert.ToInt32(type2.AgentId));
                        entity.DealerId = new int?(type2.DealerId);
                        entity.AgentName = type2.AgentName;
                        entity.DealerName = type2.DealerName;
                        entity.AccountName = type2.AccountName;
                    }
                    //entity.AccountId = new int?(obj.input.AccountId);
                    //entity.AccountUser = (from a in this.db.users where (a.EntityId == obj.input.AccountUserId) && (a.RoleID == 6) select a.name).FirstOrDefault();
                    //entity.AccountUserId = new int?(obj.input.AccountUserId);
                }

                entity.ServiceId = new int?(obj.input.ServiceId);
                entity.ServiceCode = obj.input.serviceName;
                entity.ServiceRefference = obj.input.serviceName;
                entity.Amount = new double?(Convert.ToDouble(obj.input.paymentamount));
                entity.CountryCode = obj.input.countryCode;
                entity.DeviceId = new int?(obj.input.DeviceID);
                entity.DeviceIMEI = (from a in this.db.Devices
                                     where a.Id == obj.input.DeviceID
                                     select a.IMEI).FirstOrDefault<string>();
                entity.MobileNumber = obj.input.msisdn;
                entity.PaymentChannel = obj.input.paymentChannelCode;
                entity.TrackID = obj.input.transactionId;

                if (rep != null)
                {
                    entity.OperatorReference = rep.operatorRef;
                    entity.PaymentReference = "";
                    entity.Pin = rep.pin;
                    entity.Serial = rep.serial;
                    if (rep.status != null)
                    {
                        entity.Status = rep.status;
                        entity.Details = rep.statusDescription;
                    }
                    else
                    {
                        entity.Status = Status;
                        entity.Details = FailureReason;
                    }
                }
                else
                {
                    entity.Status = Status;
                    entity.Details = FailureReason;
                }
                entity.TransactionDate = new DateTime?(DateTime.Now);
                entity.UpdatedOn = new DateTime?(DateTime.Now);
                entity.info1 = BalanceType;
                entity.info2 = Type;
                entity.info3 = Id;
                entity.DeductedAmount = Balances[0];
                entity.Commission = Balances[1];
                this.db.DealerTransactions.Add(entity);
                this.db.SaveChanges();
                return entity.Id.ToString();
            }
            catch (Exception exception)
            {
                return ("ERROR" + exception.Message);
            }
        }

    }
}
