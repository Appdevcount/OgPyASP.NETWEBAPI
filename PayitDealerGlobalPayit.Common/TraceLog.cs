using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PayitDealerGlobalPayit.Common
{
    public class TraceLog
    {
        private static TraceSource _source;

        static TraceLog()
        {
            _source = new TraceSource("ServiceTestSource");  //refactor this for better usage
            _source.Switch.Level = SourceLevels.All;
            Trace.AutoFlush = true;
        }

        protected static TraceSource Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public static void SetSource(string name)
        {
            _source = new TraceSource(name);
        }

        public static void WriteToLog(string message)
        {
            //You can add more methods of this kind to log errors/warnings etc.

            //Source.TraceEvent(TraceEventType.Information, 0, message); //this would also work
            Source.TraceInformation(message);
        }

        public static string GetAllErrMessages(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            for (Exception eCurrent = ex; eCurrent != null; eCurrent = eCurrent.InnerException)
            {
                sb.AppendLine(eCurrent.Message + " at " + eCurrent.Source + ", trace: " + eCurrent.StackTrace);
            }
            return sb.ToString();
        }

        public static void WriteToLog(string message, Exception ex)
        {
            Source.TraceInformation(message + " - " + GetAllErrMessages(ex));
            Source.TraceEvent(TraceEventType.Error, 0, "{0}: {1}", message, ex);
        }

    }
}
