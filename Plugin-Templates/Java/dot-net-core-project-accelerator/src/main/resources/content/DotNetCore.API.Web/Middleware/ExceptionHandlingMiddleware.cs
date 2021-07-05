using DotNetCore.API.Web.Security.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DotNetCore.Framework.ExceptionHandling.Models;

namespace DotNetCore.API.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiExceptionOptions _options;
        public ExceptionHandlingMiddleware(RequestDelegate next, ApiExceptionOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context,ex);
            }
           
        }
        private Task HandleException(HttpContext context, Exception ex)
        {
            var tranId = Guid.NewGuid().ToString();
            if (ex is AppException)
            {
                tranId = ((AppException)ex).LogEntry.TransactionId.ToString();
            }
            ApiExceptionResponse response = new ApiExceptionResponse
            {
                ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                FriendlyMessage = "We're sorry. The system encountered an error condition and the request could not be completed.",
                RequestPath = context.Request.Path,
                ExceptionId = tranId
            };
            _options.SetExceptionResponse?.Invoke(context, ex, response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));


        }
      

    }
}
