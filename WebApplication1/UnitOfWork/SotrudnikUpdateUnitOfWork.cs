using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class SotrudnikUpdateUnitOfWork : ISotrudnikUpdateUnitOfWork
    {
        private readonly KadrovikContext _dbContext;

        public SotrudnikUpdateUnitOfWork(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
