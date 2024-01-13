using WebApplication2;

namespace WebApplication1.Repository
{
    public class AddDoljnostRepository : IAddDoljstostRepository
    {
        private readonly KadrovikContext _context;

        public AddDoljnostRepository(KadrovikContext context)
        {
            _context = context;
        }

        public void Add(Doljnost doljnost)
        {
            _context.Doljnosts.Add(doljnost);
        }
    }
}
