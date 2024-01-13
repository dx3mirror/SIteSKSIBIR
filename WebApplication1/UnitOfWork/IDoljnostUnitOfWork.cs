using WebApplication1.Repository;

namespace WebApplication1.UnitOfWork
{
    public interface IDoljnostUnitOfWork : IDisposable
    {
        IDoljnostRepository DoljnostRepository { get; }
        Task SaveChangesAsync();
    }
}
