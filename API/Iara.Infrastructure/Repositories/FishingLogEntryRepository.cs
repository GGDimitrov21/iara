using System;
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
    public class FishingLogEntryRepository : GenericRepository<FishingLogEntry>, IFishingLogEntryRepository
    {
        public FishingLogEntryRepository(IaraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FishingLogEntry>> GetFilteredAsync(FishingLogEntryFilter filter)
        {
            var query = _dbSet.AsQueryable();

            if (filter.ShipId.HasValue)
                query = query.Where(l => l.ShipId == filter.ShipId.Value);

            if (filter.LogDateFrom.HasValue)
                query = query.Where(l => l.LogDate >= filter.LogDateFrom.Value);

            if (filter.LogDateTo.HasValue)
                query = query.Where(l => l.LogDate <= filter.LogDateTo.Value);

            if (!string.IsNullOrEmpty(filter.FishingZone))
                query = query.Where(l => l.FishingZone.Contains(filter.FishingZone));

            if (filter.IsSigned.HasValue)
                query = query.Where(l => l.IsSigned == filter.IsSigned.Value);

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                query = query.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                            .Take(filter.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<FishingLogEntry> GetByShipAndDateAsync(int shipId, DateTime logDate)
        {
            return await _dbSet.FirstOrDefaultAsync(l => l.ShipId == shipId && l.LogDate.Date == logDate.Date);
        }
    }
}