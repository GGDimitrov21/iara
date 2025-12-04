namespace Iara.Application.Configuration;

/// <summary>
/// Database configuration settings
/// </summary>
public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";
    public string ConnectionString { get; set; } = string.Empty;
}
