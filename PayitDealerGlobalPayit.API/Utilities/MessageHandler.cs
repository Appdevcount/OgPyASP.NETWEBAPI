using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using PayitDealerGlobalPayit.Common;



namespace PayitDealerGlobalPayit.API.Utilities
{
    public abstract class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {


            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);

            var result = await request.Content.ReadAsByteArrayAsync();

            await IncommingMessageAsync(corrId, result);

            var response = await base.SendAsync(request, cancellationToken);

            byte[] responseMessage;

            if (response.IsSuccessStatusCode)
                responseMessage = await response.Content.ReadAsByteArrayAsync();
            else
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            await OutgoingMessageAsync(corrId, responseMessage);



            //request properties
            var requestMethod = request.Method.Method;
            var requestUri = request.RequestUri.ToString();
            var requestTimestamp = DateTime.UtcNow;
            var requestUserId = Thread.CurrentPrincipal.Identity.Name;
            // var requestIpAddress = RetrieveClientIPAddress(request);
            var userAgent = request.Headers.UserAgent.ToString();
            var userIPAddress = GetClientIp(request);
            var acceptType = request.Headers.Accept.ToString();


            //response properties
            var responseStatusCode = (int)response.StatusCode;


            //server &, application properties
            var machineName = Environment.MachineName;
            var releaseVersion = Assembly.GetExecutingAssembly().GetName().Version;

            GlobalData.ipaddress = userIPAddress;
            // var contentType = response.RequestMessage.Content.Headers.ContentType.ToString();

            //GlobalData.saveLog(GlobalData.tsInfo, string.Format("RequestMethod : {0} , RequestURI : {1}, MachineName : {2}, responseStatusCode : {3}, IPAddress : {4}, UserAgent : {5}, Accept : {6}, ContentType : {7}", requestMethod, requestUri, machineName, responseStatusCode,userIPAddress,userAgent,acceptType,""));
            TraceLog.WriteToLog(string.Format("RequestMethod : {0} , RequestURI : {1}, MachineName : {2}, responseStatusCode : {3}, IPAddress : {4}, UserAgent : {5}, Accept : {6}, ContentType : {7}, Username/Password : {8}/{9}", requestMethod, requestUri, machineName, responseStatusCode, userIPAddress, userAgent, acceptType, "",GlobalData.username,GlobalData.password));

            return response;
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            string ip = "";
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                if (ctx != null)
                {
                    ip = ctx.Request.UserHostAddress.ToString();
                    //do stuff with IP
                }
            }
            return ip;
        }
        protected abstract Task IncommingMessageAsync(string correlationId, byte[] message);
        protected abstract Task OutgoingMessageAsync(string correlationId, byte[] message);
    }



    public class MessageLoggingHandler : MessageHandler
    {
        protected override async Task IncommingMessageAsync(string correlationId, byte[] message)
        {
            await Task.Run(() =>
                //Debug.WriteLine(string.Format("{0} - Incomming messages: {1}", correlationId, Encoding.UTF8.GetString(message))));
                //GlobalData.saveLog(GlobalData.tsInfo, string.Format("{0} - Incomming messages: {1}", correlationId, Encoding.UTF8.GetString(message))));
            TraceLog.WriteToLog(string.Format("{0} - Incomming messages: {1}", correlationId, Encoding.UTF8.GetString(message))));
        }


        protected override async Task OutgoingMessageAsync(string correlationId, byte[] message)
        {
            await Task.Run(() =>
                //GlobalData.saveLog(GlobalData.tsInfo, string.Format("{0} - Outgoing messages: {1}", correlationId, Encoding.UTF8.GetString(message))));
                // Debug.WriteLine(string.Format("{0} - Outgoing messages: {1}", correlationId, Encoding.UTF8.GetString(message))));

            TraceLog.WriteToLog(string.Format("{0} - Outgoing messages: {1}", correlationId, Encoding.UTF8.GetString(message))));
        }
    }
}