using Iara.Application.Common;
using Iara.Application.DTOs.Auth;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Iara.Infrastructure.Security;

/// <summary>
/// Authentication service handling login, registration, and token management
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
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);

        if (user == null)
            return Result<AuthResponse>.Failure("Invalid username or password");

        if (!user.IsActive)
            return Result<AuthResponse>.Failure("User account is inactive");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Invalid username or password");

        var accessToken = _tokenService.GenerateAccessToken(user.UserId, user.Username, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Store refresh token in database
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.UserId,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedByIp = GetClientIpAddress(),
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        };

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        // Check if username already exists
        if (await _unitOfWork.Users.UsernameExistsAsync(request.Username, cancellationToken))
            return Result<AuthResponse>.Failure("Username already exists");

        // Check if email already exists
        if (!string.IsNullOrWhiteSpace(request.Email) && 
            await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
            return Result<AuthResponse>.Failure("Email already exists");

        // Validate role
        var validRoles = new[] { UserRoles.Admin, UserRoles.Inspector, UserRoles.Fisherman, UserRoles.Viewer };
        if (!validRoles.Contains(request.Role))
            return Result<AuthResponse>.Failure("Invalid role specified");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user.UserId, user.Username, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Store refresh token in database
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.UserId,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedByIp = GetClientIpAddress(),
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        };

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        // Validate and retrieve the refresh token from database
        var refreshTokenEntity = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken, cancellationToken);
        
        if (refreshTokenEntity == null)
            return Result<AuthResponse>.Failure("Invalid refresh token");

        if (!refreshTokenEntity.IsActive)
        {
            // Token was revoked or expired - potential token reuse attack
            await _unitOfWork.RefreshTokens.RevokeAllUserTokensAsync(refreshTokenEntity.UserId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<AuthResponse>.Failure("Token is invalid or has been revoked");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(refreshTokenEntity.UserId, cancellationToken);
        if (user == null)
            return Result<AuthResponse>.Failure("User not found");

        if (!user.IsActive)
            return Result<AuthResponse>.Failure("User account is inactive");

        // Generate new tokens
        var newAccessToken = _tokenService.GenerateAccessToken(user.UserId, user.Username, user.Role);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Revoke old refresh token and create new one
        refreshTokenEntity.IsRevoked = true;
        refreshTokenEntity.RevokedAt = DateTime.UtcNow;
        refreshTokenEntity.RevokedByIp = GetClientIpAddress();
        refreshTokenEntity.ReplacedByToken = newRefreshToken;
        _unitOfWork.RefreshTokens.Update(refreshTokenEntity);

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedByIp = GetClientIpAddress(),
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        };

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result> RevokeTokenAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username, cancellationToken);
        if (user == null)
            return Result.Failure("User not found");

        // Revoke all active refresh tokens for this user
        await _unitOfWork.RefreshTokens.RevokeAllUserTokensAsync(user.UserId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
