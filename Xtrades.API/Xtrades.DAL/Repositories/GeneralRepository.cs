using Microsoft.EntityFrameworkCore;
using Xtrades.DAL.Context;
using Xtrades.DAL.Entities;

namespace Xtrades.DAL.Repositories
{
    public class GeneralRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GeneralRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(TEntity ientityd)
        {
                _dbSet.Remove(ientityd);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Func<TEntity, bool> predicate)
        {
            var entitiesToDelete = _dbSet.Where(predicate);
            if (entitiesToDelete != null && entitiesToDelete.Any())
            {
                _dbSet.RemoveRange(entitiesToDelete);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public async Task<TEntity> ReadAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }


}
