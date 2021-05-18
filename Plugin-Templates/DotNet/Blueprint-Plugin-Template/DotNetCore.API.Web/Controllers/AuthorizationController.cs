using DotNetCore.API.Web.Security.Implementation;
using DotNetCore.API.Web.Security.Service;
using DotNetCore.Framework.Logging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizationController(TransactionLogEntry logEntry,
            IAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor) : base(logEntry)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("singleSignOn")]
        public ActionResult AuthenticateWithSSO()
        {
            try
            {
                var response = _authenticationService.Authenticate(ConnectionId());
                if (response != null)
                {
                    LogEntry.LogInUserId = response.UserId;
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("authenticate")]
        public ActionResult Authenticate(AuthenticateRequest request)
        {
            try
            {
                var response = _authenticationService.Authenticate(request,ConnectionId());
                if (response != null)
                {
                    LogEntry.LogInUserId = response.UserId;
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Request.Headers.Clear();
            foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return Ok(new { message = "Token revoked." });
        }
        private string ConnectionId()
        {
            return _httpContextAccessor.HttpContext.Connection.Id;
        }
    }
}
