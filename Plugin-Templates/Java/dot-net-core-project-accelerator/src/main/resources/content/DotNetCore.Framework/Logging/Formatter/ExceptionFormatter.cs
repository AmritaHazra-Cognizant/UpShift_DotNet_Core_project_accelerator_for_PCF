using DotNetCore.Framework.ExceptionHandling.Models;
using DotNetCore.Framework.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.ExceptionHandling.Formatter
{
    public class ExceptionFormatter
    {
        public ExceptionFormatter()
        { }

        public virtual string Format(AppException appEx)
        {
            if (null == appEx)
                throw new ArgumentNullException("appEx");
            StringBuilder sb = new StringBuilder();
            var logEntry = appEx.LogEntry ?? new TransactionLogEntry();
            logEntry.ExtendedProperties = logEntry.ExtendedProperties ?? new Dictionary<string, object>();
            DateTime now = DateTime.Now;
            sb.AppendFormat("[{0}] ", now.ToString("dd/MM/yyyy:HH:mm:ss zzz"));

            sb.AppendFormat("sessionId={0};",
                !string.IsNullOrWhiteSpace(logEntry.SessionId) ?
                logEntry.SessionId : Guid.Empty.ToString());
            sb.AppendFormat("transactionId={0};",
               !string.IsNullOrWhiteSpace(Convert.ToString(logEntry.TransactionId)) ?
               logEntry.SessionId : Guid.Empty.ToString());

            sb.AppendFormat("logEntryType={0};", logEntry.LogEntryType.ToString());
            sb.AppendFormat("transactionStatus={0};", logEntry.TransactionStatus.ToString());
            sb.AppendFormat("loggingFunctionName={0};", logEntry.LoggingFunctionName);
            sb.AppendFormat("loggingMethodName={0};", logEntry.LoggingMethodName);
            sb.AppendFormat("logInUserId={0};", logEntry.LogInUserId);
            sb.AppendFormat("StartTime={0};", logEntry.StartTime.ToString("HH:mm:ss:fff"));
            sb.AppendFormat("duration={0}", logEntry.Duration);
            sb.AppendFormat("loggingHostName={0};", logEntry.LoggingHostName);

            if (!string.IsNullOrWhiteSpace(logEntry.ErrorMessage))
            {
                sb.AppendFormat("errorMessage=\"{0}\", ;", logEntry.ErrorMessage.Replace(Environment.NewLine, " "));
            }

            if (appEx.InnerException != null)
            {
                string errMsg = string.IsNullOrWhiteSpace(appEx.InnerException.Message) ?
                        appEx.Message.Replace(Environment.NewLine, " ") :
                        appEx.InnerException.Message.Replace(Environment.NewLine, " ");
                if (!string.IsNullOrWhiteSpace(errMsg))
                {
                    sb.AppendFormat("errorMessage={0};", errMsg);
                }

                string stck = string.IsNullOrWhiteSpace(appEx.InnerException.StackTrace) ?
                       appEx.StackTrace.Replace(Environment.NewLine, " ") :
                       appEx.InnerException.StackTrace
                       .Replace(Environment.NewLine, " ");
                if (!string.IsNullOrWhiteSpace(stck))
                {
                    sb.AppendFormat("stackTrace={0};", stck);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(appEx.Message))
                {
                    sb.AppendFormat("errorMessage={0};", appEx.Message);
                }
                if (!string.IsNullOrWhiteSpace(appEx.StackTrace))
                {
                    sb.AppendFormat("stackTrace={0};", appEx.StackTrace);
                }
            }
            sb.AppendFormat("HandlingInstanceId={0};", GetHandlingInstanceId(appEx.InnerException));
            if (logEntry.ExtendedProperties != null && logEntry.ExtendedProperties.Count > 0)
            {
                foreach (KeyValuePair<string, object> kvp in logEntry.ExtendedProperties)
                {
                    sb.Append(string.Format(", {0}=\"{1}\";", kvp.Key, kvp.Value));
                }
            }


            return sb.ToString().TrimEnd(new char[] { ',',' '}); 
        }

        private string GetHandlingInstanceId(Exception ex)
        {
            var guid = Guid.NewGuid().ToString();
            if (ex == null)
                return guid;

            if(ex.Data[(object)"handlingInstanceId"]!=null)
            {
                guid = ex.Data[(object)"handlingInstanceId"].ToString();

            }
            return guid;

        }
    }
}
