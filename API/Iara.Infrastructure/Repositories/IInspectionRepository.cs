using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.Entities;
using Iara.DomainModel.Filters;
using Iara.Infrastructure.Base;

namespace Iara.Infrastructure.Repositories
{
    public interface IInspectionRepository : IGenericRepository<Inspection>
    {
        Task<IEnumerable<Inspection>> GetFilteredAsync(InspectionFilter filter);
        Task<Inspection> GetByProtocolNumberAsync(string protocolNumber);
    }
}