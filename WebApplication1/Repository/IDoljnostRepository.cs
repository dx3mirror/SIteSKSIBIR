using WebApplication2;

namespace WebApplication1.Repository
{
    public interface IDoljnostRepository
    {
        Task<List<Doljnost>> GetDoljnostiForSotrudnikAsync(int sotrudnikId);
    }
}
