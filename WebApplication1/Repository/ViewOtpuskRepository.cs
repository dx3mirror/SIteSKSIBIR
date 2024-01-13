using Microsoft.EntityFrameworkCore;
using WebApplication2;

namespace WebApplication1.Repository
{
    public class ViewOtpuskRepository<TEntity> : IOtpuskViewRepository<TEntity> where TEntity : class
    {
        private readonly KadrovikContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public ViewOtpuskRepository(KadrovikContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
