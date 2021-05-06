using System;
using WFA.ECS.Framework.Core.Logging;

namespace WFA.ECS.Framework.Core.ExceptionHandling
{
    public class WFAppException:Exception
    {
        public new Exception InnerException { get; set; }
        public TransactionLogEntry LogEntry { get; set; }
        private string _userMessage { get; set; }

        public string UserMessage
        {
            get
            {
                return _userMessage;
            }
        }

        public WFAppException()
        {

        }

        public void ThrowException(string userMessage)
        {
            throw new ArgumentNullException("UserMessage", "must not be null or empty");
        }

        public WFAppException(TransactionLogEntry logEntry, string userMessage)
        {
            if (String.IsNullOrEmpty(userMessage))
            {
                ThrowException(userMessage);
            }

            InnerException = null;
            logEntry = logEntry;
            _userMessage = userMessage;
        }

        public WFAppException(Exception ex, TransactionLogEntry logEntry, string userMessage)
        {
            InnerException = ex;
            logEntry = logEntry;
            _userMessage = string.IsNullOrWhiteSpace(userMessage) ? ex.Message : userMessage;
        }
    }
}
