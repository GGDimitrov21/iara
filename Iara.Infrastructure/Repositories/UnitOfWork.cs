using Microsoft.EntityFrameworkCore.Storage;
using Iara.Domain.Repositories;
using Iara.Infrastructure.Persistence;

namespace Iara.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation coordinating repositories and transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IaraDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        IaraDbContext context,
        IUserRepository users,
        IFishingShipRepository fishingShips,
        IRegistrationRepository registrations,
        IFishingPermitRepository fishingPermits,
        IFishingLogEntryRepository fishingLogEntries,
        ICatchCompositionRepository catchCompositions,
        IInspectionRepository inspections,
        IShipClassificationLogRepository shipClassificationLogs,
        IRefreshTokenRepository refreshTokens)
    {
        _context = context;
        Users = users;
        FishingShips = fishingShips;
        Registrations = registrations;
        FishingPermits = fishingPermits;
        FishingLogEntries = fishingLogEntries;
        CatchCompositions = catchCompositions;
        Inspections = inspections;
        ShipClassificationLogs = shipClassificationLogs;
        RefreshTokens = refreshTokens;
    }

    public IUserRepository Users { get; }
    public IFishingShipRepository FishingShips { get; }
    public IRegistrationRepository Registrations { get; }
    public IFishingPermitRepository FishingPermits { get; }
    public IFishingLogEntryRepository FishingLogEntries { get; }
    public ICatchCompositionRepository CatchCompositions { get; }
    public IInspectionRepository Inspections { get; }
    public IShipClassificationLogRepository ShipClassificationLogs { get; }
    public IRefreshTokenRepository RefreshTokens { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
