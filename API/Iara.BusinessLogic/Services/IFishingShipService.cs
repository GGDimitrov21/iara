using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;
using Iara.DomainModel.Filters;

namespace Iara.BusinessLogic.Services
{
    public interface IFishingShipService
    {
        Task<FishingShipResponseDTO> GetByIdAsync(int id);
        Task<IEnumerable<FishingShipResponseDTO>> GetAllAsync();
        Task<IEnumerable<FishingShipResponseDTO>> GetFilteredAsync(FishingShipFilter filter);
        Task<FishingShipResponseDTO> CreateAsync(CreateFishingShipRequestDTO request);
        Task<FishingShipResponseDTO> UpdateAsync(int id, UpdateFishingShipRequestDTO request);
        Task DeleteAsync(int id);
    }
}