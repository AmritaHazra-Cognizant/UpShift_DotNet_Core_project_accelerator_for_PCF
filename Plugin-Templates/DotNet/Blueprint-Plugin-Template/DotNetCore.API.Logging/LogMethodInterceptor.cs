using DotNetCore.Framework.ExceptionHandling.Models;
using DotNetCore.Framework.Interception;
using DotNetCore.Framework.Interception.Interfaces;
using DotNetCore.Framework.Logging.Interface;
using DotNetCore.Framework.Logging.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DotNetCore.API.Logging
{
    public class LogMethodInterceptor : IMethodInterceptor
    {
        #region Private Variables

        private readonly object _lockObject;
        private TransactionLogEntry _logEntry;
        private readonly Stopwatch _stopWatch;
        private LogMethodAttribute loggingAttribute;

        #endregion
        public LogMethodInterceptor()
        {
            _lockObject = new object();
            _stopWatch = new Stopwatch();
        }

        public void BeforeInvoke(InvocationContext invocationContext)
        {
            // Initialize fresh Log object for each Layer copying from Global log object

            _logEntry = invocationContext.ServiceProvider.GetRequiredService<TransactionLogEntry>();
            IBaseHandler targetObject = invocationContext.Invocation.InvocationTarget as IBaseHandler;
            if (targetObject != null)
            {
                var targetLogEntry = CommonLogUtility.populateTransaction(_logEntry);
                targetObject.LockObject = new object();
                targetObject.LogEntry = targetLogEntry;

            }
            _stopWatch.Reset();
            _stopWatch.Start();

            // Get the logging attribute
            this.loggingAttribute = invocationContext.GetAttributeFromMethod<LogMethodAttribute>();
            // To DO: Customized with your code before finction call here..
        }

        public void AfterInvoke(InvocationContext invocationContext, object methodResult, Exception ex)
        {
            try {
                HandlePostMethodExecution(invocationContext, methodResult, 
                    (ex != null && ex.InnerException != null ? ex.InnerException : ex));
            }
            catch (Exception innerEx)
            {
                // Do nothing.. Suppress the log here
                bool tempFlag = this.loggingAttribute.IsFlowBreakReuired;
                this.loggingAttribute.IsFlowBreakReuired = false;
                HandlePostMethodExecution(invocationContext, methodResult,
                     (innerEx != null && innerEx.InnerException != null ?
                     innerEx.InnerException : innerEx));
                this.loggingAttribute.IsFlowBreakReuired = tempFlag;
            }
        }

        private void HandlePostMethodExecution(InvocationContext invocationContext, object result, Exception ex)
        {
            _stopWatch.Stop();
            IBaseHandler targetObject = invocationContext.Invocation.InvocationTarget as IBaseHandler;
            TransactionLogEntry methodlogEntry = null;
            if (targetObject != null)
            {
                methodlogEntry = targetObject.LogEntry;
            }

        }

        private void HandleTransactionFlow(TransactionLogEntry methodLogEntry,
            InvocationContext invocationContext, IBaseHandler targetObject, object result, Exception ex)
        {
            methodLogEntry.Duration = _stopWatch.ElapsedMilliseconds;
            methodLogEntry.LoggingMethodName = invocationContext.GetExecutingMethodFullName();
            methodLogEntry.ErrorMessage = ex == null ?
                (string.IsNullOrWhiteSpace(methodLogEntry.ErrorMessage) ?
                "None" : methodLogEntry.ErrorMessage) : 
                !string.IsNullOrWhiteSpace(this.loggingAttribute.ErrorMessage) ? 
                this.loggingAttribute.ErrorMessage : ex.Message;

            methodLogEntry.TransactionStatus = ex == null ? TransactionStatus.Success : TransactionStatus.Fail;
            if (this.loggingAttribute.InputLoggingReuired && invocationContext.Invocation.Arguments != null)
            {
                var serialier = JsonConvert.SerializeObject(invocationContext.Invocation.Arguments);
                serialier = serialier.Replace("\"", "'");
                serialier = serialier.Replace(",", ";");
                methodLogEntry.ExtendedProperties["inputData"] = "\"" + serialier + "\"";
                if (result != null)
                {
                    serialier = JsonConvert.SerializeObject(result);
                    serialier = serialier.Replace("\"", "'");
                    serialier = serialier.Replace(",", ";");
                    methodLogEntry.ExtendedProperties["outputData"] = "\"" + serialier + "\"";
                }
            }

            if (!this.loggingAttribute.IsFlowBreakReuired)
            {
                methodLogEntry.ExtendedProperties["isPartialError"] = "True";
            }

            // Log
            Log.Info(methodLogEntry);
        }

        private void HandleExceptionFlow(TransactionLogEntry methodLogEntry,
           InvocationContext invocationContext, IBaseHandler targetObject, Exception ex)
        {
            AppException exToThrow = null;
            if (ex is AppException)
            {
                var appEx = (AppException)ex;
                appEx.LogEntry = methodLogEntry;
                exToThrow = appEx;
            }
            else {
                exToThrow = new AppException(ex, methodLogEntry, this.loggingAttribute.ErrorMessage);
            }
            // LOG
            Log.Error(exToThrow);
            if (this.loggingAttribute.IsFlowBreakReuired)
                invocationContext.MethodException = exToThrow;
            else
                invocationContext.MethodException = null;
        }
        }
}
