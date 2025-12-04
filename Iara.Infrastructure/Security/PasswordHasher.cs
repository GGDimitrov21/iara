using Iara.Application.Services;

namespace Iara.Infrastructure.Security;

/// <summary>
/// BCrypt password hasher implementation for secure password storage
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12; // BCrypt work factor (higher = more secure but slower)

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }
}
