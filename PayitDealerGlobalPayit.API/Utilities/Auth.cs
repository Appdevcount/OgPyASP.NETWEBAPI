using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayitDealerGlobalPayit.API.Security;

using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PayitDealerGlobalPayit.Common;
using PayitDealerGlobalPayit.Services.Interfaces;
using PayitDealerGlobalPayit.Services.Implementations;

namespace PayitDealerGlobalPayit.API.Utilities
{
    public class Auth : BasicAuthenticationFilter
    {
        static readonly IDealers dealers = new Dealers();

        public Auth()
        { }

        public Auth(bool active)
            : base(active)
        { }

        public bool Authenticate(string username, string password)
        {
            if (username.Equals("GlobalPayit") && password.Equals("GlobalPayit"))
                return true;
            return false;
        }

        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {

            GlobalData.username = username;
            GlobalData.password = password;
           

            return dealers.checkLogin(username, password);
        }


    }
}