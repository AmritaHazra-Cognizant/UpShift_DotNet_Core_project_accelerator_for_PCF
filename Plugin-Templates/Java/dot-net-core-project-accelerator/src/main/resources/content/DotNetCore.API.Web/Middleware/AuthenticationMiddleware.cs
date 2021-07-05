using DotNetCore.API.Web.Security.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthenticationService _userProvider;
        public AuthenticationMiddleware(RequestDelegate next, IAuthenticationService userProvider)
        {
            _next = next;
            _userProvider = userProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           // By pass Token Checking for Authorization API
            if (!context.Request.Path.Value.ToLower().Contains("authorization/"))
            {
                string token;
                if (!TryRetrieveToken(context.Request, out token))
                {
                    // Short circuit the request pipeline
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                   
                }
                else
                {
                    ClaimsIdentity currentUser = _userProvider.GetAuthenticateduser(token);
                    if (null == currentUser)
                    {
                        // Short circuit the request pipeline
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    }
                    else {
                        context.User = new ClaimsPrincipal(currentUser);
                        await _next(context);
                    }
                }
            }
            else {
                await _next(context);
            }
           
        }

        private bool TryRetrieveToken(Microsoft.AspNetCore.Http.HttpRequest request, out string token)
        {
            token = null;
            Microsoft.Extensions.Primitives.StringValues authHeaders;
            if (!request.Headers.TryGetValue("Authorization", out authHeaders) || authHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }
}
