
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
    }
}
