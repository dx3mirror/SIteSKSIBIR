using WebApplication2;

namespace WebApplication1.Repository
{
    public class SotrudnikUpdateRepository : ISotrudnikUpdateRepository
    {
        private readonly KadrovikContext _dbContext;

        public SotrudnikUpdateRepository(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sotrudnik> FindSotrudnikByIdAsync(int id)
        {
            return await _dbContext.Sotrudniks.FindAsync(id);
        }

        public void UpdateSotrudnik(Sotrudnik existingSotrudnik, Sotrudnik updatedSotrudnik)
        {
            _dbContext.Entry(existingSotrudnik).CurrentValues.SetValues(updatedSotrudnik);
        }
    }
}
