using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Security.Implementation
{
    public class JwtClaimKey
    {
        public const string Issuer = "iss";
        public const string Subject = "sub";
        public const string Audience = "aud";
        public const string Expires = "exp";
        public const string NotBefore = "nbf";
        public const string IssuedAt = "iat";
        public const string ClientId = "client_id";

    }
}
