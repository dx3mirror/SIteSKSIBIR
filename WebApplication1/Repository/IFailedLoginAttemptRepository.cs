using Microsoft.Extensions.Caching.Memory;
namespace WebApplication1.Repository
{
    public interface IFailedLoginAttemptRepository
    {
        int GetFailedLoginAttempts(string username);
        void IncrementFailedLoginAttempts(string username);
        void ResetFailedLoginAttempts(string username);
        bool IsAccountLocked(string username);
    }
}
