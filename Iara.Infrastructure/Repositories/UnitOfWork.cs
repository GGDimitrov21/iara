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
        IPersonnelRepository personnel,
        IVesselRepository vessels,
        IPermitRepository permits,
        ISpeciesRepository species,
        ICatchQuotaRepository catchQuotas,
        ILogbookRepository logbook,
        IInspectionRepository inspections,
        ITicketRepository tickets)
    {
        _context = context;
        Personnel = personnel;
        Vessels = vessels;
        Permits = permits;
        Species = species;
        CatchQuotas = catchQuotas;
        Logbook = logbook;
        Inspections = inspections;
        Tickets = tickets;
    }

    public IPersonnelRepository Personnel { get; }
    public IVesselRepository Vessels { get; }
    public IPermitRepository Permits { get; }
    public ISpeciesRepository Species { get; }
    public ICatchQuotaRepository CatchQuotas { get; }
    public ILogbookRepository Logbook { get; }
    public IInspectionRepository Inspections { get; }
    public ITicketRepository Tickets { get; }

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
