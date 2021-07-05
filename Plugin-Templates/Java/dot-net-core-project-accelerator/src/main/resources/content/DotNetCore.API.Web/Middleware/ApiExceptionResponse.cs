using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Middleware
{
    public class ApiExceptionResponse
    {
        public string ExceptionId { get; set; }
        public string RequestPath { get; set; }
        public string FriendlyMessage { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
