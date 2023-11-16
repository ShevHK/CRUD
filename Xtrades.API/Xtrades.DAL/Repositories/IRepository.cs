namespace Xtrades.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity entity);

        Task<TEntity> ReadAsync(int id);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity,bool> func);
        Task SaveChangesAsync();
    }
}
