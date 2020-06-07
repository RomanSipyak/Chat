using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.AppContext.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().Remove(entity)).ConfigureAwait(false);
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().RemoveRange(entities)).ConfigureAwait(false);
        }

        public async Task<bool> EntityExistsAsync(int id)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(e => e.Id == id);
        }     
    }
}
