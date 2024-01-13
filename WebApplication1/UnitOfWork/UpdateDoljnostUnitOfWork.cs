using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class UpdateDoljnostUnitOfWork : IUpdateDoljnostUnitOfWork
    {
        private readonly KadrovikContext _context;

        public IUpdateDoljnostRepository DoljnostRepository { get; }

        public UpdateDoljnostUnitOfWork(KadrovikContext context, IUpdateDoljnostRepository doljnostRepository)
        {
            _context = context;
            DoljnostRepository = doljnostRepository;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
