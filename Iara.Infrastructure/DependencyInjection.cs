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
        // Database Configuration - using SQL Server
        var connectionString = configuration.GetSection(DatabaseSettings.SectionName)["ConnectionString"]
            ?? configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured");
        }

        services.AddDbContext<IaraDbContext>(options =>
            options.UseSqlServer(connectionString));

        // HTTP Context Accessor for IP address tracking
        services.AddHttpContextAccessor();

        // Repository Registration
        services.AddScoped<IPersonnelRepository, PersonnelRepository>();
        services.AddScoped<IVesselRepository, VesselRepository>();
        services.AddScoped<IPermitRepository, PermitRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<ICatchQuotaRepository, CatchQuotaRepository>();
        services.AddScoped<ILogbookRepository, LogbookRepository>();
        services.AddScoped<IInspectionRepository, InspectionRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

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
        services.AddScoped<IVesselService, VesselService>();
        services.AddScoped<IPermitService, PermitService>();
        services.AddScoped<ILogbookService, LogbookService>();
        services.AddScoped<IInspectionService, NewInspectionService>();
        services.AddScoped<ISpeciesService, SpeciesService>();
        services.AddScoped<ICatchQuotaService, CatchQuotaService>();

        return services;
    }
}
