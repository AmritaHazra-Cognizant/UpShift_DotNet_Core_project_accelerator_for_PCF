using DotNetCore.API.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Extensions
{
    public static class CustomMiddlewareExtensions
    {
        internal static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }

        internal static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>(new ApiExceptionOptions());
        }
        internal static IApplicationBuilder UseCoreResponseHaedersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseHeadersMiddleware>();
        }
        internal static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder, Action<ApiExceptionOptions> config)
        {
            var options = new ApiExceptionOptions();
            config(options);
            return builder.UseMiddleware<ExceptionHandlingMiddleware>(options);
        }
    }
}
