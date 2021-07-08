using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using DotNetCore.API.Web.Security.Implementation;

namespace DotNetCore.API.Web.Security.Service
{
    public interface IAuthenticationService
    {
        AuthenticateResponse Authenticate(string connectionId);
        AuthenticateResponse Authenticate(AuthenticateRequest request, string connectionId);
        ClaimsIdentity GetSignOnUserIdentity();

        ClaimsIdentity GetAuthenticateduser(string token);
    }
}
