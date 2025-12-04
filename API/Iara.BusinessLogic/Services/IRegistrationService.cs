using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;

namespace Iara.BusinessLogic.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationResponseDTO> GetByIdAsync(int id);
        Task<IEnumerable<RegistrationResponseDTO>> GetAllAsync();
        Task<RegistrationResponseDTO> CreateAsync(CreateRegistrationRequestDTO request);
        Task DeleteAsync(int id);
    }
}