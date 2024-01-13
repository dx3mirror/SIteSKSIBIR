using WebApplication2;

namespace WebApplication1.Repository
{
    public class UpdateDoljnostRepository : IUpdateDoljnostRepository
    {
        private readonly KadrovikContext _context;

        public UpdateDoljnostRepository(KadrovikContext context)
        {
            _context = context;
        }

        public async Task<Doljnost?> GetByIdAsync(int? id)
        {
            return await _context.Doljnosts.FindAsync(id);
        }
    }
}
