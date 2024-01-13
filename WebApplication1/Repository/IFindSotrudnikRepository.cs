using WebApplication2;

namespace WebApplication1.Repository
{
    public interface IFindSotrudnikRepository
    {
        Task<IEnumerable<Sotrudnik>> FindSotrudniksAsync();
    }
}
