using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly KadrovikContext _dbContext;

        public AppUnitOfWork(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(_dbContext);
        }

        public IUserRepository UserRepository { get; private set; }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
