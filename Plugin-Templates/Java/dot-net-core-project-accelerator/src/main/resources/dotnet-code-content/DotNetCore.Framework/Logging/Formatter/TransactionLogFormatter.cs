using DotNetCore.Framework.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Logging.Formatter
{
    public class TransactionLogFormatter
    {
        public TransactionLogFormatter()
        { }

        public virtual string Format(TransactionLogEntry logEntry)
        {
            StringBuilder sb = new StringBuilder();
         
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
            sb.AppendFormat("duration={0};", logEntry.Duration);
            sb.AppendFormat("loggingHostName={0};", logEntry.LoggingHostName);

            if (!string.IsNullOrWhiteSpace(logEntry.ErrorMessage))
            {
                sb.AppendFormat("errorMessage=\"{0}\", ;", logEntry.ErrorMessage.Replace(Environment.NewLine, " "));
            }
            if (logEntry.ExtendedProperties != null && logEntry.ExtendedProperties.Count > 0)
            {
                foreach (KeyValuePair<string, object> kvp in logEntry.ExtendedProperties)
                {
                    sb.Append(string.Format(", {0}=\"{1}\";", kvp.Key, kvp.Value));
                }
            }


            return sb.ToString().TrimEnd(new char[] { ',', ' ' });
        }
    }
}
