using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.RestService
{
  public  class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public IDictionary<string,string> Headers { get; set; }
        public T Data { get; private set; }

        public ApiResponse(int statusCode, IDictionary<string, string> headers, T data)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
            this.Data = data;
        }
    }
}
