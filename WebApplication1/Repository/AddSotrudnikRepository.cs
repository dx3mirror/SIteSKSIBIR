using WebApplication2;

namespace WebApplication1.Repository
{
    public class SotrudnikRepository : IAddSotrudnikRepository
    {
        private readonly KadrovikContext _dbContext;

        public SotrudnikRepository(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSotrudnikAsync(Sotrudnik sotrudnik)
        {
            _dbContext.Sotrudniks?.AddAsync(sotrudnik);
        }
    }
}
