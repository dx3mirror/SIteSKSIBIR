using WebApplication2;

namespace WebApplication1.Repository
{
    public interface IUserRepository
    {
        Task<UsersSite> FindUserAsync(string username, string password);
    }
}
