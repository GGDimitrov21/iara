using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;
using Iara.DomainModel.Filters;

namespace Iara.BusinessLogic.Services
{
    public interface IFishingLogEntryService
    {
        Task<FishingLogEntryResponseDTO> GetByIdAsync(long id);
        Task<IEnumerable<FishingLogEntryResponseDTO>> GetAllAsync();
        Task<IEnumerable<FishingLogEntryResponseDTO>> GetFilteredAsync(FishingLogEntryFilter filter);
        Task<FishingLogEntryResponseDTO> CreateAsync(CreateFishingLogEntryRequestDTO request);
        Task DeleteAsync(long id);
        Task<bool> SignLogEntryAsync(long id);
    }
}