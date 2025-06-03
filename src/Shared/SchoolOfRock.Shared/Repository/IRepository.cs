using Microsoft.EntityFrameworkCore;

namespace SchoolOfRock.Shared.Repository
{
    public interface IRepository<TContext, TEntity> 
        where TContext : DbContext
        where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity?> ReadAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(object id);

        DbSet<TEntity> Set { get; }
        TContext Context { get; }
    }
}