using WebApplication2;

namespace WebApplication1.Repository
{
    public interface IUpdateDoljnostRepository
    {
        Task<Doljnost> GetByIdAsync(int? id);
    }
}
