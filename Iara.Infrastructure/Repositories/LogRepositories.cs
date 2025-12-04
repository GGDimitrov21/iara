using Microsoft.EntityFrameworkCore;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Iara.Infrastructure.Persistence;

namespace Iara.Infrastructure.Repositories;

public class FishingLogEntryRepository : Repository<FishingLogEntry>, IFishingLogEntryRepository
{
    public FishingLogEntryRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FishingLogEntry>> GetLogEntriesByShipIdAsync(int shipId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.ShipId == shipId)
            .Include(l => l.Ship)
            .Include(l => l.CatchCompositions)
            .OrderByDescending(l => l.LogDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<FishingLogEntry?> GetLogEntryWithCatchesAsync(long logEntryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Ship)
            .Include(l => l.CatchCompositions)
            .FirstOrDefaultAsync(l => l.LogEntryId == logEntryId, cancellationToken);
    }

    public async Task<IEnumerable<FishingLogEntry>> GetLogEntriesByDateRangeAsync(
        int shipId, 
        DateOnly startDate, 
        DateOnly endDate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.ShipId == shipId && l.LogDate >= startDate && l.LogDate <= endDate)
            .Include(l => l.CatchCompositions)
            .OrderBy(l => l.LogDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> LogEntryExistsForDateAsync(int shipId, DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(l => l.ShipId == shipId && l.LogDate == date, cancellationToken);
    }
}

public class CatchCompositionRepository : Repository<CatchComposition>, ICatchCompositionRepository
{
    public CatchCompositionRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CatchComposition>> GetCatchesByLogEntryIdAsync(long logEntryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.LogEntryId == logEntryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CatchComposition>> GetCatchesBySpeciesAsync(string species, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.FishSpecies == species)
            .Include(c => c.LogEntry)
            .ToListAsync(cancellationToken);
    }
}

public class InspectionRepository : Repository<Inspection>, IInspectionRepository
{
    public InspectionRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Inspection>> GetInspectionsByShipIdAsync(int shipId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => i.ShipId == shipId)
            .Include(i => i.Inspector)
            .Include(i => i.Ship)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inspection>> GetInspectionsByInspectorIdAsync(int inspectorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => i.InspectorId == inspectorId)
            .Include(i => i.Ship)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inspection>> GetViolationsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => i.HasViolation)
            .Include(i => i.Inspector)
            .Include(i => i.Ship)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inspection>> GetUnprocessedInspectionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => !i.IsProcessed)
            .Include(i => i.Inspector)
            .Include(i => i.Ship)
            .OrderBy(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Inspection?> GetByProtocolNumberAsync(string protocolNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Inspector)
            .Include(i => i.Ship)
            .FirstOrDefaultAsync(i => i.ProtocolNumber == protocolNumber, cancellationToken);
    }
}

public class ShipClassificationLogRepository : Repository<ShipClassificationLog>, IShipClassificationLogRepository
{
    public ShipClassificationLogRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ShipClassificationLog>> GetClassificationsByShipIdAsync(int shipId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ShipId == shipId)
            .Include(c => c.Ship)
            .OrderByDescending(c => c.ClassificationYear)
            .ToListAsync(cancellationToken);
    }

    public async Task<ShipClassificationLog?> GetClassificationByYearAsync(int shipId, int year, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Ship)
            .FirstOrDefaultAsync(c => c.ShipId == shipId && c.ClassificationYear == year, cancellationToken);
    }
}

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeAllUserTokensAsync(int userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _dbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }
    }

    public async Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await _dbSet
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow.AddDays(-30))
            .ToListAsync(cancellationToken);

        _dbSet.RemoveRange(expiredTokens);
    }
}
