using System.Data;
using JwtAuth.API.APIModels;
using JwtAuth.API.Authorization;
using JwtAuth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.API.Controllers;

[Authorize(Roles = nameof(AuthorizationRoles.Traveler))]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthPortalService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthPortalService authService, ILogger<AuthController> logger)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task <ActionResult<AuthTokenResponse>> Register(UserRequest user)
    {
        return Ok(await _authService.Register(user));
    }

    [HttpPost("refreshToken")]
    public ActionResult<bool> RefreshToken()
    {
        return Ok(true);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserRequest user)
    {
        var token = await _authService.Login(user);

        if (token == string.Empty)
        {
            return BadRequest("Failed login!");
        }
        return Ok(token);
    }

    [HttpGet("logout")]
    public ActionResult<UserLoggedResponse> Logout(UserRequest user)
    {
        return Ok(_authService.Logout(user));
    }
}

