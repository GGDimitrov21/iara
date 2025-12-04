using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.Entities;
using Iara.DomainModel.Filters;
using Iara.Infrastructure.Base;

namespace Iara.Infrastructure.Repositories
{
    public interface IFishingLogEntryRepository : IGenericRepository<FishingLogEntry>
    {
        Task<IEnumerable<FishingLogEntry>> GetFilteredAsync(FishingLogEntryFilter filter);
        Task<FishingLogEntry> GetByShipAndDateAsync(int shipId, DateTime logDate);
    }
}