using JwtAuth.API.APIModels;
using JwtAuth.API.Authorization;
using JwtAuth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.API.Controllers
{
    [Authorize(Roles = nameof(AuthorizationRoles.PortalOwner))]
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController: ControllerBase
    {
        private readonly IAuthPortalService _authService;
        private readonly ILogger<AccessController> _logger;

        public AccessController(IAuthPortalService authService, ILogger<AccessController> logger)
		{
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthTokenResponse>> Register(UserRequest user)
        {
            var bearerToken = Request.Headers.Authorization.FirstOrDefault();

            if (string.IsNullOrEmpty(bearerToken))
            {
                return BadRequest("Invalid registration!");
            }

            var access = await _authService.Register(user, bearerToken.Replace("bearer ", string.Empty));

            if (string.IsNullOrEmpty(access.UserName))
            {
                return BadRequest("Failed registration!");
            }

            return Ok(access);
        }
    }
}

