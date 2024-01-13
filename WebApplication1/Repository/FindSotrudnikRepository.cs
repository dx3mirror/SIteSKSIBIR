using Microsoft.EntityFrameworkCore;
using WebApplication2;

namespace WebApplication1.Repository
{
    public class FindSotrudnikRepository : IFindSotrudnikRepository
    {
        private readonly KadrovikContext _dbContext;

        public FindSotrudnikRepository(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Sotrudnik>> FindSotrudniksAsync()
        {
            return await _dbContext.Sotrudniks
                .Where(s => s.Del == "no")
                .OrderBy(s => s.Id)
                .ToListAsync();
        }
    }
}
