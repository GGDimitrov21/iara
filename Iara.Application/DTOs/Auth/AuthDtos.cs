namespace Iara.Application.DTOs.Auth;

/// <summary>
/// DTO for user login request
/// </summary>
public record LoginRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// DTO for user registration request
/// </summary>
public record RegisterRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string Role { get; init; } = string.Empty;
}

/// <summary>
/// DTO for authentication response with JWT token
/// </summary>
public record AuthResponse
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}

/// <summary>
/// DTO for token refresh request
/// </summary>
public record RefreshTokenRequest
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
