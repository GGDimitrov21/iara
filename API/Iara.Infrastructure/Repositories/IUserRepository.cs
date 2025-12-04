using System.Threading.Tasks;
using Iara.DomainModel.Entities;
using Iara.Infrastructure.Base;

namespace Iara.Infrastructure.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
    }
}