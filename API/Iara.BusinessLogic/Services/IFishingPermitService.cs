using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;
using Iara.DomainModel.Filters;

namespace Iara.BusinessLogic.Services
{
    public interface IFishingPermitService
    {
        Task<FishingPermitResponseDTO> GetByIdAsync(int id);
        Task<IEnumerable<FishingPermitResponseDTO>> GetAllAsync();
        Task<IEnumerable<FishingPermitResponseDTO>> GetFilteredAsync(FishingPermitFilter filter);
        Task<FishingPermitResponseDTO> CreateAsync(CreateFishingPermitRequestDTO request);
        Task DeleteAsync(int id);
    }
}