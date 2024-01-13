using Microsoft.EntityFrameworkCore;
using WebApplication1.Repository;
using WebApplication2;

namespace WebApplication1.UnitOfWork
{
    public class ViewOtpuskUnitofWork : IViewOtpuskUnitOfWork
    {
        private readonly KadrovikContext _context;
        public ViewOtpuskUnitofWork(KadrovikContext context, IOtpuskViewRepository<Otpusk> otpuskRepository)
        {
            _context = context;
            OtpuskRepository = otpuskRepository;
        }

        public IOtpuskViewRepository<Otpusk> OtpuskRepository { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
