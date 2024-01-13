using WebApplication2;

namespace WebApplication1.Repository
{
    public class RequestSiteRepository : IRequestSiteRepository
    {
        private readonly KadrovikContext _context;

        public RequestSiteRepository(KadrovikContext context)
        {
            _context = context;
        }

        public void Add(RequestSite requestSite)
        {
            _context.RequestSites.Add(requestSite);
        }

        public IEnumerable<RequestSite> GetAll()
        {
            return _context.RequestSites.ToList();
        }

    }
}
