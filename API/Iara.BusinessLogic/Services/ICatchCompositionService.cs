using System.Collections.Generic;
using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;

namespace Iara.BusinessLogic.Services
{
    public interface ICatchCompositionService
    {
        Task<CatchCompositionResponseDTO> GetByIdAsync(long id);
        Task<IEnumerable<CatchCompositionResponseDTO>> GetByLogEntryIdAsync(long logEntryId);
        Task<CatchCompositionResponseDTO> CreateAsync(CreateCatchCompositionRequestDTO request);
        Task DeleteAsync(long id);
    }
}