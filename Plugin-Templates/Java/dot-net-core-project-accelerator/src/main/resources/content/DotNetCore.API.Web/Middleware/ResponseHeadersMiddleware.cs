using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Middleware
{
    public class ResponseHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseHeadersMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public async Task InvokeAsync(HttpContext context)
        {
            // For Very first request clear cache
            if (context.Request.Path.Value=="/")
            {
                context.Response.Headers.Add("Cache-Control", "no-store,no-cache");
                context.Response.Headers.Add("Pragma", "no-cache");
            }
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            // To DO:-
            // Add your Custome Headers
            await _next(context);
        }
    }
}
