using DotNetCore.Framework.ExceptionHandling.Formatter;
using DotNetCore.Framework.Logging.Formatter;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Web;
using DotNetCore.Framework.ExceptionHandling.Models;
using DotNetCore.Framework.Logging.Models;

namespace DotNetCore.API.Logging
{
   public static class Log
    {
        private static TransactionLogFormatter _transactionlogFormatter;
        private static ExceptionFormatter _exceptionFormatter;
        static Log()
        {
            _transactionlogFormatter = new TransactionLogFormatter();
            _exceptionFormatter = new ExceptionFormatter();
            Logger = NLogBuilder.ConfigureNLog("NLog.config").GetLogger("DotNetCore.API.Logging");
        }
        public static Logger Logger { get; private set; }
        public static void Error(AppException ex)
        {
            Logger.Error(_exceptionFormatter.Format(ex));
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }

        public static void Info(TransactionLogEntry logEntry)
        {
            Logger.Info(_transactionlogFormatter.Format(logEntry));
        }
    }
}
