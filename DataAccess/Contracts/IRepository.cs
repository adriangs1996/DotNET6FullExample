using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IRepository<TEntity, TEntityId> where TEntity: class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetByIdAsync(TEntityId id);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
    }
}