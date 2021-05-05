using System;
using WFA.ECS.Framework.Core.Logging;

namespace WFA.ECS.Framework.Core.ExceptionHandling
{
    public class WFFaultException: Exception
    {
        public new Exception InnerException { get; set; }
        public TransactionLogEntry LogEntry { get; set; }

        public WFFault_Type MyWFFault { get; set; }
        public object WFFault { get; set; }

        public WFFaultException(Exception ex)
        {
            this.InnerException = ex;
        }

        public WFFaultException(Exception ex, TransactionLogEntry logEntry)
        {
            this.InnerException = ex;
            this.LogEntry = LogEntry;
        }

        public WFFaultException(Exception ex, WFFault_Type t)
        {
            this.InnerException = ex;
            this.MyWFFault = t;
        }

        public WFFaultException(Exception ex, TransactionLogEntry logEntry, WFFault_Type t)
        {
            this.InnerException = ex;
            this.LogEntry = LogEntry;
            this.MyWFFault = t;
        }
    }
}
