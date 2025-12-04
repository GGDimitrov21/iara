using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;

namespace Iara.BusinessLogic.Services
{
    public interface IShipClassificationLogService
    {
        Task<ShipClassificationLogResponseDTO> GetByIdAsync(int id);
        Task<IEnumerable<ShipClassificationLogResponseDTO>> GetByShipIdAsync(int shipId);
        Task<ShipClassificationLogResponseDTO> CreateAsync(CreateShipClassificationLogRequestDTO request);
        Task DeleteAsync(int id);
    }
}