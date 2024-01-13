using Microsoft.EntityFrameworkCore;
using WebApplication2;

namespace WebApplication1.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KadrovikContext _dbContext;

        public UserRepository(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsersSite?>  FindUserAsync(string username, string password)
        {
            return await _dbContext.UsersSites?.FirstOrDefaultAsync(u => u.Login == username && u.Password == password);
        }
    }
}
