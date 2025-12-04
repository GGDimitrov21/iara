using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Auth;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Authentication controller for user login, registration, and token management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticate user and get access token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request.Username);

        var result = await _authService.LoginAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Login failed for username: {Username}", request.Username);
            return Unauthorized(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Login successful for username: {Username}", request.Username);
        return Ok(result.Data);
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    [Authorize(Roles = "Admin")] // Only admins can register new users
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registration attempt for username: {Username}", request.Username);

        var result = await _authService.RegisterAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Registration failed for username: {Username} - {Error}", request.Username, result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Registration successful for username: {Username}", request.Username);
        return CreatedAtAction(nameof(Login), new { username = request.Username }, result.Data);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Token refresh attempt");

        var result = await _authService.RefreshTokenAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Token refresh failed: {Error}", result.ErrorMessage);
            return Unauthorized(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Token refresh successful");
        return Ok(result.Data);
    }

    /// <summary>
    /// Revoke user's refresh tokens (logout)
    /// </summary>
    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> RevokeToken(CancellationToken cancellationToken)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }

        _logger.LogInformation("Token revocation for username: {Username}", username);

        var result = await _authService.RevokeTokenAsync(username, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return NoContent();
    }
}
