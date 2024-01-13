using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class DoljnostUnitOfWork : IDoljnostUnitOfWork
    {
        private readonly KadrovikContext _context;

        public DoljnostUnitOfWork(KadrovikContext context)
        {
            _context = context;
            DoljnostRepository = new DoljnostRepository(_context);
        }

        public IDoljnostRepository DoljnostRepository { get; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
