using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Framework.Logging.Models;
using Microsoft.AspNetCore.Mvc;
namespace DotNetCore.API.Web.Controllers
{
    public abstract class BaseApiController:ControllerBase
    {
        private TransactionLogEntry _logEntry;
       
        public BaseApiController(TransactionLogEntry logEntry)
        {
            _logEntry = logEntry;
        }

        public TransactionLogEntry LogEntry { get { return _logEntry; } }
        public string JwtToken { get; set; }

        public string LoggedInUserId { get; }

    }
}
