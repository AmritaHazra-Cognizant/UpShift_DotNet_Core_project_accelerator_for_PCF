using DotNetCore.Framework.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Logging.Interface
{
   public interface IBaseHandler
    {
        TransactionLogEntry LogEntry { get; set; }
        object LockObject { get; set; }
    }
}
