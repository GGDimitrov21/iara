using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// User entity representing system users (fishermen, inspectors, admins)
/// </summary>
public class User : BaseEntity
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

/// <summary>
/// User roles in the system
/// </summary>
public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Inspector = "Inspector";
    public const string Fisherman = "Fisherman";
    public const string Viewer = "Viewer";
}
