using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Security.Configuration
{
    public interface IConfigJwtWebToken
    {
        string SharedSecretBase64 { get; }
        string Issuer { get; }
        string Audience { get; }

    }
    public class ConfigJwtWebToken : IConfigJwtWebToken
    {
        public const string JwtWebToken = "JwtWebToken";
        public string SharedSecretBase64 { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
