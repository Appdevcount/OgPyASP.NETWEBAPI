using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayitDealerGlobalPayit.Services.Interfaces;
using PayitDealerGlobalPayit.BL;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PayitDealerGlobalPayit.DAL;
using PayitDealerGlobalPayit.Common;
using PayitDealerGlobalPayit.Models;
using System.Web.Script.Serialization;
using PayitDealerGlobalPayit.Models.Outputs;
using PayitDealerGlobalPayit.Models.Inputs;

namespace PayitDealerGlobalPayit.Services.Implementations
{
    public class Dealers : IDealers
    {
        private PayitGlobalDealersDBEntities db = new PayitGlobalDealersDBEntities();

        private GlobalPayitEntities db2 = new GlobalPayitEntities();
        public Models.DTO<Models.Outputs.LoginDTO> login(Models.Input<Models.Inputs.LoginIN> obj)
        {
            Models.DTO<Models.Outputs.LoginDTO> op = new Models.DTO<Models.Outputs.LoginDTO>();
            op.objname = "Login";
            Models.Outputs.LoginDTO rep = new Models.Outputs.LoginDTO();
            try
            {

                if (!isHashValid(obj.secure))
                {
                    throw new Exception("Tampered Data : Invalid Secure Hash");
                }


                if (!isValidObj(obj, obj.secure.data))
                {
                    throw new Exception("Tampered Data : Requests doesn't match");
                }

                Authentication At = new Authentication();
                var Result=At.checkLoginwithDeviceID(obj.param.username, obj.param.password);

                if (Result.loginstatus=="true")
                {
                    rep = Result;
                    op.status = new Models.Status(0);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.AccountUserInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(101);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.AccountInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(102);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.AgentInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(103);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.DealerInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(104);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.AccountDeviceInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(105);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.AgentDeviceInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(106);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == (int)PayitDealerGlobalPayit.Common.GlobalDealerEnums.LoginMessageCode.DealerDeviceInactive)
                {
                    rep = Result;
                    op.status = new Models.Status(107);
                }
                else if (Result.loginstatus == "false" && Result.LoginStatusCode == 404)
                {
                    rep = Result;
                    op.status = new Models.Status(404);
                }
                
               
            }
            catch (Exception ex)
            {
                Common.TraceLog.WriteToLog(ex.Message.ToString());

                op.status = new Models.Status(1);
                op.status.info1 = ex.Message;
            }
            op.response = rep;
            return op;

        }

        public Models.DTO<Models.Outputs.ServicesDTO> GetServicesByCountry(Models.Input<Models.Inputs.ServicesIN> obj)
        {
            Models.DTO<Models.Outputs.ServicesDTO> op = new Models.DTO<Models.Outputs.ServicesDTO>();
            op.objname = "GetServices";
            Models.Outputs.ServicesDTO Sop = new Models.Outputs.ServicesDTO();

            try
            {

                if (!isHashValid(obj.secure))
                {
                    throw new Exception("Tampered Data : Invalid Secure Hash");
                }


                if (!isValidObj(obj, obj.secure.data))
                {
                    throw new Exception("Tampered Data : Requests doesn't match");
                }
               
                GetAccountServices GS = new GetAccountServices();
               List<Models.Outputs.AccountServices> ACs=GS.GetAccountService(obj.input.AccountID);
               Sop.AccountServices = ACs;

                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new NetworkCredential("PayitDealerWebV1Test", "PayitDealerWebV1Test");
                HttpClient client = new HttpClient(handler);
                client.BaseAddress = new Uri("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetActiveServicesByCountry1");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                Models.Inputs.GetActiveServices rep = obj.input.GetActiveServices;


                string json = JsonConvert.SerializeObject(rep);
                StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetActiveServicesByCountry1", theContent).Result;
               
                Task<string> ss = response.Content.ReadAsStringAsync();
                var pag = JsonConvert.DeserializeObject<Models.Outputs.mainServices>(ss.Result);

                Sop.mainServices = pag;
                op.status = new Models.Status(0);
            }
            catch (Exception ex)
            {
                op.status = new Models.Status(1);
                op.status.info1 = ex.Message;

            }
            op.response = Sop;
            return op;
        }

