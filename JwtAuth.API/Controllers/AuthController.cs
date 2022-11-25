using FluentValidation;
using JwtAuth.API.APIModels;
using JwtAuth.API.Authorization;
using JwtAuth.API.Services.Interfaces;
using JwtAuth.API.Validation;
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
    private readonly IValidator<UserRequest> _loginValidator;
    private readonly IValidator<string> _userNamevalidator;

    public AuthController(IAuthPortalService authService, IValidator<string> userNamevalidator, IValidator<UserRequest> loginValidator, ILogger<AuthController> logger)
    {
        _logger = logger;
        _loginValidator = loginValidator;
        _userNamevalidator = userNamevalidator;
        _authService = authService;
    }

    [HttpPost("refreshToken")]
    public async Task<ActionResult> RefreshToken(string username)
    {
        var validation = await _userNamevalidator.ValidateAsync(username);

        if (!validation.IsValid)
        {
            return BadRequest($"Invalid username: {string.Join(' ', validation.Errors.ToList())}");
        }

        var refreshCookie = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshCookie))
        {
            return BadRequest("Invalid refresh login!");
        }

        var token = await _authService.RecycleRefreshToken(username, refreshCookie);

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
        var validation = await _loginValidator.ValidateAsync(user);

        if (!validation.IsValid)
        {
            return BadRequest($"Invalid login request: {string.Join(' ', validation.Errors.ToList())}");
        }

        var token = await _authService.Login(user);

        if (token.JwtToken == string.Empty)
        {
            return BadRequest("Failed login!");
        }
        SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresOn);
        return Ok(token.JwtToken);
    }

    [HttpGet("logout")]
    public async Task<ActionResult<UserLogoutResponse>> Logout(string username)
    {
        var validation = await _userNamevalidator.ValidateAsync(username);

        if (!validation.IsValid)
        {
            return BadRequest($"Invalid username: {string.Join(' ', validation.Errors.ToList())}");
        }

        var refreshCookie = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshCookie))
        {
            return BadRequest("Invalid logout!");
        }

        var logout = await _authService.Logout(username, refreshCookie);

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