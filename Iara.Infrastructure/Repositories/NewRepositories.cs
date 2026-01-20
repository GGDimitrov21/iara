using Microsoft.EntityFrameworkCore;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Iara.Infrastructure.Persistence;

namespace Iara.Infrastructure.Repositories;

public class PersonnelRepository : Repository<Personnel>, IPersonnelRepository
{
    public PersonnelRepository(IaraDbContext context) : base(context) { }

    public async Task<Personnel?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.ContactEmail == email, cancellationToken);
    }

    public async Task<IEnumerable<Personnel>> GetByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.Role == role).ToListAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(p => p.ContactEmail == email, cancellationToken);
    }
}

public class VesselRepository : Repository<Vessel>, IVesselRepository
{
    public VesselRepository(IaraDbContext context) : base(context) { }

    public async Task<Vessel?> GetByRegNumberAsync(string regNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(v => v.RegNumber == regNumber, cancellationToken);
    }

    public async Task<IEnumerable<Vessel>> GetByCaptainIdAsync(int captainId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(v => v.CaptainId == captainId).ToListAsync(cancellationToken);
    }

    public async Task<Vessel?> GetWithDetailsAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.Captain)
            .Include(v => v.Permits)
            .FirstOrDefaultAsync(v => v.VesselId == vesselId, cancellationToken);
    }
}

public class PermitRepository : Repository<Permit>, IPermitRepository
{
    public PermitRepository(IaraDbContext context) : base(context) { }

    public async Task<IEnumerable<Permit>> GetByVesselIdAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Vessel)
            .Where(p => p.VesselId == vesselId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Permit?> GetActivePermitForVesselAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Vessel)
            .FirstOrDefaultAsync(p => p.VesselId == vesselId && p.IsActive && p.ExpiryDate >= DateTime.Now, cancellationToken);
    }

    public async Task<IEnumerable<Permit>> GetActivePermitsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Vessel)
            .Where(p => p.IsActive && p.ExpiryDate >= DateTime.Now)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permit>> GetExpiringPermitsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Vessel)
            .Where(p => p.ExpiryDate >= fromDate && p.ExpiryDate <= toDate)
            .ToListAsync(cancellationToken);
    }
}

public class SpeciesRepository : Repository<Species>, ISpeciesRepository
{
    public SpeciesRepository(IaraDbContext context) : base(context) { }

    public async Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.SpeciesName == name, cancellationToken);
    }
}

public class CatchQuotaRepository : Repository<CatchQuota>, ICatchQuotaRepository
{
    public CatchQuotaRepository(IaraDbContext context) : base(context) { }

    public async Task<IEnumerable<CatchQuota>> GetByPermitIdAsync(int permitId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(q => q.Species)
            .Where(q => q.PermitId == permitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CatchQuota>> GetBySpeciesIdAsync(int speciesId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(q => q.Permit)
            .Where(q => q.SpeciesId == speciesId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CatchQuota?> GetByPermitSpeciesYearAsync(int permitId, int speciesId, short year, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(
            q => q.PermitId == permitId && q.SpeciesId == speciesId && q.Year == year, 
            cancellationToken);
    }
}

public class LogbookRepository : Repository<Logbook>, ILogbookRepository
{
    public LogbookRepository(IaraDbContext context) : base(context) { }

    public async Task<IEnumerable<Logbook>> GetByVesselIdAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Vessel)
            .Include(l => l.Captain)
            .Include(l => l.Species)
            .Where(l => l.VesselId == vesselId)
            .OrderByDescending(l => l.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Logbook>> GetByCaptainIdAsync(int captainId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Vessel)
            .Include(l => l.Species)
            .Where(l => l.CaptainId == captainId)
            .OrderByDescending(l => l.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Logbook>> GetByDateRangeAsync(int vesselId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Vessel)
            .Include(l => l.Captain)
            .Include(l => l.Species)
            .Where(l => l.VesselId == vesselId && l.StartTime >= startDate && l.StartTime <= endDate)
            .OrderByDescending(l => l.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<Logbook?> GetWithDetailsAsync(int logEntryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Vessel)
            .Include(l => l.Captain)
            .Include(l => l.Species)
            .FirstOrDefaultAsync(l => l.LogEntryId == logEntryId, cancellationToken);
    }
}

public class InspectionRepository : Repository<Inspection>, IInspectionRepository
{
    public InspectionRepository(IaraDbContext context) : base(context) { }

    public async Task<IEnumerable<Inspection>> GetByVesselIdAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Vessel)
            .Include(i => i.Inspector)
            .Where(i => i.VesselId == vesselId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inspection>> GetByInspectorIdAsync(int inspectorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Vessel)
            .Where(i => i.InspectorId == inspectorId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inspection>> GetIllegalInspectionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Vessel)
            .Include(i => i.Inspector)
            .Where(i => !i.IsLegal)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Inspection?> GetWithDetailsAsync(int inspectionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Vessel)
            .Include(i => i.Inspector)
            .Include(i => i.Tickets)
            .FirstOrDefaultAsync(i => i.InspectionId == inspectionId, cancellationToken);
    }
}

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(IaraDbContext context) : base(context) { }

    public async Task<Ticket?> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber, cancellationToken);
    }

    public async Task<IEnumerable<Ticket>> GetByInspectionIdAsync(int inspectionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.InspectionId == inspectionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ticket>> GetUnvalidatedTicketsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => !t.IsValidated)
            .ToListAsync(cancellationToken);
    }
}
