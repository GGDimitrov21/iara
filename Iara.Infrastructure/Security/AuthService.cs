using Iara.Application.Common;
using Iara.Application.DTOs.Auth;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Iara.Infrastructure.Security;

/// <summary>
/// Authentication service handling login and token management
/// Uses Personnel table for authentication based on email and password
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtSettings,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetClientIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        // Find personnel by email (using username as email)
        var personnel = await _unitOfWork.Personnel.GetByEmailAsync(request.Username, cancellationToken);

        if (personnel == null)
            return Result<AuthResponse>.Failure("Invalid credentials");

        if (!personnel.IsActive)
            return Result<AuthResponse>.Failure("Account is inactive");

        // Verify password
        if (string.IsNullOrEmpty(personnel.PasswordHash) || 
            !_passwordHasher.VerifyPassword(request.Password, personnel.PasswordHash))
            return Result<AuthResponse>.Failure("Invalid credentials");
        
        var accessToken = _tokenService.GenerateAccessToken(personnel.PersonId, personnel.Name, personnel.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var response = new AuthResponse
        {
            UserId = personnel.PersonId,
            Username = personnel.ContactEmail ?? personnel.Name,
            FullName = personnel.Name,
            Role = personnel.Role,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        };

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        // Check if email already exists
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existing = await _unitOfWork.Personnel.GetByEmailAsync(request.Email, cancellationToken);
            if (existing != null)
                return Result<AuthResponse>.Failure("Email already exists");
        }

        // Validate role
        var validRoles = new[] { "Admin", "Inspector", "Captain", "Viewer" };
        if (!validRoles.Contains(request.Role))
            return Result<AuthResponse>.Failure("Invalid role specified");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var personnel = new Personnel
        {
            Name = request.FullName,
            Role = request.Role,
            ContactEmail = request.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Personnel.AddAsync(personnel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(personnel.PersonId, personnel.Name, personnel.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var response = new AuthResponse
        {
            UserId = personnel.PersonId,
            Username = personnel.ContactEmail ?? personnel.Name,
            FullName = personnel.Name,
            Role = personnel.Role,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        };

        return Result<AuthResponse>.Success(response);
    }

    public Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        // Simplified token refresh - in production, you'd validate against stored tokens
        return Task.FromResult(Result<AuthResponse>.Failure("Token refresh requires re-login"));
    }

    public async Task<Result> RevokeTokenAsync(string username, CancellationToken cancellationToken = default)
    {
        var personnel = await _unitOfWork.Personnel.GetByEmailAsync(username, cancellationToken);
        if (personnel == null)
            return Result.Failure("User not found");

        return Result.Success();
    }
}
