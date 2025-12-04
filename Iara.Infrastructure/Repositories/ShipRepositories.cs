using Microsoft.EntityFrameworkCore;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Iara.Infrastructure.Persistence;

namespace Iara.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }
}

public class FishingShipRepository : Repository<FishingShip>, IFishingShipRepository
{
    public FishingShipRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<FishingShip?> GetByIaraIdAsync(string iaraId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.RegistrationDocument)
            .FirstOrDefaultAsync(s => s.IaraIdNumber == iaraId, cancellationToken);
    }

    public async Task<FishingShip?> GetByMaritimeNumberAsync(string maritimeNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.RegistrationDocument)
            .FirstOrDefaultAsync(s => s.MaritimeNumber == maritimeNumber, cancellationToken);
    }

    public async Task<IEnumerable<FishingShip>> GetActiveShipsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .Include(s => s.RegistrationDocument)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FishingShip>> GetShipsByOwnerAsync(string ownerName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.OwnerName.Contains(ownerName))
            .Include(s => s.RegistrationDocument)
            .ToListAsync(cancellationToken);
    }
}

public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
{
    public RegistrationRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Registration>> GetValidRegistrationsAsync(DateOnly asOfDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.ValidFrom <= asOfDate && r.ValidUntil >= asOfDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Registration>> GetExpiringRegistrationsAsync(DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.ValidUntil >= fromDate && r.ValidUntil <= toDate)
            .ToListAsync(cancellationToken);
    }
}

public class FishingPermitRepository : Repository<FishingPermit>, IFishingPermitRepository
{
    public FishingPermitRepository(IaraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FishingPermit>> GetPermitsByShipIdAsync(int shipId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.ShipId == shipId)
            .Include(p => p.Ship)
            .Include(p => p.RegistrationDocument)
            .ToListAsync(cancellationToken);
    }

    public async Task<FishingPermit?> GetActivePermitForShipAsync(int shipId, int year, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Ship)
            .Include(p => p.RegistrationDocument)
            .FirstOrDefaultAsync(p => p.ShipId == shipId && 
                                     p.PermitYear == year && 
                                     p.Status == PermitStatus.Active, 
                                cancellationToken);
    }

    public async Task<IEnumerable<FishingPermit>> GetPermitsByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.PermitYear == year)
            .Include(p => p.Ship)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FishingPermit>> GetExpiringPermitsAsync(DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.ValidUntil >= fromDate && p.ValidUntil <= toDate)
            .Include(p => p.Ship)
            .ToListAsync(cancellationToken);
    }
}
