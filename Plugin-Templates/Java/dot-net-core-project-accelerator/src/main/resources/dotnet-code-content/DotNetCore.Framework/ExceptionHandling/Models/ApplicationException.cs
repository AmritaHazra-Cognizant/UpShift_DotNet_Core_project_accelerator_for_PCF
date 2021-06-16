using DotNetCore.Framework.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.ExceptionHandling.Models
{
    [Serializable]
    public class AppException : Exception
    {
        private string _userMessage;
        public TransactionLogEntry LogEntry { get; set; }
        public string UserMessage { get { return _userMessage; } }

        public AppException(TransactionLogEntry logEntry, string userMessage) : base(userMessage)
        {
            LogEntry = logEntry;
            _userMessage = userMessage;

        }
        public AppException(Exception ex, TransactionLogEntry logEntry, string userMessage) : base(userMessage, ex)
        {
            LogEntry = logEntry;
          
        }

    }
}
