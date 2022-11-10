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

    [HttpPost("refreshToken")]
    public async Task<ActionResult> RefreshToken(string userName)
    {
        var refreshCookie = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshCookie))
        {
            return BadRequest("Invalid refresh login!");
        }

        var token = await _authService.RecycleRefreshToken(userName, refreshCookie);

        if (token.RefreshToken == string.Empty)
        {
            return BadRequest("Failed refresh login!");
        }

        SetRefreshToken(token.RefreshToken, token.ExpiresOn);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserRequest user)
    {
        var token = await _authService.Login(user);

        if (token.JwtToken == string.Empty)
        {
            return BadRequest("Failed login!");
        }
        SetRefreshToken(token.JwtToken, token.RefreshTokenExpiresOn);
        return Ok(token.JwtToken);
    }

    [HttpGet("logout")]
    public async Task<ActionResult<UserLogoutResponse>> Logout(string userName)
    {
        var refreshCookie = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshCookie))
        {
            return BadRequest("Invalid logout!");
        }

        var logout = await _authService.Logout(userName, refreshCookie);

        if (string.IsNullOrEmpty(logout.Message))
        {
            return BadRequest("Failed logout!");
        }

        return Ok((logout));
    }

    private void SetRefreshToken(string refreshToken, DateTime expiresOn)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = expiresOn
        };

        Response.Cookies.Append("refreshToken", refreshToken, options);
    }
}