namespace WebApplication1.UnitOfWork
{
    public interface IAppUnitOfWork : IDisposable
    {
        Repository.IUserRepository UserRepository { get; }
        void SaveChanges();
    }
}
