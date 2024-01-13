using WebApplication1.Repository;

namespace WebApplication1.UnitOfWork
{
    public interface IRequestUnitOfWork : IDisposable
    {
        IRequestSiteRepository RequestSiteRepository { get; }
        void SaveChanges();
    }
}
