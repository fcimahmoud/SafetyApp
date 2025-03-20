
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity
        : BaseEntity<TKey>
    {
        public Task<TEntity?> GetAsync(TKey id);
        public Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = true);
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);

        public Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> condition);
        public Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> condition);

        public Task<TEntity?> GetWithIncludesAsync(Expression<Func<TEntity, bool>> condition, params Expression<Func<TEntity, object>>[] includes);
        public Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(Expression<Func<TEntity, bool>> condition, params Expression<Func<TEntity, object>>[] includes);
    }
}
