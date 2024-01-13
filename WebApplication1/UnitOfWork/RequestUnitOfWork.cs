using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class RequestUnitOfWork : IRequestUnitOfWork
    {
        private readonly KadrovikContext _context;
        private IRequestSiteRepository _requestSiteRepository;
        public RequestUnitOfWork(KadrovikContext context)
        {
            _context = context;
        }
        public IRequestSiteRepository RequestSiteRepository
        {
            get
            {
                if (_requestSiteRepository == null)
                {
                    _requestSiteRepository = new RequestSiteRepository(_context);
                }
                return _requestSiteRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
