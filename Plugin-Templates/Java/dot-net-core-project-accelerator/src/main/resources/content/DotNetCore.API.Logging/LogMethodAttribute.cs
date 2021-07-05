using DotNetCore.Framework.Interception.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Logging
{
    public class LogMethodAttribute : MethodInterceptionAttribute
    {

        #region Properties
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public bool InputLoggingReuired { get; set; }
        public int Order { get; set; }
        public bool IsExceptionHandlingNotRequired { get; set; }
        public bool IsFlowBreakReuired { get; set; }

        #endregion

        public LogMethodAttribute(string errorCode, string errorMessage,
            bool inputLoggingRequired = false, 
            bool isExceptionHandlingNotReuired = false, 
            bool isFlowBreakReuired = false)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            InputLoggingReuired = inputLoggingRequired;
            IsExceptionHandlingNotRequired = isExceptionHandlingNotReuired;
            IsFlowBreakReuired = isFlowBreakReuired;
        }
    }
}
