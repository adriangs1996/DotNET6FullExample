using System.Linq.Expressions;
using DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Implementations
{
    public class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId> where TEntity : class
    {
        private readonly TestContext _context;

        public Repository(TestContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TEntityId id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }
}