using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Personnel entity representing captains, inspectors, admins, owners, and users
/// </summary>
public class Personnel : BaseEntity
{
    public int PersonId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Vessel> CaptainedVessels { get; set; } = new List<Vessel>();
    public ICollection<Logbook> LogbookEntries { get; set; } = new List<Logbook>();
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}

/// <summary>
/// Personnel roles in the system
/// </summary>
public static class PersonnelRoles
{
    public const string Captain = "Captain";
    public const string Inspector = "Inspector";
    public const string Admin = "Admin";
    public const string Owner = "Owner";
    public const string User = "User";
}
