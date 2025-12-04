using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.Entities;
using Iara.DomainModel.Filters;
using Iara.Infrastructure.Base;

namespace Iara.Infrastructure.Repositories
{
    public interface IFishingPermitRepository : IGenericRepository<FishingPermit>
    {
        Task<IEnumerable<FishingPermit>> GetFilteredAsync(FishingPermitFilter filter);
        Task<IEnumerable<FishingPermit>> GetByShipIdAsync(int shipId);
    }
}