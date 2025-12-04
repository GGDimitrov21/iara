using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Iara.Application.Configuration;
using Iara.Application.Services;
using Iara.Domain.Repositories;
using Iara.Infrastructure.Persistence;
using Iara.Infrastructure.Repositories;
using Iara.Infrastructure.Security;
using Iara.Infrastructure.Services;

namespace Iara.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection configuration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database Configuration
        var connectionString = configuration.GetSection(DatabaseSettings.SectionName)["ConnectionString"];
        
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured");
        }

        services.AddDbContext<IaraDbContext>(options =>
            options.UseNpgsql(connectionString));

        // HTTP Context Accessor for IP address tracking
        services.AddHttpContextAccessor();

        // Repository Registration
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFishingShipRepository, FishingShipRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<IFishingPermitRepository, FishingPermitRepository>();
        services.AddScoped<IFishingLogEntryRepository, FishingLogEntryRepository>();
        services.AddScoped<ICatchCompositionRepository, CatchCompositionRepository>();
        services.AddScoped<IInspectionRepository, InspectionRepository>();
        services.AddScoped<IShipClassificationLogRepository, ShipClassificationLogRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Security Services
        services.Configure<JwtSettings>(options =>
        {
            configuration.GetSection(JwtSettings.SectionName).Bind(options);
        });
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

        // Business Services
        services.AddScoped<IFishingShipService, FishingShipService>();
        services.AddScoped<IFishingPermitService, FishingPermitService>();
        services.AddScoped<IFishingLogEntryService, FishingLogEntryService>();
        services.AddScoped<IInspectionService, InspectionService>();

        return services;
    }
}
