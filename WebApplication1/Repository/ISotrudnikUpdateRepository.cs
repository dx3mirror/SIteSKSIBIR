using WebApplication2;

namespace WebApplication1.Repository
{
    public interface ISotrudnikUpdateRepository
    {
        Task<Sotrudnik> FindSotrudnikByIdAsync(int id);
        void UpdateSotrudnik(Sotrudnik existingSotrudnik, Sotrudnik updatedSotrudnik);
    }
}
