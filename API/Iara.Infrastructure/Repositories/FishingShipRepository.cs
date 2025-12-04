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
    public class FishingShipRepository : GenericRepository<FishingShip>, IFishingShipRepository
    {
        public FishingShipRepository(IaraDbContext context) : base(context)
        {
        }

        public async Task<FishingShip> GetByIaraIdNumberAsync(string iaraIdNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.IaraIdNumber == iaraIdNumber);
        }

        public async Task<FishingShip> GetByMaritimeNumberAsync(string maritimeNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.MaritimeNumber == maritimeNumber);
        }

        public async Task<IEnumerable<FishingShip>> GetFilteredAsync(FishingShipFilter filter)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(filter.IaraIdNumber))
                query = query.Where(s => s.IaraIdNumber.Contains(filter.IaraIdNumber));

            if (!string.IsNullOrEmpty(filter.MaritimeNumber))
                query = query.Where(s => s.MaritimeNumber.Contains(filter.MaritimeNumber));

            if (!string.IsNullOrEmpty(filter.ShipName))
                query = query.Where(s => s.ShipName.Contains(filter.ShipName));

            if (!string.IsNullOrEmpty(filter.OwnerName))
                query = query.Where(s => s.OwnerName.Contains(filter.OwnerName));

            if (filter.IsActive.HasValue)
                query = query.Where(s => s.IsActive == filter.IsActive.Value);

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                query = query.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                            .Take(filter.PageSize.Value);
            }

            return await query.ToListAsync();
        }
    }
}