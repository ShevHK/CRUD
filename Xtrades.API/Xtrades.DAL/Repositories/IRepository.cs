namespace Xtrades.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> ReadAsync(int id);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(Func<TEntity, bool> func);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity,bool> func);
        Task SaveChangesAsync();
    }
}
