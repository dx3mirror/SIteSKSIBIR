using WebApplication1.Repository;

namespace WebApplication1.UnitOfWork
{
    public interface IUpdateDoljnostUnitOfWork
    {
        IUpdateDoljnostRepository DoljnostRepository { get; }
        Task SaveAsync();
    }
}
