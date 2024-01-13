using Microsoft.EntityFrameworkCore.Storage;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class AddDoljnostUnitOfWork : IAddDoljnostUnitOfWork
    {
        private readonly KadrovikContext _context;

        public AddDoljnostUnitOfWork(KadrovikContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
