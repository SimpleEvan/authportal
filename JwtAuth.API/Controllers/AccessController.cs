using FluentValidation;
using System;
using JwtAuth.API.APIModels;
using JwtAuth.API.Authorization;
using JwtAuth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;

namespace JwtAuth.API.Controllers
{
    [Authorize(Roles = nameof(AuthorizationRoles.PortalOwner))]
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController: ControllerBase
    {
        private readonly IAuthPortalService _authService;
        private readonly IValidator<UserRequest> _validator;
        private readonly ILogger<AccessController> _logger;

        public AccessController(IAuthPortalService authService, IValidator<UserRequest> validator, ILogger<AccessController> logger)
		{
            _logger = logger;
            _validator = validator;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthTokenResponse>> Register(UserRequest user)
        {
            using (Operation.Time("Register user: ", user.Username))
            {
                var validation = await _validator.ValidateAsync(user);

                if (!validation.IsValid)
                {
                    return BadRequest($"Invalid registration request: {string.Join(' ', validation.Errors.ToList())}");
                }

                var bearerToken = Request.Headers.Authorization.FirstOrDefault();

                if (string.IsNullOrEmpty(bearerToken))
                {
                    return BadRequest("Invalid registration!");
                }

                var access = await _authService.Register(user, bearerToken.Replace("bearer ", string.Empty));

                if (string.IsNullOrEmpty(access.Username))
                {
                    return BadRequest("Failed registration!");
                }
                return Ok(access);
            }
        }
    }
}

