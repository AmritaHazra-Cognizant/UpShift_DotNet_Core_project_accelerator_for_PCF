using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace DotNetCore.API.Web.Security.Implementation
{

    /// <summary>
    /// This class will hold the Loggedin User Data
    /// </summary>
    public class SignOnUser
    {
        private readonly IHttpContextAccessor _currentContextAccessor;
        private readonly HttpContext currentContext;
        public SignOnUser(IHttpContextAccessor currentContextAccessor)
        {
            this._currentContextAccessor = currentContextAccessor;
            if (this._currentContextAccessor != null)
            {
                currentContext = this._currentContextAccessor.HttpContext;
            }

            // TO DO: initialize with test data.
            UserId = "Test-001";
            DisplayName = "TestUser";
            UserRoleGroups = new List<string>() { "roles" };
            Email = "Test@Test.com";
            FirstName = "Test";
            LastName = "Test";

        }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public List<string> UserRoleGroups { get; set; }

        /// <summary>
        /// Get header value
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        private string GetRequestHeaderValue(string headerName)
        {
            if (this.currentContext != null)
            {
                if (currentContext.Request.Headers.ContainsKey(headerName))
                    return currentContext.Request.Headers[headerName];
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
