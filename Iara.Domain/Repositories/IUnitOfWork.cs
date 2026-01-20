namespace Iara.Domain.Repositories;

/// <summary>
/// Unit of Work pattern for transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IPersonnelRepository Personnel { get; }
    IVesselRepository Vessels { get; }
    IPermitRepository Permits { get; }
    ISpeciesRepository Species { get; }
    ICatchQuotaRepository CatchQuotas { get; }
    ILogbookRepository Logbook { get; }
    IInspectionRepository Inspections { get; }
    ITicketRepository Tickets { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
