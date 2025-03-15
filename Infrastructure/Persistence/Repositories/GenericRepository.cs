
global using Domain.Contracts;
global using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(SafetyDbContext context)
        : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity)
            => await context.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
            => context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
            => context.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = true)
            => trackChanges ? await context.Set<TEntity>().ToListAsync()
                            : await context.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetAsync(TKey id)
            => await context.Set<TEntity>().FindAsync(id);

    }
}
