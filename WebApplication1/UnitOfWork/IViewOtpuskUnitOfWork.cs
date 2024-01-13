using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public interface IViewOtpuskUnitOfWork : IDisposable
    {
        IOtpuskViewRepository<Otpusk> OtpuskRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
