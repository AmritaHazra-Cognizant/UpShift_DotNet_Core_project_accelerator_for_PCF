using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
//using WFA.ECS.Framework.Core.Logging;

namespace WFA.ECS.Framework.Core.Framework.ExceptionHandling.Formatter
{
    public class SplunkExceptionFormatter
    {
        private readonly Regex GuidExpression =
            new Regex("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.Compiled);
        public SplunkExceptionFormatter() { }

        public virtual string Format(Exception exception)
        {
            if (null == exception)
            {
                throw new ArgumentNullException("exception");
            }
            if (exception is WFAppException wfaAppEx)
                return Format(wfaAppEx);
            else if (exception is WFFaultException wffaultEx)
                return Format(wffaultEx);
            else
            {
                return FormatErrorMessage(exception);
            }

        }

        public string FormatErrorMessage(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}], ", DateTime.Now.ToString("dd/MMM/yyyy:HH:mm:ss:zzz"));
            if (!string.IsNullOrEmpty(exception.Message))
            {
                sb.AppendFormat("errorMessage=\"{0}\",", (exception.GetType().FullName
                                                                + "-" + exception.Message.Replace(Environment.NewLine, " ")));
            }
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                sb.AppendFormat("stackTrace=\"{0}\",",  exception.StackTrace.Replace(Environment.NewLine, " "));
            }
            sb.AppendFormat("loggingHostName={0}, ", Dns.GetHostName());
            var handlingInstanceId = GetHandlingInstanceId(exception);
            if (handlingInstanceId != Guid.Empty)
            {
                sb.AppendFormat("HandlingInstanceId={0}", handlingInstanceId.ToString("D",
                                        (IFormatProvider)System.Globalization.CultureInfo.InvariantCulture));
            }
            sb.AppendFormat("loggingCategory={0}, ", "Error");
            sb.AppendFormat("errorCode={0}, ", "0");
            if (exception.Data.Count > 0)
            {
                foreach (DictionaryEntry dictionaryEntry in exception.Data)
                {
                    sb.AppendFormat(string.Join(", ", new String[1] {
                    string.Format("{0}={1}",dictionaryEntry.Key, dictionaryEntry.Value)
                    }));

                }
            }
            return sb.ToString();
        }

        public virtual string Format(WFAppException appException)
        {
            if (null == appException)
            {
                throw new ArgumentNullException("exception");
            }
            TransactionLogEntry transactionLogEntry = appException.LogEntry ?? new TransactionLogEntry();
            if (transactionLogEntry.ExtendedProperties == null)
            {
                transactionLogEntry.ExtendedProperties = new System.Collections.Generic.Dictionary<string, object>();
            }
            if (!string.IsNullOrEmpty(appException.UserMessage))
            {
                transactionLogEntry.ExtendedProperties["userMessage"] =
                        String.Format("\"{0}\"", appException.UserMessage);
            }
            return GetWfAppFormatString(appException);
        }

        public virtual string Format(WFFaultException wfFaultException)
        {
            if (null == wfFaultException)
            {
                throw new ArgumentNullException("exception");
            }
            TransactionLogEntry transactionLogEntry = wfFaultException.LogEntry ?? new TransactionLogEntry();
            if (transactionLogEntry.ExtendedProperties == null)
            {
                transactionLogEntry.ExtendedProperties = new System.Collections.Generic.Dictionary<string, object>();
            }
            if (wfFaultException.MyWFFault != null)
            {
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.AdviceText))
                {
                    transactionLogEntry.ExtendedProperties.Add("adviceText", 
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.AdviceText));
                }
                if ( wfFaultException.MyWFFault.DataReference!=null &&
                            !string.IsNullOrEmpty(wfFaultException.MyWFFault.DataReference.ReferenceId))
                {
                    transactionLogEntry.ExtendedProperties.Add("dataReferenceResourceId",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.DataReference.ReferenceId));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.EmbeddedException))
                {
                    transactionLogEntry.ExtendedProperties.Add("embeddedException",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.EmbeddedException));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.ExceptionInstanceId))
                {
                    transactionLogEntry.ExtendedProperties.Add("exceptionInstanceId",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.ExceptionInstanceId));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.FaultActor))
                {
                    transactionLogEntry.ExtendedProperties.Add("faultActor",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.FaultActor));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.FaultCode))
                {
                    transactionLogEntry.ExtendedProperties.Add("faultCode",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.FaultCode));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.FaultReasonText))
                {
                    transactionLogEntry.ExtendedProperties.Add("faultReasonText",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.FaultReasonText));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.FaultSubCode))
                {
                    transactionLogEntry.ExtendedProperties.Add("faultSubCode",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.FaultSubCode));
                }
                transactionLogEntry.ExtendedProperties.Add("faultType",
                          wfFaultException.MyWFFault.FaultType==FaultType_Enum.APPL?"APPL":"SYSTEM");
                string severity = string.Empty;
                switch (wfFaultException.MyWFFault.Severity)
                {
                    case FaultSeverity_Enum.CRITICAL_ERROR:
                        {
                            severity = "CRITICAL";
                            break;
                        }
                    case FaultSeverity_Enum.ERROR:
                        {
                            severity = "ERROR";
                            break;
                        }
                    case FaultSeverity_Enum.INFORMATION:
                        {
                            severity = "INFORMATION";
                            break;
                        }
                    case FaultSeverity_Enum.WARNING:
                        {
                            severity = "WARNING";
                            break;
                        }
                }
                if (!string.IsNullOrEmpty(severity))
                {
                    transactionLogEntry.ExtendedProperties.Add("severity", severity);
                }

                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.StackTrace))
                {
                    transactionLogEntry.ExtendedProperties.Add("stackTrace",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.StackTrace  ));
                }
                if (!string.IsNullOrEmpty(wfFaultException.MyWFFault.TechnicalText))
                {
                    transactionLogEntry.ExtendedProperties.Add("technicalText",
                            string.Format("\"{0}\"", wfFaultException.MyWFFault.TechnicalText));
                }   
            }
            return GetWfFaultFormatString(wfFaultException);
        }

        public string GetWfFaultFormatString(WFFaultException wfFaultException)
        {
            TransactionLogEntry transactionLogEntry = wfFaultException.LogEntry;
            string rtrn = null,
                empty, 
                str, 
                empty1,
                str1;
            bool hasFault = false;
            StringBuilder sb = new StringBuilder();
            if (transactionLogEntry != null)
            {
                DateTime now = DateTime.Now;
                sb.AppendFormat("[{0}], ", now.ToString("dd/MMM/yyyy:HH:mm:ss:zzz"));
                //logging for each layer
                if (transactionLogEntry.SessionId != null)
                {
                    empty = transactionLogEntry.SessionId;
                }
                else {
                    empty = string.Empty;
                }
                string sessionIdField = empty;
                if (!string.IsNullOrEmpty(sessionIdField))
                {
                    sb.AppendFormat("sessionId={0}, ", sessionIdField);
                }
                else
                {
                    sb.AppendFormat("sessionId={0}, ", transactionLogEntry.SessionId);
                }

                if (!string.IsNullOrEmpty(transactionLogEntry.SessionSeqNo))
                {
                    sb.AppendFormat("sessionSeqNo={0}, ", transactionLogEntry.SessionSeqNo);
                }
                else
                {
                    sb.AppendFormat("sessionId={0}, ", "0");
                }

                if (transactionLogEntry.TransactionId != null)
                {
                    str = transactionLogEntry.TransactionId.Equals(Guid.Empty.ToString()) ?
                        string.Empty : transactionLogEntry.TransactionId;
                }
                else
                {
                    str = string.Empty;
                }
                string transactionIdField = str;
                if (!string.IsNullOrEmpty(transactionIdField))
                {
                    sb.AppendFormat("transactionId={0}, ", transactionIdField);
                }

                sb.AppendFormat("transactionType={0}, ", transactionLogEntry.TransactionType);
                sb.AppendFormat("status={0}, ", transactionLogEntry.Status);

                if (!string.IsNullOrEmpty(transactionLogEntry.StatusCode))
                {
                    sb.AppendFormat("statusCode={0}, ", transactionLogEntry.StatusCode);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.Message))
                {
                    sb.AppendFormat("statusMessage=\"{0}\",", transactionLogEntry.Message.Replace(Environment.NewLine, " "));
                }
                sb.AppendFormat("logEntryType={0}, ", transactionLogEntry.LogEntryType);

                if (transactionLogEntry.MessageId != null)
                {
                    empty1 = transactionLogEntry.MessageId.Equals(Guid.Empty.ToString()) ?
                        string.Empty : transactionLogEntry.MessageId;
                }
                else
                {
                    empty1 = string.Empty;
                }
                string messageIdField = empty1;
                if (!string.IsNullOrEmpty(messageIdField))
                {
                    sb.AppendFormat("messageId={0}, ", messageIdField);
                }
                if (transactionLogEntry.RefToMessageId != null)
                {
                    str1 = transactionLogEntry.RefToMessageId.Equals(Guid.Empty.ToString()) ?
                        string.Empty : transactionLogEntry.RefToMessageId;
                }
                else
                {
                    str1 = messageIdField;
                }

                string RefToMessageIdField = str1;
                if (!string.IsNullOrEmpty(RefToMessageIdField))
                {
                    sb.AppendFormat("refToMessageId={0}, ", RefToMessageIdField);
                }
                DateTime startTime = transactionLogEntry.StartTime;
                sb.AppendFormat("startTime={0}", startTime.ToString("HH:mm:ss:fff"));
                sb.AppendFormat("duration={0}", transactionLogEntry.Duration);

                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingAppId))
                {
                    sb.AppendFormat("loggingAppId={0}, ", transactionLogEntry.LoggingAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingSubAppId))
                {
                    sb.AppendFormat("loggingSubAppId={0}, ", transactionLogEntry.LoggingSubAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingServiceName))
                {
                    sb.AppendFormat("loggingServiceName={0}, ", transactionLogEntry.LoggingServiceName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingOperationName))
                {
                    sb.AppendFormat("loggingOperationName={0}, ", transactionLogEntry.LoggingOperationName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingMethodName))
                {
                    sb.AppendFormat("loggingMethodName={0}, ", transactionLogEntry.LoggingMethodName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingHostName))
                {
                    sb.AppendFormat("loggingHostName={0}, ", transactionLogEntry.LoggingHostName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingApplicationName))
                {
                    sb.AppendFormat("loggingApplicationName={0}, ", transactionLogEntry.LoggingApplicationName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingFunctionName))
                {
                    sb.AppendFormat("loggingFunctionName={0}, ", transactionLogEntry.LoggingFunctionName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.SenderAppId))
                {
                    sb.AppendFormat("callingAppId={0}, ", transactionLogEntry.SenderAppId);
                }
                else
                {
                    sb.AppendFormat("callingAppId={0}, ", transactionLogEntry.LoggingAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.SenderSubAppId))
                {
                    sb.AppendFormat("callingSubAppId={0}, ", transactionLogEntry.SenderSubAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.CallingHostName))
                {
                    sb.AppendFormat("callingHostName={0}, ", transactionLogEntry.CallingHostName);
                }
                sb.AppendFormat("errorCode={0}, ", transactionLogEntry.ErrorCode);
                if (wfFaultException.MyWFFault != null)
                {
                    string str5 = string.IsNullOrEmpty(wfFaultException.MyWFFault.TecnicalText)
                        ? null != wfFaultException.InnerException &&
                        !string.IsNullOrEmpty(wfFaultException.InnerException.Message)
                        ? wfFaultException.InnerException.Message.Replace(Environment.NewLine, " ")
                        : wfFaultException.Message.Replace(Environment.NewLine, " ")
                        : wfFaultException.MyWFFault.TechnicalText.Replace(Environment.NewLine, " ");
                    if (!string.IsNullOrEmpty(str5))
                    {
                        sb.AppendFormat("errorMessage=\"{0}\", ", str5);
                    }
                    hasFault = true;
                }

                if (wfFaultException.InnerException != null)
                {
                    if (!hasFault)
                    {
                        string str5 = string.IsNullOrEmpty(wfFaultException.InnerException.Message)
                     ? wfFaultException.Message.Replace(Environment.NewLine, " ")
                     : wfFaultException.InnerException.Message.Replace(Environment.NewLine, " ");

                        if (!string.IsNullOrEmpty(str5))
                        {
                            sb.AppendFormat("errorMessage=\"{0}\", ", str5);
                        }
                    }
                    string str6 = string.IsNullOrEmpty(wfFaultException.InnerException.StackTrace)
                       ? string.IsNullOrEmpty(wfFaultException.StackTrace) ? string.Empty
                       : wfFaultException.StackTrace.Replace(Environment.NewLine, " ")
                       : wfFaultException.InnerException.StackTrace.Replace(Environment.NewLine, " ");

                    if (!string.IsNullOrEmpty(str6))
                    {
                        sb.AppendFormat("errorMessage=\"{0}\", ", (object)str6);
                    }

                }
                else
                {
                    if (!hasFault && !string.IsNullOrEmpty(wfFaultException.Message))
                    {
                        sb.AppendFormat("errorMessage=\"{0}\", ",
                           wfFaultException.Message.Replace(Environment.NewLine, " "));
                    }
                    if (!string.IsNullOrEmpty(wfFaultException.StackTrace))
                    {
                        sb.AppendFormat("stackTrace=\"{0}\", ",
                          wfFaultException.StackTrace.Replace(Environment.NewLine, " "));
                    }
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggedInPPID))
                {
                    sb.AppendFormat("loggedInPPID={0}, ", transactionLogEntry.LoggedInPPID);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.ActivityIdString))
                {
                    sb.AppendFormat("activityIdString={0}, ", transactionLogEntry.ActivityIdString);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.Version))
                {
                    sb.AppendFormat("version={0}, ", transactionLogEntry.Version);
                }
                var handlingInstanceId = GeHandlingInstanceId(wfFaultException.InnerException);
                if (handlingInstanceId != Guid.Empty)
                {
                    sb.AppendFormat("handlingInstanceID={0}", handlingInstanceId.ToString(
                        "D",
                        (IFormatProvider)System.Globalization.CultureInfo.InvariantCulture));
                }
                if (transactionLogEntry.ExtendedProperties != null && transactionLogEntry.ExtendedProperties.count > 0)
                {
                    foreach (DictionaryEntry dictionaryEntry in transactionLogEntry.ExtendedProperties)
                    {
                        sb.AppendFormat(string.Format("{0}={1}",dictionaryEntry.Key, dictionaryEntry.Value));

                    }
                }
                rtrn = sb.ToString().TrimEnd(new char[]
                    { ',',' '});

            }
            return rtrn;
        }

        public string GetWfAppFormatString(WFAppException wfAppException)
        {
            TransactionLogEntry transactionLogEntry = wfFaultException.LogEntry;
            string rtrn = null,
                empty,
                str,
                empty1,
                str1;
            bool hasFault = false;
            StringBuilder sb = new StringBuilder();
            if (transactionLogEntrym is TransactionLogEntry)
            {
                DateTime now = DateTime.Now;
                sb.AppendFormat("[{0}], ", now.ToString("dd/MMM/yyyy:HH:mm:ss:zzz"));
                //logging for each layer
                if (transactionLogEntry.SessionId != null)
                {
                    empty = transactionLogEntry.SessionId;
                }
                else
                {
                    empty = string.Empty;
                }
                string sessionIdField = empty;
                if (!string.IsNullOrEmpty(sessionIdField))
                {
                    sb.AppendFormat("sessionId={0}, ", sessionIdField);
                }
                else
                {
                    sb.AppendFormat("sessionId={0}, ", transactionLogEntry.TransactionId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.SessionSeqNo))
                {
                    sb.AppendFormat("sessionSeqNo={0}, ", transactionLogEntry.SessionSeqNo);
                }
                else
                {
                    sb.AppendFormat("sessionId={0}, ", "0");
                }
                if (transactionLogEntry.TransactionId != null)
                {
                    str = transactionLogEntry.TransactionId.Equals(Guid.Empty.ToString()) ?
                        string.Empty : transactionLogEntry.TransactionId;
                }
                else
                {
                    str = string.Empty;
                }
                string transactionIdField = str;
                if (!string.IsNullOrEmpty(transactionIdField))
                {
                    sb.AppendFormat("transactionId={0}, ", transactionIdField);
                }
                sb.AppendFormat("transactionType={0}, ", transactionLogEntry.TransactionType);
                sb.AppendFormat("status={0}, ", transactionLogEntry.Status);

                if (!string.IsNullOrEmpty(transactionLogEntry.StatusCode))
                {
                    sb.AppendFormat("statusCode={0}, ", transactionLogEntry.StatusCode);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.Message))
                {
                    sb.AppendFormat("statusMessage=\"{0}\",", transactionLogEntry.Message.Replace(Environment.NewLine, " "));
                }
                sb.AppendFormat("logEntryType={0}, ", transactionLogEntry.LogEntryType);

                if (transactionLogEntry.MessageId != null)
                {
                    empty1 = transactionLogEntry.MessageId.Equals(Guid.Empty.ToString()) ?
                        string.Empty : transactionLogEntry.MessageId;
                }
                else
                {
                    empty1 = string.Empty;
                }
                string messageIdField = empty1;
                if (!string.IsNullOrEmpty(messageIdField))
                {
                    sb.AppendFormat("messageId={0}, ", messageIdField);
                }
                if (transactionLogEntry.RefToMessageId != null)
                {
                    str1 = transactionLogEntry.RefToMessageId.Equals(Guid.Empty.ToString()) ?
                        string.Empty : transactionLogEntry.RefToMessageId;
                }
                else
                {
                    str1 = messageIdField;
                }

                string RefToMessageIdField = str1;
                if (!string.IsNullOrEmpty(RefToMessageIdField))
                {
                    sb.AppendFormat("refToMessageId={0}, ", RefToMessageIdField);
                }
                DateTime startTime = transactionLogEntry.StartTime;
                sb.AppendFormat("startTime={0}", startTime.ToString("HH:mm:ss:fff"));
                sb.AppendFormat("duration={0}", transactionLogEntry.Duration);

                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingAppId))
                {
                    sb.AppendFormat("loggingAppId={0}, ", transactionLogEntry.LoggingAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingSubAppId))
                {
                    sb.AppendFormat("loggingSubAppId={0}, ", transactionLogEntry.LoggingSubAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingServiceName))
                {
                    sb.AppendFormat("loggingServiceName={0}, ", transactionLogEntry.LoggingServiceName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingOperationName))
                {
                    sb.AppendFormat("loggingOperationName={0}, ", transactionLogEntry.LoggingOperationName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingMethodName))
                {
                    sb.AppendFormat("loggingMethodName={0}, ", transactionLogEntry.LoggingMethodName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingHostName))
                {
                    sb.AppendFormat("loggingHostName={0}, ", transactionLogEntry.LoggingHostName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingApplicationName))
                {
                    sb.AppendFormat("loggingApplicationName={0}, ", transactionLogEntry.LoggingApplicationName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggingFunctionName))
                {
                    sb.AppendFormat("loggingFunctionName={0}, ", transactionLogEntry.LoggingFunctionName);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.SenderAppId))
                {
                    sb.AppendFormat("callingAppId={0}, ", transactionLogEntry.SenderAppId);
                }
                else
                {
                    sb.AppendFormat("callingAppId={0}, ", transactionLogEntry.LoggingAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.SenderSubAppId))
                {
                    sb.AppendFormat("callingSubAppId={0}, ", transactionLogEntry.SenderSubAppId);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.CallingHostName))
                {
                    sb.AppendFormat("callingHostName={0}, ", transactionLogEntry.CallingHostName);
                }
                sb.AppendFormat("errorCode={0}, ", transactionLogEntry.ErrorCode);
                if (wfAppException.InnerException != null)
                {
                    string str5 = string.IsNullOrEmpty(wfAppException.InnerException.Message)
                       ? wfAppException.Message.Replace(Environment.NewLine, " ")
                       : wfAppException.InnerException.Message.Replace(Environment.NewLine, " ");
                    if (!string.IsNullOrEmpty(str5))
                    {
                        sb.AppendFormat("errorMessage=\"{0}\", ", str5);
                    }
                    string str6 = string.IsNullOrEmpty(wfAppException.InnerException.StackTrace)
                      ? wfAppException.StackTrace.Replace(Environment.NewLine, " ")
                      : wfAppException.InnerException.StackTrace.Replace(Environment.NewLine, " ");

                    if (!string.IsNullOrEmpty(str6))
                    {
                        sb.AppendFormat("errorMessage=\"{0}\", ", (object)str6);
                    }

                }
                else 
                {
                    if (!string.IsNullOrEmpty(wfAppException.Message))
                    {
                        sb.AppendFormat("errorMessage=\"{0}\", ",
                           wfAppException.Message.Replace(Environment.NewLine, " "));
                    }
                    if (!string.IsNullOrEmpty(wfAppException.StackTrace))
                    {
                        sb.AppendFormat("stackTrace=\"{0}\", ",
                          wfAppException.StackTrace.Replace(Environment.NewLine, " "));
                    }
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.LoggedInPPID))
                {
                    sb.AppendFormat("loggedInPPID={0}, ", transactionLogEntry.LoggedInPPID);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.ActivityIdString))
                {
                    sb.AppendFormat("activityIdString={0}, ", transactionLogEntry.ActivityIdString);
                }
                if (!string.IsNullOrEmpty(transactionLogEntry.Version))
                {
                    sb.AppendFormat("version={0}, ", transactionLogEntry.Version);
                }
                var handlingInstanceId = GeHandlingInstanceId(wfFaultException.InnerException);
                if (handlingInstanceId != Guid.Empty)
                {
                    sb.AppendFormat("handlingInstanceID={0}", handlingInstanceId.ToString(
                        "D",
                        (IFormatProvider)System.Globalization.CultureInfo.InvariantCulture));
                }
                if (transactionLogEntry.ExtendedProperties != null && transactionLogEntry.ExtendedProperties.count > 0)
                {
                    foreach (DictionaryEntry dictionaryEntry in transactionLogEntry.ExtendedProperties)
                    {
                        sb.AppendFormat(string.Format("{0}={1}", dictionaryEntry.Key, dictionaryEntry.Value));

                    }
                }
                if (transactionLogEntry.ExtendedProperties != null && transactionLogEntry.ExtendedProperties.count > 0)
                {
                    foreach (DictionaryEntry dictionaryEntry in transactionLogEntry.ExtendedProperties)
                    {
                        sb.AppendFormat(string.Format("{0}={1}", dictionaryEntry.Key, dictionaryEntry.Value));

                    }
                }
                rtrn = sb.ToString().TrimEnd(new char[]
                    { ',',' '});

            }
            return rtrn;
        }

        private Guid GetHandlingInstanceId(Exception exception)
        {
            Guid guid = new Guid();
            if (exception != null)
            {
                if (exception.Data[(object)"handlingInstanceId"] != null)
                {
                    Match match = this.GuidExpression.Match(exception.Data[(object)"handlingInstanceId"].ToString());
                    if (match.Success)
                    {
                        guid = new Guid(match.Value);
                    }
                }
                else if (exception is WFFaultException)
                {
                    guid =Guid.Parse((WFFaultException)exception).LogEntry.TransactionId);
                }
            }
            return guid;

        }
    }
}
