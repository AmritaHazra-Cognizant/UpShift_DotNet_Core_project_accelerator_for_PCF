using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DotNetCore.API.Web.Security.Implementation
{
    public class AuthenticateResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("groups")]
        public virtual ICollection Groups { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("jwtToken")]
        public string JwtToken { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        public AuthenticateResponse()
        { }
        public AuthenticateResponse(UserToken user, string jwtToken, string refreshToken)
        {
            UserId = user.UserId;
            Groups = user.Groups;
            DisplayName = user.DisplayName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            ConnectionId = user.ConnectionId;
        }
    }
}
