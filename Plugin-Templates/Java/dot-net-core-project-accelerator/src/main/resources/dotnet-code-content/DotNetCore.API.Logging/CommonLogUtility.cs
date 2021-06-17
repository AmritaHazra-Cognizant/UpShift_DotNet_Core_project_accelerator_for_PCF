using DotNetCore.Framework.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.API.Logging
{
    public static class CommonLogUtility
    {
        public static TransactionLogEntry populateTransaction(TransactionLogEntry inputLogEntry)
        {
            TransactionLogEntry newTranEntry = new TransactionLogEntry();
            newTranEntry.TransactionId = inputLogEntry.TransactionId;
            newTranEntry.LoggingFunctionName = inputLogEntry.LoggingFunctionName;
            newTranEntry.LoggingHostName = inputLogEntry.LoggingHostName;
            newTranEntry.LogEntryType = TransactionRecordType.Detail;
            newTranEntry.LogInUserId = inputLogEntry.LogInUserId;
            newTranEntry.SessionId = inputLogEntry.SessionId;
            newTranEntry.ExtendedProperties = new Dictionary<string, object>();
            return newTranEntry;
        }
    }
}