        public Models.DTO<Models.Outputs.CategoryServices> GetServicesByCountry2(Models.Input<Models.Inputs.ServicesIN> obj)
        {
            DTO<Models.Outputs.CategoryServices> dto = new DTO<Models.Outputs.CategoryServices>
            {
                objname = "GetServices"
            };
            CategoryServices services = new CategoryServices();
            try
            {
                Func<ServiceGroup, bool> predicate = null;
                if (!isHashValid(obj.secure))
                {
                    throw new Exception("Tampered Data : Invalid Secure Hash");
                }
                if (!isValidObj(obj, obj.secure.data))
                {
                    throw new Exception("Tampered Data : Requests doesn't match");
                }
                List<string> ACs = new GetAccountServices().GetAccountService2(obj.input.AccountID,obj.input.usertype);
                HttpClientHandler handler = new HttpClientHandler
                {
                    Credentials = new NetworkCredential("PayitDealerWebV1Test", "PayitDealerWebV1Test")
                };
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetActiveServicesByCountryForDealer"),
                    DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } }
                };
                String json = JsonConvert.SerializeObject(obj.input.GetActiveServices);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                services = JsonConvert.DeserializeObject<CategoryServices>(client.PostAsync("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetActiveServicesByCountryForDealer", content).Result.Content.ReadAsStringAsync().Result);
                dto.status = new Status(0);
                if (services.StatusInfo.StatusDescription.ToLower() == "success")
                {
                    foreach (CategoryService service in services.ServiceCategories)
                    {
                        if (predicate == null)
                        {
                            predicate = a => ACs.Contains(a.ServiceCode);
                        }
                        service.ServiceGroups = (from a in service.ServiceGroups
                                                 orderby a.Priority
                                                 select a).Where<ServiceGroup>(predicate).ToList<ServiceGroup>();

                        foreach (var item in service.ServiceGroups)
                        {
                            if (predicate == null)
                            {
                                predicate = a => ACs.Contains(a.ServiceCode);
                            }
                            item.Services= (from a in item.Services where ACs.Contains(a.ServiceCode) select a).ToList();
                            // orderby a.Priority

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                dto.status = new Status(1);
                dto.status.info1 = exception.Message;
            }
            dto.response = services;
            return dto;
        }

        public Models.DTO<Models.Outputs.ServiceDetailsDTO2> GetServicesDetails(Models.Inputs.ServiceDetailsIN obj)
        {
            DTO<Models.Outputs.ServiceDetailsDTO2> resp = new DTO<Models.Outputs.ServiceDetailsDTO2>();
            Models.Outputs.ServiceDetailsDTO op = new Models.Outputs.ServiceDetailsDTO();
            Models.Outputs.ServiceDetailsDTO2 op2 = new ServiceDetailsDTO2();
            resp.status = new Status(0);

            try
            {

                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new NetworkCredential("PayitDealerWebV1Test", "PayitDealerWebV1Test");
                HttpClient client = new HttpClient(handler);
                client.BaseAddress = new Uri("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/" + "GetServiceDetailsforDealers");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                string json = JsonConvert.SerializeObject(obj);
                StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/" + "GetServiceDetailsforDealers", theContent).Result;

                Task<string> ss = response.Content.ReadAsStringAsync();
                var pag = JsonConvert.DeserializeObject<Models.Outputs.ServiceDetailsDTO>(ss.Result);
                op = pag;

                if (op!=null)
                {
                    op2.ServiceCategory = op.ServiceCategory;
                    op2.ConversionAmount = op.ConversionAmount;
                    op2.Validations = op.Validations;
                    op2.StatusInfo = op.StatusInfo;

                    ServiceDetails sd = new ServiceDetails();
                    if (op.TelecomServiceDetails!=null)
                    {
                        sd.CompanyID = op.TelecomServiceDetails.CompanyID;
                        sd.CompanyName = op.TelecomServiceDetails.CompanyName;
                        sd.ComapnyNameAR = op.TelecomServiceDetails.ComapnyNameAR;
                        sd.Images = op.TelecomServiceDetails.Images;
                        service s = new service();
                        if (op.TelecomServiceDetails.ServicePrepaid!=null && obj.ServiceCode.ToLower().EndsWith("-x"))
                        {
                            s.ServiceID = op.TelecomServiceDetails.ServicePrepaid.ServiceID;
                            s.ServiceCode = op.TelecomServiceDetails.ServicePrepaid.ServiceCode;
                            s.ServiceName = op.TelecomServiceDetails.ServicePrepaid.ServiceName;
                            s.ServiceConfigID = op.TelecomServiceDetails.ServicePrepaid.ServiceConfigID;
                            s.ServiceNameAR = op.TelecomServiceDetails.ServicePrepaid.ServiceNameAR;
                            s.ServiceType = op.TelecomServiceDetails.ServicePrepaid.ServiceType;
                            s.CurrencyID = op.TelecomServiceDetails.ServicePrepaid.CurrencyID;
                            s.ServiceCurrencyID = op.TelecomServiceDetails.ServicePrepaid.ServiceCurrencyID;
                            s.ServiceCurrency = op.TelecomServiceDetails.ServicePrepaid.ServiceCurrency;
                            s.ServiceTwoCurrencyCode = op.TelecomServiceDetails.ServicePrepaid.ServiceTwoCurrencyCode;
                            s.ServiceThreeCurrencyCode = op.TelecomServiceDetails.ServicePrepaid.ServiceThreeCurrencyCode;
                            s.isEnabled = op.TelecomServiceDetails.ServicePrepaid.isEnabled;
                            s.PrepaidDenomincations = op.TelecomServiceDetails.ServicePrepaid.PrepaidDenomincations;
                            s.Translations = op.TelecomServiceDetails.ServicePrepaid.Translations;
                            //List<servicePrepaidDenomination> lspd = new List<servicePrepaidDenomination>();
                            //if (op.TelecomServiceDetails.ServicePrepaid.PrepaidDenomincations.Count>0)
                            //{
                            //    foreach (var item in op.TelecomServiceDetails.ServicePrepaid.PrepaidDenomincations)
                            //    {
                            //        servicePrepaidDenomination spd = new servicePrepaidDenomination();
                            //        spd.DenominationID = item.DenominationID;
                            //        spd.DenominationAmount = item.DenominationAmount;
                            //        spd.Currency = item.Currency;
                            //        lspd.Add(spd);

                            //    }
                            //}
                        }
                        else if (op.TelecomServiceDetails.ServicePostpaid != null && obj.ServiceCode.ToLower().EndsWith("-p"))
                        {
                            s.ServiceID = op.TelecomServiceDetails.ServicePostpaid.ServiceID;
                            s.ServiceCode = op.TelecomServiceDetails.ServicePostpaid.ServiceCode;
                            s.ServiceName = op.TelecomServiceDetails.ServicePostpaid.ServiceName;
                            s.ServiceConfigID = op.TelecomServiceDetails.ServicePostpaid.ServiceConfigID;
                            s.ServiceNameAR = op.TelecomServiceDetails.ServicePostpaid.ServiceNameAR;
                            s.ServiceType = op.TelecomServiceDetails.ServicePostpaid.ServiceType;
                            s.CurrencyID = op.TelecomServiceDetails.ServicePostpaid.CurrencyID;
                            s.ServiceCurrencyID = op.TelecomServiceDetails.ServicePostpaid.ServiceCurrencyID;
                            s.ServiceCurrency = op.TelecomServiceDetails.ServicePostpaid.ServiceCurrency;
                            s.ServiceTwoCurrencyCode = op.TelecomServiceDetails.ServicePostpaid.ServiceTwoCurrencyCode;
                            s.ServiceThreeCurrencyCode = op.TelecomServiceDetails.ServicePostpaid.ServiceThreeCurrencyCode;
                            s.isEnabled = op.TelecomServiceDetails.ServicePostpaid.isEnabled;                           
                            s.Translations = op.TelecomServiceDetails.ServicePrepaid.Translations;
                            
                        }
                        sd.Service = s;
                    }
                    else if (op.VoucherServiceDetails != null)
                    {
                        sd.CompanyID = op.VoucherServiceDetails.CompanyID;
                        sd.CompanyName = op.VoucherServiceDetails.CompanyName;
                        sd.ComapnyNameAR = op.VoucherServiceDetails.ComapnyNameAR;
                        sd.Images = op.VoucherServiceDetails.Images;
                        service s = new service();
                        if (op.VoucherServiceDetails.ServiceVoucher != null)
                        {
                            s.ServiceID = op.VoucherServiceDetails.ServiceVoucher.ServiceID;
                            s.ServiceCode = op.VoucherServiceDetails.ServiceVoucher.ServiceCode;
                            s.ServiceName = op.VoucherServiceDetails.ServiceVoucher.ServiceName;
                            s.ServiceConfigID = op.VoucherServiceDetails.ServiceVoucher.ServiceConfigID;
                            s.ServiceNameAR = op.VoucherServiceDetails.ServiceVoucher.ServiceNameAR;
                            s.ServiceType = op.VoucherServiceDetails.ServiceVoucher.ServiceType;
                            s.CurrencyID = op.VoucherServiceDetails.ServiceVoucher.CurrencyID;
                            s.ServiceCurrencyID = op.VoucherServiceDetails.ServiceVoucher.ServiceCurrencyID;
                            s.ServiceCurrency = op.VoucherServiceDetails.ServiceVoucher.ServiceCurrency;
                            s.ServiceTwoCurrencyCode = op.VoucherServiceDetails.ServiceVoucher.ServiceTwoCurrencyCode;
                            s.ServiceThreeCurrencyCode = op.VoucherServiceDetails.ServiceVoucher.ServiceThreeCurrencyCode;
                            s.isEnabled = op.VoucherServiceDetails.ServiceVoucher.isEnabled;
                            s.VoucherDenominations = op.VoucherServiceDetails.ServiceVoucher.VoucherDenominations;
                            s.Translations = op.VoucherServiceDetails.ServiceVoucher.Translations;
                            
                        }
                        sd.Service = s;
                    }
                    else if (op.InternationalTopupServicesDetails != null)
                    {
                        sd.CompanyID = op.InternationalTopupServicesDetails.CompanyID;
                        sd.CompanyName = op.InternationalTopupServicesDetails.CompanyName;
                        sd.ComapnyNameAR = op.InternationalTopupServicesDetails.ComapnyNameAR;
                        sd.Images = op.InternationalTopupServicesDetails.Images;
                        service s = new service();
                        if (op.InternationalTopupServicesDetails.serviceInternationalTopup != null)
                        {
                            s.ServiceID = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceID;
                            s.ServiceCode = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceCode;
                            s.ServiceName = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceName;
                            s.ServiceConfigID = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceConfigID;
                            s.ServiceNameAR = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceNameAR;
                            s.ServiceType = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceType;
                            s.CurrencyID = op.InternationalTopupServicesDetails.serviceInternationalTopup.CurrencyID;
                            s.ServiceCurrencyID = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceCurrencyID;
                            s.ServiceCurrency = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceCurrency;
                            s.ServiceTwoCurrencyCode = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceTwoCurrencyCode;
                            s.ServiceThreeCurrencyCode = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceThreeCurrencyCode;
                            s.CountryID = op.InternationalTopupServicesDetails.serviceInternationalTopup.CountryID;
                            s.ServiceCountry = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceCountry;
                            s.ServiceTwoCountryCode = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceTwoCountryCode;
                            s.ServiceThreeCountryCode = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceThreeCountryCode;
                            s.ServiceMobileCountryCode = op.InternationalTopupServicesDetails.serviceInternationalTopup.ServiceMobileCountryCode;
                            s.isEnabled = op.InternationalTopupServicesDetails.serviceInternationalTopup.isEnabled;                           
                            s.Translations = op.InternationalTopupServicesDetails.serviceInternationalTopup.Translations;                        


                            s.VoucherDenominations = op.InternationalTopupServicesDetails.serviceInternationalTopup.VoucherDenominations;

                        }
                        sd.Service = s;
                    }
                    else if (op.OtherServiceDetails != null)
                    {
                        sd.CompanyID = op.OtherServiceDetails.CompanyID;
                        sd.CompanyName = op.OtherServiceDetails.CompanyName;
                        sd.ComapnyNameAR = op.OtherServiceDetails.ComapnyNameAR;
                        sd.Images = op.OtherServiceDetails.Images;
                        service s = new service();
                        if (op.OtherServiceDetails.ServiceOther != null)
                        {
                            s.ServiceID = op.OtherServiceDetails.ServiceOther.ServiceID;
                            s.ServiceCode = op.OtherServiceDetails.ServiceOther.ServiceCode;
                            s.ServiceName = op.OtherServiceDetails.ServiceOther.ServiceName;
                            s.ServiceConfigID = op.OtherServiceDetails.ServiceOther.ServiceConfigID;
                            s.ServiceNameAR = op.OtherServiceDetails.ServiceOther.ServiceNameAR;
                            s.ServiceType = op.OtherServiceDetails.ServiceOther.ServiceType;
                            s.CurrencyID = op.OtherServiceDetails.ServiceOther.CurrencyID;
                            s.ServiceCurrencyID = op.OtherServiceDetails.ServiceOther.ServiceCurrencyID;
                            s.ServiceCurrency = op.OtherServiceDetails.ServiceOther.ServiceCurrency;
                            s.ServiceTwoCurrencyCode = op.OtherServiceDetails.ServiceOther.ServiceTwoCurrencyCode;
                            s.ServiceThreeCurrencyCode = op.OtherServiceDetails.ServiceOther.ServiceThreeCurrencyCode;                         
                            s.isEnabled = op.OtherServiceDetails.ServiceOther.isEnabled;
                            s.Translations = op.OtherServiceDetails.ServiceOther.Translations;
                            
                            List<voucherDenomination> lvd = new List<voucherDenomination>();
                            if (op.OtherServiceDetails.ServiceOther.voucherDenominationComments.Count>0)
                            {
                                foreach (var item in op.OtherServiceDetails.ServiceOther.voucherDenominationComments)
                                {
                                    voucherDenomination vd = new voucherDenomination();
                                    vd.VoucherComment = item.VoucherComment;
                                    vd.VoucherCurrencyID = item.VoucherCurrencyID;
                                    vd.VoucherDenominationAmount = item.VoucherDenominationAmount;
                                    vd.VoucherDenominationCommentID = item.VoucherDenominationCommentID;
                                    vd.VoucherDenominationID = item.VoucherDenominationID;
                                    vd.VoucherThreeCurrencyCode = item.VoucherThreeCurrencyCode;
                                    vd.VoucherTwoCurrencyCode = item.VoucherTwoCurrencyCode;
                                    lvd.Add(vd);
                                }

                            }
                            s.VoucherDenominations = lvd;

                        }
                        sd.Service = s;
                    }
                    op2.ServiceDetails = sd;
                }
            }
            catch (Exception exception)
            {
                resp.status = new Status(1);
                resp.status.info1 = exception.Message;
            }

            resp.response = op2;
            return resp;
        }
        public Models.DTO<Models.Outputs.CategoryServices>  GetAllServices(Models.Input<Models.Inputs.GetActiveServices> obj)
        {
            DTO<Models.Outputs.CategoryServices> dto = new DTO<Models.Outputs.CategoryServices>
            {
                objname = "GetServices"
            };
            CategoryServices services = new CategoryServices();
            try
            {
               
                
                HttpClientHandler handler = new HttpClientHandler
                {
                    Credentials = new NetworkCredential("PayitDealerWebV1Test", "PayitDealerWebV1Test")
                };
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetActiveServicesByCountryForDealer"),
                    DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } }
                };
                String json = JsonConvert.SerializeObject(obj.input);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                services = JsonConvert.DeserializeObject<CategoryServices>(client.PostAsync("http://192.168.1.11/GlobalPayit/GlobalPayitAPIv5/api/GlobalPayitServices/GetActiveServicesByCountryForDealer", content).Result.Content.ReadAsStringAsync().Result);
                if (services != null)
                {
                    if (services.ServiceCategories.Count > 0)
                    {
                        foreach (var item in services.ServiceCategories)
                        {
                            foreach (var item2 in item.ServiceGroups)
                            {
                                foreach (var item3 in item2.Services)
                                {
                                    item3.ServiceCommissionDetails = GetServiceCommissionDetails(item3.ServiceCode);
                                }
                            }
                        }
                    }
                }

