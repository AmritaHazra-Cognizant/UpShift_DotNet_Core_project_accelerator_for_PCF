using DotNetCore.API.Web.Security.Configuration;
using DotNetCore.API.Web.Security.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Security.Service
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly SignOnUser _signOnUser;

        private readonly IConfigJwtWebToken _config;
        private readonly IWebHostEnvironment _env;
        public AuthenticationService(IOptions<ConfigJwtWebToken> config,
            SignOnUser signOnUser, IWebHostEnvironment env)
        {
            _signOnUser = signOnUser;
            _env = env;
            _config = config.Value;

        }
        public AuthenticateResponse Authenticate(string connectionId)
        {
            AuthenticateResponse response = null;
            if (_signOnUser != null)
            {
                var channelsecureIdentity = GetSignOnUserIdentity();
                var authToken = CreateJwtToken(_config, channelsecureIdentity);
                response = new AuthenticateResponse()
                {
                    UserId = _signOnUser.UserId,
                    Groups = _signOnUser.UserRoleGroups,
                    DisplayName = _signOnUser.DisplayName,
                    JwtToken = authToken,
                    ConnectionId = connectionId
                };
            }
            return response;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest request,
            string connectionId)
        {
            // To DO: Call External service or DB service to validate the user
            // We need o initialize _signOnUser object.
            // For now we are setting alaways true here.

            var isValidUser = true;

            if (isValidUser)
            {
                _signOnUser.UserId = request.UserName;
            }

            AuthenticateResponse response = null;
            if (_signOnUser != null)
            {
                var channelsecureIdentity = GetSignOnUserIdentity();
                var authToken = CreateJwtToken(_config, channelsecureIdentity);
                response = new AuthenticateResponse()
                {
                    UserId = _signOnUser.UserId,
                    Groups = _signOnUser.UserRoleGroups,
                    DisplayName = _signOnUser.DisplayName,
                    JwtToken = authToken,
                    ConnectionId = connectionId
                };
            }

            return response;
        }

        public ClaimsIdentity GetAuthenticateduser(string token)
        {
            ClaimsIdentity currentUser = null;

            if (null == token)
            {
                //logger.Logarning("Authorization token not provided.");
            }
            else
            {
                currentUser = FromToken(token);


            }
            return currentUser;
        }
        /*
         * Please note: this should be considered a preliminary prototype for an approach to extracting client user
        * information from the subject segment of a passed Jwt Authorization token.
        • The format of the content in the token Subject segment and its subsequent mapping to a
        * ClaimsIdentity or a Claimsprinciple instance is TBD.
        **/
        private ClaimsIdentity FromToken(string token)
        {
            ClaimsIdentity currentUser = null;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Convert.FromBase64String(_config.SharedSecretBase64);
                //var key = Encoding.ASCII.GetBytes(_config.secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set Clockskew to zero so tokens expire exactly at token expiration time(instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                DateTime effective = FromJsonNumeric(jwtToken.Claims.First(x => x.Type == JwtClaimKey.NotBefore).Value);
                DateTime expiry = FromJsonNumeric(jwtToken.Claims.First(x => x.Type == JwtClaimKey.Expires).Value);
                DateTime issued = FromJsonNumeric(jwtToken.Claims.First(x => x.Type == JwtClaimKey.IssuedAt).Value);


                // allow long lifespan token in dev only
                bool isWithinProdTimelimit = effective.ToUniversalTime().AddMinutes(15) > DateTime.Now.ToUniversalTime();
                bool iswithinDevTimeLimit = expiry > effective;
                bool isExpired = _env.IsDevelopment() ? !iswithinDevTimeLimit : !isWithinProdTimelimit;
                string issuer = jwtToken.Claims.First(x => x.Type == JwtClaimKey.Issuer).Value;
                string[] tokenAudience = jwtToken.Claims.First(x => x.Type == JwtClaimKey.Audience).Value.Split(", ");
                string[] thisAudience = _config.Audience.Split(',');

                bool isInAudience = tokenAudience.Intersect(thisAudience).Any();
                // roughly following https://tools.ietf.org/html/draft-ietf-oauth-json-web-token-32#section-4.1.1
                if (!isExpired && isInAudience && issued <= effective &&
                _config.Issuer.Equals(issuer))
                {

                    // use of ApplicationCurrentUser is arbitrary and subject to change
                    currentUser = new ClaimsIdentity(jwtToken.Claims);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Malformed Jwt token");
                // do nothing if jut validation fails
                // user is not attached to context so request won't have access to secure routes
            }
            return currentUser;
        }
        private static DateTime FromJsonNumeric(string secondsString)
        {
            return new DateTime(1970, 1, 1).ToUniversalTime().AddSeconds(double.Parse(secondsString));
        }
        public ClaimsIdentity GetSignOnUserIdentity()
        {
            var signOnuserClaim = new List<Claim>();
            signOnuserClaim.Add(new Claim(ClaimTypes.Name, _signOnUser.DisplayName));
            signOnuserClaim.Add(new Claim(ClaimTypes.AuthenticationMethod, AuthorizationConstants.SIGN_ON_METHOD));
            signOnuserClaim.Add(new Claim(ClaimTypes.Email, _signOnUser.Email));
            signOnuserClaim.Add(new Claim(ClaimTypes.GivenName, _signOnUser.FirstName));
            signOnuserClaim.Add(new Claim(ClaimTypes.Surname, _signOnUser.LastName));
            signOnuserClaim.Add(new Claim(AuthorizationConstants.USER_ID, _signOnUser.UserId));
            foreach (var group in _signOnUser.UserRoleGroups)
            {
                signOnuserClaim.Add(new Claim(ClaimTypes.Role, group));
            }
            var signOnUserIdentity = new ClaimsIdentity(signOnuserClaim, AuthorizationConstants.SIGN_ON_IDENTITY);
            return signOnUserIdentity;
        }

        internal static string CreateJwtToken(IConfigJwtWebToken config, ClaimsIdentity claimIdentity)
        {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(config.SharedSecretBase64));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                NotBefore = DateTime.Now.ToUniversalTime(),
                Expires = DateTime.Now.AddMinutes(15).ToUniversalTime(),
                Issuer = config.Issuer,
                Audience = config.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        private RefreshToken RefreshJwtToken(string ipAddress)
        {
            using (var ingCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                ingCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }


    }
}
