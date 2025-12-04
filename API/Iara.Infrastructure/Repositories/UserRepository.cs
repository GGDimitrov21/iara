using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iara.DomainModel.Entities;
using Iara.Infrastructure.Base;
using Iara.Persistence.Data;

namespace Iara.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IaraDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}