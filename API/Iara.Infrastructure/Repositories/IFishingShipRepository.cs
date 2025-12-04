using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.Entities;
using Iara.DomainModel.Filters;
using Iara.Infrastructure.Base;

namespace Iara.Infrastructure.Repositories
{
    public interface IFishingShipRepository : IGenericRepository<FishingShip>
    {
        Task<FishingShip> GetByIaraIdNumberAsync(string iaraIdNumber);
        Task<FishingShip> GetByMaritimeNumberAsync(string maritimeNumber);
        Task<IEnumerable<FishingShip>> GetFilteredAsync(FishingShipFilter filter);
    }
}