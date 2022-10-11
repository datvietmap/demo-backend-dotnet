using System.Linq.Expressions;

namespace InventoryManagement.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        int Count();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetSingleOrDefault(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> Get(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<bool> Delete(TEntity entity);
    }
}
