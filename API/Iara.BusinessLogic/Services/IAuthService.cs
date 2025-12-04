using System.Threading.Tasks;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;

namespace Iara.BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<string> GenerateJwtTokenAsync(UserResponseDTO user);
    }
}