                dto.status = new Status(0);
              
            }
            catch (Exception exception)
            {
                dto.status = new Status(1);
                dto.status.info1 = exception.Message;
            }
            dto.response = services;
            return dto;
        }
        private String GetServiceCommissionDetails(String ServiceCode)
        {
            String Commission = "";
            var rows = db.DServiceCommisions.Where(a => a.DealerName == ServiceCode && a.Status == true).ToList();
            var rows1 = rows.Where(a => a.ThresholdType == "LESSTHAN").OrderBy(a => a.Threshold).ToList();
            foreach (var item in rows1)
            {
                if (item.ComType == "FIXED")
                {
                    Commission = Commission + "Amount less than " + item.Threshold.ToString() + " " + item.Info1 + ", Get " + item.ComValue.ToString() + " " + item.Info1 + " Commission" + System.Environment.NewLine;
                }
                else
                {
                    Commission = Commission + "Amount less than " + item.Threshold.ToString() + " " + item.Info1 + ", Get " + item.ComValue.ToString() + "% Commission" + System.Environment.NewLine;
                }
               }

                var rows2 = rows.Where(a => a.ThresholdType == "GREATERTHAN").OrderByDescending(a => a.Threshold).ToList();
                foreach (var item2 in rows2)
                {
                    if (item2.ComType == "FIXED")
                    {
                        Commission = Commission + "Amount greater than " + item2.Threshold.ToString() + " " + item2.Info1 + ", Get " + item2.ComValue.ToString() + " " + item2.Info1 + " Commission" + System.Environment.NewLine;
                }
                    else
                    {
                        Commission = Commission + "Amount greater than " + item2.Threshold.ToString() + " " + item2.Info1 + ", Get " + item2.ComValue.ToString() + "% Commission" + System.Environment.NewLine;

                }

                }
            return Commission;
        }

        public Models.DTO<Models.Outputs.ProcessDTO> Process(Models.Input<Models.Inputs.ProcessIN> obj)
        {

            Models.DTO<Models.Outputs.ProcessDTO> op = new Models.DTO<Models.Outputs.ProcessDTO>();
            PayitGlobalDealersDBEntities db = new PayitGlobalDealersDBEntities();
            op.objname = "Process";
            Models.Outputs.ProcessDTO rep = new Models.Outputs.ProcessDTO();
            Process GAB = new Process();
            //try
            //{
            //    if (obj.input.PaymentChannel.ToUpper() == "CASH")
            //    {
            //        // Double AccBal = GAB.GetAccountBalance(obj.input.AccountId, obj.input.ServiceId, obj.input.PaymentAmount);

            //        var BalResult = db.GetBalance(obj.input.AccountId, obj.input.ServiceId, obj.input.PaymentAmount);
            //        foreach (var item in BalResult)
            //        {
            //            String BalanceFor = item.ID.ToString();

            //            if (item.BalanceType == "NOBAL")
            //            {

            //                String Excep = "Insufficient Balane In Account";
            //                GAB.InsertTransaction(item.BalanceType, item.Type, BalanceFor, obj, rep, Excep, "FAIL");
            //                throw new Exception("Insufficient Balane In Account");
            //            }
            //            else
            //            {

            //                rep = processCore(obj.input);
            //                if ("success".Equals(rep.statusDescription.ToLower()))
            //                {
            //                    Common.TraceLog.WriteToLog("Before Inserting into Dealers Table");
            //                    //AccBal = AccBal - obj.input.PaymentAmount;
            //                    GAB.UpdateAccountBalance(item.BalanceType, item.Type, BalanceFor, obj.input.AccountId, obj.input.ServiceId, obj.input.PaymentAmount, obj.input.ServiceAmount, Convert.ToDouble(obj.input.ConvertionAmount));
            //                    String traCkID = GAB.InsertTransaction(item.BalanceType, item.Type, BalanceFor, obj, rep, "", "SUCCESS");
            //                    Common.TraceLog.WriteToLog("Method=Process Trackid=" + traCkID);
            //                    op.status = new Models.Status(0);
            //                }
            //                else
            //                {
            //                    String traCkID = GAB.InsertTransaction(item.BalanceType, item.Type, BalanceFor, obj, rep, "", "FAIL");
            //                    op.status = new Models.Status(1);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Common.TraceLog.WriteToLog(ex.Message.ToString());
            //    op.status = new Models.Status(1);
            //    op.status.info1 = ex.Message;
            //}
            op.response = rep;
            return op;
        }

        public  Models.DTO<Models.Outputs.ProcessDTO> Process2(Models.Input<Models.Inputs.ProcessIN2> obj)
        {
            DTO<ProcessDTO> dto = new DTO<ProcessDTO>();
            PayitGlobalDealersDBEntities entities = new PayitGlobalDealersDBEntities();
            dto.objname = "Process";
            ProcessDTO rep = new ProcessDTO();
            PayitDealerGlobalPayit.BL.Process process = new PayitDealerGlobalPayit.BL.Process();

            try
            {
                if (obj.input.paymentChannelCode.ToUpper() == "CASH")
                {
                    List<Double> Balances = new List<Double> { 0, 0 };
                    foreach (GetBalanceNew_Result result2 in entities.GetBalanceNew(new int?(obj.input.AccountId), new int?(obj.input.ServiceId), new double?(Convert.ToDouble(obj.input.paymentamount)),obj.input.usertype.ToString()))
                    {

                        string id = result2.ID.ToString();
                        if (result2.BalanceType == "NOBAL")
                        {
                            string failureReason = "Insufficient Balance In Account";
                            process.InsertTransaction2(result2.BalanceType, result2.Type, id, obj, rep, failureReason, "FAIL", Balances);
                            throw new Exception("Insufficient Balance In Account");
                        }
                        long transactionID = saveTransactionComplete(obj.input);
                        if (transactionID <= 0L)
                        {
                            throw new Exception("SAVE TRANSACTION COMPLETE ERROR");
                        }
                        rep = processCore2(obj.input);
                        if ("success".Equals(rep.statusDescription.ToLower()))
                        {
                            TraceLog.WriteToLog("Before Inserting into Dealers Table");
                           Balances= process.UpdateAccountBalance(result2.BalanceType, result2.Type, id, obj.input.AccountId, obj.input.ServiceId, Convert.ToDouble(obj.input.paymentamount), Convert.ToDouble(obj.input.serviceamount),Convert.ToDouble(obj.input.ConversionAmount));
                            TraceLog.WriteToLog("Method=Process Trackid=" + process.InsertTransaction2(result2.BalanceType, result2.Type, id, obj, rep, "", "SUCCESS", Balances));
                            rep.UserBalance = GlobalData.UserBalance;
                            dto.status = new Status(0);
                        }
                        else
                        {
                            process.InsertTransaction2(result2.BalanceType, result2.Type, id, obj, rep, "", "FAIL", Balances);
                            dto.status = new Status(1);
                        }
                        if (updateTransactionComplete(rep, transactionID) <= 0L)
                        {
                            throw new Exception("UPDATE TRANSACTION COMPLETE ERROR");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                TraceLog.WriteToLog(exception.Message.ToString());
                dto.status = new Status(1);
                dto.status.info1 = exception.Message;
            }
            dto.response = rep;
            return dto;
        }

        private long updateTransactionComplete(ProcessDTO tran, long TransactionID)
        {
            try
            {
                TransactionsComplete complete = new TransactionsComplete();
                complete = (from tbl in this.db2.TransactionsCompletes
                            where tbl.ID == TransactionID
                            select tbl).FirstOrDefault<TransactionsComplete>();
                complete.ResponseTrackID = tran.operatorRef;
                if (tran.status.Equals("0"))
                {
                    complete.StatusDescription = "SUCCESS";
                }
                else
                {
                    complete.StatusDescription = "FAILED";
                }
                complete.ProcessTranDate = new DateTime?(DateTime.Now);
                complete.Info1 = tran.statusDescription;
                this.db2.SaveChanges();
                return 1L;
            }
            catch (Exception ex)
            {
                TraceLog.WriteToLog("Update Transaction Complete Exception  " + ex.Message); 
                return 0L;
            }
        }

        public Models.Outputs.ProcessDTO processCore(Models.Inputs.ProcessIN pt)
        {
            Random rnd = new Random();
            Models.Outputs.ProcessDTO _tranResult = new Models.Outputs.ProcessDTO();

            Service1Client telcoProxy = new Service1Client();
            wcfProductsAPI.data.ServiceInfo info = new wcfProductsAPI.data.ServiceInfo();



            info.msisdn = pt.msisdn;
            info.ServiceId = "0";
            info.ServiceName = pt.seriviceCode;
            info.countryCode = pt.countryCode;
            info.TransactionId = pt.transactionId;
            info.paymentRef = DateTime.Now.ToString("ddMMyyyHHmmss") + rnd.Next(1, 9999); ;
            info.amount = pt.ServiceAmount;

            wcfProductsAPI.data.ResponseOfTopupResponseHE7w_StsE tres;
            wcfProductsAPI.data.ResponseOfPayBillResponseHE7w_StsE pres;
            wcfProductsAPI.data.ResponseOfVoucherPinResponseHE7w_StsE vres;


            if (pt.seriviceCode.ToLower().EndsWith("x"))
            {
                tres = telcoProxy.Topup(info);
                _tranResult.status = tres.Status.ToString();
                _tranResult.statusDescription = tres.StatusDescription;

                if ("success".Equals(tres.StatusDescription.ToLower()))
                {
                    _tranResult.currentBalance = tres.Result.CurrentBalance.ToString(); ;
                    _tranResult.rechargedAmount = tres.Result.RechargedAmount.ToString();
                    _tranResult.operatorRef = tres.Result.OperatorReference;
                    _tranResult.customMessage = "prepaid custom message english.";
                    _tranResult.customMessageAr = "prepaid custom message arabic.";

                }
                else
                {

                    _tranResult.customMessage = tres.ErrorMessage;
                }
            }
            else if (pt.seriviceCode.ToLower().EndsWith("p"))
            {
                pres = telcoProxy.PayBill(info);
                _tranResult.status = pres.Status.ToString();
                _tranResult.statusDescription = pres.StatusDescription;

                if ("success".Equals(pres.StatusDescription.ToLower()))
                {
                    _tranResult.operatorRef = pres.Result.OperatorReference;
                    _tranResult.customMessage = "postpaid custom message english.";
                    _tranResult.customMessageAr = "postpaid custom message arabic.";
                }
                else
                {

                    _tranResult.customMessage = pres.ErrorMessage;
                }

            }
            else if (pt.seriviceCode.ToLower().EndsWith("o") || pt.seriviceCode.ToLower().EndsWith("vs"))
            {
                vres = telcoProxy.Voucher(info);
                _tranResult.status = vres.Status.ToString();
                _tranResult.statusDescription = vres.StatusDescription;

                if ("success".Equals(vres.StatusDescription.ToLower()))
                {
                    _tranResult.operatorRef = vres.Result.PinId;
                    _tranResult.serial = vres.Result.Serial;
                    _tranResult.pin = vres.Result.Pin;
                    _tranResult.customMessage = "voucher custom message english.";
                    _tranResult.customMessageAr = "voucher custom message arabic.";
                }
                else
                {

                    _tranResult.customMessage = vres.ErrorMessage;
                }
            }
            else if (pt.seriviceCode.ToLower().EndsWith("intltt"))
            {
                info.ServiceName = "IntlTopupTransferTo";
                tres = telcoProxy.Topup(info);
                _tranResult.status = tres.Status.ToString();
                _tranResult.statusDescription = tres.StatusDescription;

                if ("success".Equals(tres.StatusDescription.ToLower()))
                {
                    _tranResult.currentBalance = "";
                    _tranResult.rechargedAmount = tres.Result.RechargedAmount.ToString();
                    _tranResult.operatorRef = tres.Result.OperatorReference;
                    _tranResult.customMessage = "prepaid custom message english.";
                    _tranResult.customMessageAr = "prepaid custom message arabic.";
                }
                else
                {

                    _tranResult.customMessage = tres.ErrorMessage;
                }
            }
            else if (pt.seriviceCode.ToLower().EndsWith("r"))
            {

                tres = telcoProxy.Topup(info);
                _tranResult.status = tres.Status.ToString();
                _tranResult.statusDescription = tres.StatusDescription;

                if ("success".Equals(tres.StatusDescription.ToLower()))
                {
                    _tranResult.currentBalance = tres.Result.CurrentBalance.ToString();
                    _tranResult.rechargedAmount = tres.Result.RechargedAmount.ToString();
                    _tranResult.operatorRef = tres.Result.OperatorReference;
                    _tranResult.customMessage = "prepaid custom message english.";
                    _tranResult.customMessageAr = "prepaid custom message arabic.";
                }
                else
                {

                    _tranResult.customMessage = tres.ErrorMessage;
                }
            }

            else
            {
                //you should not be here
            }

            return _tranResult;
        }

        public ProcessDTO processCore2(ProcessIN2 pt)
        {
            wcfProductsAPI.data.ResponseOfTopupResponseHE7w_StsE se;
            wcfProductsAPI.data.ResponseOfPayBillResponseHE7w_StsE se2;
            wcfProductsAPI.data.ResponseOfDonationOutputHE7w_StsE se4;
            new Random();
            ProcessDTO sdto = new ProcessDTO();
            Service1Client client = new Service1Client();
            wcfProductsAPI.data.ServiceInfo info = new wcfProductsAPI.data.ServiceInfo
            {
                msisdn = pt.msisdn,
                ServiceId = "0",
                ServiceName = pt.serviceName,
                countryCode = pt.countryCode,
                TransactionId = pt.transactionId,
                paymentRef = pt.receiptno,
                amount = double.Parse(pt.serviceamount),
                area = pt.param1,
                serial = pt.param2,
                voucher = pt.servicetype,
                Currency = pt.servicecurrencyCode
            };
            if (!string.IsNullOrEmpty(pt.info2))
            {
                info.serial = pt.info2;
            }
            info.paymentType = (pt.paymenttype == null) ? "" : pt.paymenttype.ToUpper();
            TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessTransaction ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(info).ToString());
            if (pt.serviceName.ToLower().EndsWith("-x"))
            {
                se = client.Topup(info);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se));
                sdto.status = se.Status.ToString();
                sdto.statusDescription = se.StatusDescription;
                if ("success".Equals(se.StatusDescription.ToLower()))
                {
                    sdto.currentBalance = (se.Result.CurrentBalance == null) ? "" : se.Result.CurrentBalance.ToString();
                    sdto.rechargedAmount = (se.Result.RechargedAmount == null) ? "" : se.Result.RechargedAmount.ToString();
                    sdto.operatorRef = (se.Result.OperatorReference == null) ? "" : se.Result.OperatorReference;
                    sdto.customMessage = "prepaid custom message english.";
                    sdto.customMessageAr = "prepaid custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se.ErrorMessage;
                return sdto;
            }
            if (pt.serviceName.ToLower().EndsWith("-p"))
            {
                se2 = client.PayBill(info);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se2));
                sdto.status = se2.Status.ToString();
                sdto.statusDescription = se2.StatusDescription;
                if ("success".Equals(se2.StatusDescription.ToLower()))
                {
                    sdto.operatorRef = (se2.Result.OperatorReference == null) ? "" : se2.Result.OperatorReference;
                    sdto.customMessage = "postpaid custom message english.";
                    sdto.customMessageAr = "postpaid custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se2.ErrorMessage;
                return sdto;
            }
            if (pt.serviceName.ToLower().EndsWith("-o") || pt.serviceName.ToLower().EndsWith("-vs"))
            {
                wcfProductsAPI.data.ResponseOfVoucherPinResponseHE7w_StsE se3 = client.Voucher(info);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se3));
                sdto.status = se3.Status.ToString();
                sdto.statusDescription = se3.StatusDescription;
                if ("success".Equals(se3.StatusDescription.ToLower()))
                {
                    sdto.operatorRef = se3.Result.PinId;
                    sdto.serial = se3.Result.Serial;
                    sdto.pin = se3.Result.Pin;
                    sdto.customMessage = "voucher custom message english.";
                    sdto.customMessageAr = "voucher custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se3.ErrorMessage;
                return sdto;
            }
            if (pt.serviceName.ToLower().EndsWith("intltt"))
            {
                info.ServiceName = "IntlTopupTransferTo";
                se = client.Topup(info);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se));
                sdto.status = se.Status.ToString();
                sdto.statusDescription = se.StatusDescription;
                if ("success".Equals(se.StatusDescription.ToLower()))
                {
                    sdto.currentBalance = "";
                    sdto.rechargedAmount = (se.Result.RechargedAmount == null) ? "" : se.Result.RechargedAmount.ToString();
                    sdto.operatorRef = se.Result.OperatorReference;
                    sdto.customMessage = "prepaid custom message english.";
                    sdto.customMessageAr = "prepaid custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se.ErrorMessage;
                return sdto;
            }
            if (pt.serviceName.ToLower().EndsWith("-r"))
            {
                se = client.Topup(info);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se));
                sdto.status = se.Status.ToString();
                sdto.statusDescription = se.StatusDescription;
                if ("success".Equals(se.StatusDescription.ToLower()))
                {
                    sdto.currentBalance = se.Result.CurrentBalance.ToString();
                    sdto.rechargedAmount = se.Result.RechargedAmount.ToString();
                    sdto.operatorRef = se.Result.OperatorReference;
                    sdto.customMessage = "prepaid custom message english.";
                    sdto.customMessageAr = "prepaid custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se.ErrorMessage;
                return sdto;
            }
            if (pt.serviceName.ToLower().EndsWith("-cp"))
            {
                Service1Client client2 = new Service1Client();
                wcfProductsAPI.data.DonationInput input = new wcfProductsAPI.data.DonationInput
                {
                    ServiceName = pt.serviceName,
                    amount = pt.paymentamount,
                    msisdn = pt.msisdn,
                    country = pt.countryCode,
                    currency = pt.DonationReq.currency,
                    DonationCentre = pt.info2,
                    karats = pt.DonationReq.karats,
                    no_of_persons = pt.param1,
                    weightInGrams = pt.DonationReq.weightInGrams,
                    TransactionId = pt.transactionId,
                    paymentRef = pt.paymentReference
                };
                se4 = client2.PayZakat(input);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se4));
                sdto.status = se4.Status.ToString();
                sdto.statusDescription = se4.StatusDescription;
                if ("success".Equals(se4.StatusDescription.ToLower()))
                {
                    sdto.currentBalance = "";
                    sdto.rechargedAmount = se4.Result.amount.ToString();
                    sdto.operatorRef = "";
                    sdto.customMessage = "prepaid custom message english.";
                    sdto.customMessageAr = "prepaid custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se4.ErrorMessage;
                return sdto;
            }
            if (pt.serviceName.ToLower().StartsWith("zakat"))
            {
                Service1Client client3 = new Service1Client();
                wcfProductsAPI.data.DonationInput input2 = new wcfProductsAPI.data.DonationInput
                {
                    ServiceName = pt.serviceName,
                    amount = pt.paymentamount,
                    country = pt.countryCode,
                    currency = pt.DonationReq.currency,
                    DonationCentre = pt.DonationReq.DonationCentre,
                    karats = pt.DonationReq.karats,
                    no_of_persons = pt.DonationReq.no_of_persons,
                    weightInGrams = pt.DonationReq.weightInGrams,
                    TransactionId = pt.transactionId,
                    paymentRef = pt.paymentReference
                };
                se4 = client3.PayZakat(input2);
                TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se4));
                sdto.status = se4.Status.ToString();
                sdto.statusDescription = se4.StatusDescription;
                if ("success".Equals(se4.StatusDescription.ToLower()))
                {
                    sdto.currentBalance = "";
                    sdto.rechargedAmount = se4.Result.amount.ToString();
                    sdto.operatorRef = "";
                    sdto.customMessage = "prepaid custom message english.";
                    sdto.customMessageAr = "prepaid custom message arabic.";
                    return sdto;
                }
                sdto.customMessage = se4.ErrorMessage;
                return sdto;
            }
            se2 = client.PayBill(info);
            TraceLog.WriteToLog(string.Format("Class : GlobalPayitService Destination:Core  Method : ProcessCore ResponseFromCore", new object[0]) + new JavaScriptSerializer().Serialize(se2));
            sdto.status = se2.Status.ToString();
            sdto.statusDescription = se2.StatusDescription;
            if ("success".Equals(se2.StatusDescription.ToLower()))
            {
                sdto.operatorRef = se2.Result.OperatorReference;
                sdto.customMessage = "prepaid custom message english.";
                sdto.customMessageAr = "prepaid custom message arabic.";
                return sdto;
            }
            sdto.customMessage = se2.ErrorMessage;
            return sdto;
        }

        private long saveTransactionComplete(ProcessIN2 tran)
        {
            try
            {
                double num2 = Convert.ToDouble(tran.serviceamount);
                double num3 = Convert.ToDouble(tran.paymentamount);
                TransactionsComplete entity = new TransactionsComplete
                {
                    Amount = new double?(num2),
                    AppCode = GlobalData.username,
                    ProcessTranDate = new DateTime?(DateTime.Now),
                    CountryCode = tran.countryCode,
                    CreatedDate = new DateTime?(DateTime.Now),
                    CurrencyCode = tran.servicecurrencyCode,
                    Customer = tran.msisdn,
                    PaymentChannelCode = tran.paymentChannelCode,
                    PaymentReference = tran.paymentReference,
                    ReferenceID = tran.transactionId,
                    ServiceCode = tran.serviceName,
                    ServiceReference = tran.transactionId,
                    Status = true,
                    StatusDescription = "INPROCESS",
                    TransactionDate = new DateTime?(DateTime.Now),
                    IPAddress = ""
                };
                if ((tran.customerid == null) || string.IsNullOrEmpty(tran.customerid))
                {
                    entity.UserReference = "";
                }
                else
                {
                    entity.UserReference = tran.customerid;
                }
                entity.PaymentCurrencyCode = tran.paymentcurrencycode;
                entity.PaymentAmount = new double?(num3);
                if (((tran.info2 == null) || string.IsNullOrEmpty(tran.info2)) || tran.info2.Equals(""))
                {
                    entity.Info3 = "";
                }
                else
                {
                    entity.Info3 = tran.info2;
                }
              //  entity.AppVerConfigID = new int?(this.GetAppVersionConfigIDforSerPayID(Convert.ToInt32(tran.servicepaymentid)));
                entity.AppVersionID = new int?(Convert.ToInt32(GlobalData.appVersionID));
                entity.CountryID = new int?(Convert.ToInt32(tran.countryid));
                entity.CustomerID = new long?(Convert.ToInt64(tran.customerid));
              //  entity.PaymentConfigID = new int?(Convert.ToInt32(tran.paymentconfigid));
              //  entity.ServiceConfigID = new int?(Convert.ToInt32(tran.serviceconfigID));
              //  entity.ServicePaymentID = new int?(Convert.ToInt32(tran.servicepaymentid));
                this.db2.TransactionsCompletes.Add(entity);
                this.db2.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                TraceLog.WriteToLog("Save Transaction Complete Exception  " + ex.Message);
                return 0L;
            }
        }

        private int GetAppVersionConfigIDforSerPayID(int serpayid)
        {
            try
            {
                int appversionid = Convert.ToInt32(GlobalData.appVersionID);
                AppVersionConfiguration configuration = (from x in this.db2.AppVersionConfigurations
                                                         where (x.AppVersionID == appversionid) && (x.ServicesAndPaymentsID == serpayid)
                                                         select x).FirstOrDefault<AppVersionConfiguration>();
                if (configuration != null)
                {
                    return configuration.AppVersionConfigID;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool checkLogin(string username, string password)
        {
            Authentication At = new Authentication();
          return  At.checkLogin(username, password);

        }

        public Models.DTO<Models.Outputs.ServiceAmountsDTO> GetServiceAmounts(Models.Input<Models.Inputs.LoginIN> obj)
        {
              Models.DTO<Models.Outputs.ServiceAmountsDTO> op = new Models.DTO<Models.Outputs.ServiceAmountsDTO>();
            op.objname = "ServiceAmounts";
            Models.Outputs.ServiceAmountsDTO rep = new Models.Outputs.ServiceAmountsDTO();
            try
            {

                if (!isHashValid(obj.secure))
                {
                    throw new Exception("Tampered Data : Invalid Secure Hash");
                }


                if (!isValidObj(obj, obj.secure.data))
                {
                    throw new Exception("Tampered Data : Requests doesn't match");
                }

                GetAccountServices At = new GetAccountServices();
                var Result=At.GetServiceAmounts(obj.param.username, obj.param.password, obj.input.DeviceID);

                if (Result!=null)
                {
                    rep = Result;
                    op.status = new Models.Status(0);
                }
                else
                {
                    rep = Result;
                    op.status = new Models.Status(404);
                }
               
            }
            catch (Exception ex)
            {
                Common.TraceLog.WriteToLog(ex.Message.ToString());

                op.status = new Models.Status(1);
                op.status.info1 = ex.Message;
            }
            op.response = rep;
            return op;

        }
       
        #region Hash

         public bool isValidObj(object obj, string objReq)
        {
            if (obj.GetType().Equals(typeof(Models.Input<Models.Inputs.LoginIN>)))
            {
                Models.Input<Models.Inputs.LoginIN> oReq = (Models.Input<Models.Inputs.LoginIN>)obj;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                Models.Input<Models.Inputs.LoginIN> sReq = new Models.Input<Models.Inputs.LoginIN>();
                sReq = ser.Deserialize<Models.Input<Models.Inputs.LoginIN>>(objReq);

                string[] arr = { "secure",  "input", "param" };
                return GlobalData.PublicInstancePropertiesEqual(oReq, sReq, arr);
            }
            else if (obj.GetType().Equals(typeof(Models.Input<Models.Inputs.ServicesIN>)))
            {
                Models.Input<Models.Inputs.ServicesIN> oReq = (Models.Input<Models.Inputs.ServicesIN>)obj;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                Models.Input<Models.Inputs.ServicesIN> sReq = new Models.Input<Models.Inputs.ServicesIN>();
                sReq = ser.Deserialize<Models.Input<Models.Inputs.ServicesIN>>(objReq);

                string[] arr = { "secure", "input", "param" };
                return GlobalData.PublicInstancePropertiesEqual(oReq, sReq, arr);
            }
            else if (obj.GetType().Equals(typeof(Models.Input<Models.Inputs.ProcessIN>)))
            {
                Models.Input<Models.Inputs.ProcessIN> oReq = (Models.Input<Models.Inputs.ProcessIN>)obj;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                Models.Input<Models.Inputs.ProcessIN> sReq = new Models.Input<Models.Inputs.ProcessIN>();
                sReq = ser.Deserialize<Models.Input<Models.Inputs.ProcessIN>>(objReq);

                string[] arr = { "secure", "input", "param" };
                return GlobalData.PublicInstancePropertiesEqual(oReq, sReq, arr);
            }



            return false;
        }


        public string getSecretForApp()
        {
            GlobalPayitEntities dc = new GlobalPayitEntities();

            AppVersion app = (from tbl in dc.AppVersions where tbl.Username.Equals(GlobalData.username) && tbl.Password.Equals(GlobalData.password) select tbl).FirstOrDefault();

            if (app == null || app.SecretKey == null)
                return "";
            return app.SecureSecretKey;
        }


        public string getDecryptedSecretForApp()
        {
            return Common.Security.decrypt(getSecretForApp());
        }

        public string getHashedValueForString(string data)
        {

            string secret = getDecryptedSecretForApp();


            return Hash.GetHash(data, Hash.HashType.HMACSHA256, secret);

        }

        public bool isHashValid(PayitDealerGlobalPayit.Models.SecureHash sh)
        {
            if (sh.securehash.ToLower().Equals(getHashedValueForString(sh.data).ToLower()))
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
