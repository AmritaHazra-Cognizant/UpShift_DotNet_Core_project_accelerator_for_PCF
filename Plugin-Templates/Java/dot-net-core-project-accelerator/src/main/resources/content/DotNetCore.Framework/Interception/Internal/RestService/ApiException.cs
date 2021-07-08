using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.RestService
{
  public  class ApiException:Exception
    {
        public int ErrorCode { get; set; }
        public dynamic ErrorContent { get; set; }
        public ApiException() { }

        public ApiException(int errorCode, string message):base(message)
        {
            ErrorCode = errorCode;
        }
        public ApiException(int errorCode, string message, string errorContent) : base(message)
        {
            ErrorCode = errorCode;
            if (errorContent != null)
                ErrorContent = errorContent;
        }
    }
}
