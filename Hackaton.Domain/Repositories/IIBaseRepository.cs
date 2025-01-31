using System.Linq.Expressions;

namespace Hackaton.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindByIdAsync(long id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> AllAsync();
        Task<int> CountAllAsync();
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(long id);
        Task InsertAsync(TEntity entity);
        Task InsertManyAsync(TEntity[] entities);
        Task<bool> CommitAsync();
        Task UpdateAsync(TEntity entity);
        Task UpdateManyAsync(TEntity[] entities);
        IQueryable<TEntity> GetQueryable();
    }
}
