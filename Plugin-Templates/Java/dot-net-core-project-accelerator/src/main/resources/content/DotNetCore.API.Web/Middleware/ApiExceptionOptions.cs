using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Middleware
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiExceptionResponse> SetExceptionResponse { get; set; }
    }
}
