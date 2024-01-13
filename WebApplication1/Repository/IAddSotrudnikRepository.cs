using WebApplication2;

namespace WebApplication1.Repository
{
    public interface IAddSotrudnikRepository
    {
        Task AddSotrudnikAsync(Sotrudnik sotrudnik);
    }
}
