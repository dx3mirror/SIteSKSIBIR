using WebApplication1.Repository;

namespace WebApplication1.UnitOfWork
{
    public interface IFindSotrudnikUnitOfWork
    {
        IFindSotrudnikRepository SotrudnikRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
