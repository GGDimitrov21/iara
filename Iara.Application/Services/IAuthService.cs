using Iara.Application.Common;
using Iara.Application.DTOs.Auth;

namespace Iara.Application.Services;

/// <summary>
/// Interface for authentication service
/// </summary>
public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokeTokenAsync(string username, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for JWT token service
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(int userId, string username, string role);
    string GenerateRefreshToken();
    int? ValidateAccessToken(string token);
    bool ValidateRefreshToken(string token);
}

/// <summary>
/// Interface for password hashing service
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
