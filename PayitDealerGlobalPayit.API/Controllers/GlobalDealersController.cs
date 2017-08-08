using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PayitDealerGlobalPayit.API.Utilities;
using PayitDealerGlobalPayit.Services.Interfaces;
using PayitDealerGlobalPayit.Services.Implementations;
using Newtonsoft.Json;


namespace PayitDealerGlobalPayit.API.Controllers
{
   
    [Auth]
    public class GlobalDealersController : ApiController
    {
        #region publicAPIMethods


        static readonly IDealers dealers = new Dealers();

        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.LoginDTO> login(Models.Input<Models.Inputs.LoginIN> obj)
        {
            return dealers.login(obj);
        }

        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.ServicesDTO> GetServicesByCountry(Models.Input<Models.Inputs.ServicesIN> obj)
        {
            return dealers.GetServicesByCountry(obj);
        }

        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.CategoryServices> GetServicesByCountry2(Models.Input<Models.Inputs.ServicesIN> obj)
        {
            return dealers.GetServicesByCountry2(obj);
        }
        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.ServiceDetailsDTO2> GetServicesDetails(Models.Inputs.ServiceDetailsIN obj)
        {
            return dealers.GetServicesDetails(obj);
        }
        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.CategoryServices> GetAllServices(Models.Input<Models.Inputs.GetActiveServices> obj)
        {
            return dealers.GetAllServices(obj);
        }
        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.ProcessDTO> Process(Models.Input<Models.Inputs.ProcessIN> obj)
        {
            return dealers.Process(obj);
        }

        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.ProcessDTO> Process2(Models.Input<Models.Inputs.ProcessIN2> obj)
        {
            return dealers.Process2(obj);
        }



        [Auth]
        [HttpPost]
        public Models.DTO<Models.Outputs.ServiceAmountsDTO> GetServiceAmounts(Models.Input<Models.Inputs.LoginIN> obj)
        {
            return dealers.GetServiceAmounts(obj);
        }

        #endregion publicAPIMethods


        #region SampleMethods

        [HttpGet]
        public Models.Input<Models.Inputs.LoginIN> login()
        {
            Models.Input<Models.Inputs.LoginIN> log = new Models.Input<Models.Inputs.LoginIN>();
            Models.CommonInputParams param = new Models.CommonInputParams();
            param.username = "test";
            param.password = "test";

            log.param = param;

            Models.Inputs.LoginIN login = new Models.Inputs.LoginIN();

            log.input = login;

            return log;

        }

        [HttpGet]
        public Models.Input<Models.Inputs.ServicesIN> GetServicesByCountry()
        {
            Models.Input<Models.Inputs.ServicesIN> log = new Models.Input<Models.Inputs.ServicesIN>();
            Models.CommonInputParams param = new Models.CommonInputParams();
            Models.Inputs.ServicesIN Sin = new Models.Inputs.ServicesIN();
            Models.Inputs.GetActiveServices GAS = new Models.Inputs.GetActiveServices();
            Models.Inputs.SecureHash sh = new Models.Inputs.SecureHash();
            param.username = "test";
            param.password = "test";

            sh.data = "";
            sh.securehash = "";

            GAS.countryCode = "CW";
            GAS.secure = sh;

            Sin.AccountID = 12;
            Sin.GetActiveServices = GAS;

            log.param = param;
            log.input = Sin;

            return log;

        }

        [HttpGet]
        public Models.Input<Models.Inputs.ProcessIN> Process()
        {
            Models.Input<Models.Inputs.ProcessIN> log = new Models.Input<Models.Inputs.ProcessIN>();
            Models.CommonInputParams param = new Models.CommonInputParams();
            Models.Inputs.ProcessIN Sin = new Models.Inputs.ProcessIN();
            Models.SecureHash sh = new Models.SecureHash();
           
            param.username = "test";
            param.password = "test";

            
            Sin.AccountId = 12;
            Sin.AccountUserId = 12;
            Sin.ServiceAmount=0.0;
            Sin.PaymentAmount = 0.0;
            Sin.countryCode="";
            Sin.DeviceID=12;
            Sin.msisdn="";
            Sin.PaymentChannel="";
            Sin.seriviceCode="";
            Sin.ServiceId=12;
            Sin.serviceName="";
            Sin.transactionId="";

            sh.data = "";
            sh.securehash = "";

            log.param = param;
            log.input = Sin;
            log.secure = sh;

            

            return log;

        }


        #endregion  SampleMethods

    }


    
}
