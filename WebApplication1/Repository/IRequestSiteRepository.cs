using WebApplication2;

namespace WebApplication1.Repository
{
    public interface IRequestSiteRepository
    {
        void Add(RequestSite requestSite);
        IEnumerable<RequestSite> GetAll();
    }
}
