using JwtAuth.API.APIModels;
using JwtAuth.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
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
    public async Task<ActionResult<bool>> RefreshToken()
    {
        return Ok(true);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserRequest user)
    {
        return Ok(await _authService.Login(user));
    }

    [HttpGet("logout")]
    public async Task<ActionResult<UserLoggedResponse>> Logout(UserRequest user)
    {
        return Ok(await _authService.Logout(user));
    }
}

