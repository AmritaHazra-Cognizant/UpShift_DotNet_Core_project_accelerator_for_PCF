using DotNetCore.API.Logging;
using DotNetCore.API.Web.Controllers;
using DotNetCore.API.Web.Security.Implementation;
using DotNetCore.Framework.ExceptionHandling.Models;
using DotNetCore.Framework.Logging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.ActionFilters
{
    public class TransactionExceptionLoggingFilter : IActionFilter, IOrderedFilter
    {
        
        private readonly Stopwatch _stopwatch;
        public TransactionExceptionLoggingFilter()
        {
            _stopwatch = new Stopwatch();
        }
        public int Order { get; } = int.MaxValue - 10;
        public string UserErrorMessage { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {

            _stopwatch.Stop();
            var controller = (BaseApiController)context.Controller;

            var logEntry = controller.LogEntry;
            logEntry.Duration = _stopwatch.ElapsedMilliseconds;
            logEntry.ErrorMessage = context.Exception == null ? "None" : 
                (context.Exception.InnerException!=null? 
                context.Exception.InnerException.Message: context.Exception.Message);
            logEntry.TransactionStatus = context.Exception == null ?
                TransactionStatus.Success : TransactionStatus.Fail;
            Log.Info(logEntry);
            // Check Exception and Log
            if (null != context.Exception)
            {
                UserErrorMessage = string.IsNullOrWhiteSpace(UserErrorMessage) ? "" : UserErrorMessage;
                var exToHandle = new AppException(logEntry, UserErrorMessage);
                Log.Error(exToHandle);
                context.HttpContext.Response.Headers["X-Handling-Instance-Id"] = logEntry.TransactionId.ToString();
                throw exToHandle;
            }
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Before Call
            var controller = (BaseApiController)context.Controller;
           

            var logEntry = controller.LogEntry;
            logEntry.SessionId = context.HttpContext.Connection.Id;
            logEntry.LoggingHostName = Environment.MachineName;
            string functionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            string methodName = string.Format("{0}.{1}", context.Controller.GetType().FullName, functionName);
            string userId = string.Empty;
            if (context.HttpContext.User != null && context.HttpContext.User.Claims != null)
            {
               var userClaim= context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthorizationConstants.USER_ID);
                if (userClaim != null)
                    userId = userClaim.Value;
            }
            InitializeLogEntry(logEntry, functionName, methodName, userId);
            _stopwatch.Reset();
            _stopwatch.Start();
            // To DO: We can handle other custome reuirements here also.
        }


        private void InitializeLogEntry(TransactionLogEntry logEntry,
            string functionName, string methodName, string loggedInUserId)
        {
            logEntry.TransactionId = Guid.NewGuid();
            logEntry.TransactionStatus = TransactionStatus.Success;
            logEntry.LoggingFunctionName = functionName;
            logEntry.LoggingMethodName = methodName;
            logEntry.LogEntryType = TransactionRecordType.Summary;
            logEntry.StartTime = DateTime.Now;
            logEntry.ExtendedProperties = new Dictionary<string, object>();
        }
    }
}
