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
    public class InspectionRepository : GenericRepository<Inspection>, IInspectionRepository
    {
        public InspectionRepository(IaraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Inspection>> GetFilteredAsync(InspectionFilter filter)
        {
            var query = _dbSet.AsQueryable();

            if (filter.ShipId.HasValue)
                query = query.Where(i => i.ShipId == filter.ShipId.Value);

            if (filter.InspectorId.HasValue)
                query = query.Where(i => i.InspectorId == filter.InspectorId.Value);

            if (filter.HasViolation.HasValue)
                query = query.Where(i => i.HasViolation == filter.HasViolation.Value);

            if (filter.IsProcessed.HasValue)
                query = query.Where(i => i.IsProcessed == filter.IsProcessed.Value);

            if (filter.InspectionDateFrom.HasValue)
                query = query.Where(i => i.InspectionDate >= filter.InspectionDateFrom.Value);

            if (filter.InspectionDateTo.HasValue)
                query = query.Where(i => i.InspectionDate <= filter.InspectionDateTo.Value);

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                query = query.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                            .Take(filter.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Inspection> GetByProtocolNumberAsync(string protocolNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.ProtocolNumber == protocolNumber);
        }
    }
}