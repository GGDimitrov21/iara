namespace Iara.Domain.Repositories;

/// <summary>
/// Unit of Work pattern for transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IFishingShipRepository FishingShips { get; }
    IRegistrationRepository Registrations { get; }
    IFishingPermitRepository FishingPermits { get; }
    IFishingLogEntryRepository FishingLogEntries { get; }
    ICatchCompositionRepository CatchCompositions { get; }
    IInspectionRepository Inspections { get; }
    IShipClassificationLogRepository ShipClassificationLogs { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
