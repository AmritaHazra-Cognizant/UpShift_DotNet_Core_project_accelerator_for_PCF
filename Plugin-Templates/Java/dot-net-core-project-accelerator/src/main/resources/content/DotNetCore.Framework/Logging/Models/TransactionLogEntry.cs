using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Logging.Models
{
    public enum TransactionStatus
    { 
        Success,
        Fail
    }
    public enum TransactionRecordType
    {
        Summary,
        Detail
    }
    public class TransactionLogEntry
    {
        public Guid TransactionId { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public string LogInUserId { get; set; }
        public string LoggingFunctionName { get; set; }
        public string LoggingHostName { get; set; }
        public string LoggingMethodName { get; set; }
        public string StackStrace { get; set; }
        public string SessionId { get; set; }
        public string ErrorMessage { get; set; }
        public IDictionary<string,object> ExtendedProperties { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionRecordType LogEntryType { get; set; }

    }
}
