using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;
using Iara.DomainModel.Filters;

namespace Iara.BusinessLogic.Services
{
    public interface IInspectionService
    {
        Task<InspectionResponseDTO> GetByIdAsync(int id);
        Task<IEnumerable<InspectionResponseDTO>> GetAllAsync();
        Task<IEnumerable<InspectionResponseDTO>> GetFilteredAsync(InspectionFilter filter);
        Task<InspectionResponseDTO> CreateAsync(CreateInspectionRequestDTO request);
        Task DeleteAsync(int id);
        Task<bool> ProcessInspectionAsync(int id);
    }
}