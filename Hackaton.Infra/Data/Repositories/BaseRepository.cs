using Hackaton.Domain.Entities.BaseEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hackaton.Infra.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected DbSet<TEntity> set;
        protected readonly FiapDbContext _context;

        protected BaseRepository(FiapDbContext context)
        {
            _context = context;
            set = _context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await set.ToListAsync();
        }

        public virtual async Task<int> CountAllAsync()
        {
            return await set.CountAsync();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return set.AnyAsync(predicate);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            set.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await set.FindAsync(id);
            if (entity != null)
            {
                set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<TEntity> FindByIdAsync(long id)
        {
            return await set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<bool> CommitAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task InsertManyAsync(TEntity[] entities)
        {
            await set.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            set.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateManyAsync(TEntity[] entities)
        {
            set.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async virtual Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await set.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await set.FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return set.AsQueryable();
        }
    }
}
