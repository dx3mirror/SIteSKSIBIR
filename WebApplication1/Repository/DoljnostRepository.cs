using Microsoft.EntityFrameworkCore;
using WebApplication2;

namespace WebApplication1.Repository
{
    public class DoljnostRepository : IDoljnostRepository
    {
        private readonly KadrovikContext _context;

        public DoljnostRepository(KadrovikContext context)
        {
            _context = context;
        }

        public async Task<List<Doljnost>> GetDoljnostiForSotrudnikAsync(int sotrudnikId)
        {
            return await _context.Doljnosts
                .Where(d => d.FkSotrudnik == sotrudnikId)
                .ToListAsync();
        }
    }
}
