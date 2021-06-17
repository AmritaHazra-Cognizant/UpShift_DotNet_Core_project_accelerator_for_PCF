using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Security.Implementation
{
    public class UserToken
    {
        public string UserId { get; set; }
        public virtual ICollection Groups { get; set; }
        public string DisplayName { get; set; }
        public string ConnectionId { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens {get;set;}
    }
}
