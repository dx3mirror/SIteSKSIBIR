using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class FindSotrudnikUnitOfWork : IFindSotrudnikUnitOfWork
    {
        private readonly KadrovikContext _dbContext;

        public FindSotrudnikUnitOfWork(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
            SotrudnikRepository = new FindSotrudnikRepository(_dbContext);
        }

        public IFindSotrudnikRepository SotrudnikRepository { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
