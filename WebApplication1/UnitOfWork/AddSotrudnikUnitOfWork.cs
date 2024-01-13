using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class AddSotrudnikUnitOfWork : IAddSotrudnikUnitOfWork
    {
        private readonly KadrovikContext _dbContext;

        public AddSotrudnikUnitOfWork(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
