using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iara.DomainModel.Entities;
using Iara.DomainModel.Filters;
using Iara.Infrastructure.Base;
using Iara.Persistence.Data;

namespace Iara.Infrastructure.Repositories
{
    public class FishingPermitRepository : GenericRepository<FishingPermit>, IFishingPermitRepository
    {
        public FishingPermitRepository(IaraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FishingPermit>> GetFilteredAsync(FishingPermitFilter filter)
        {
            var query = _dbSet.AsQueryable();

            if (filter.ShipId.HasValue)
                query = query.Where(p => p.ShipId == filter.ShipId.Value);

            if (filter.PermitYear.HasValue)
                query = query.Where(p => p.PermitYear == filter.PermitYear.Value);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(p => p.Status == filter.Status);

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                query = query.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                            .Take(filter.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<FishingPermit>> GetByShipIdAsync(int shipId)
        {
            return await _dbSet.Where(p => p.ShipId == shipId).ToListAsync();
        }
    }
}