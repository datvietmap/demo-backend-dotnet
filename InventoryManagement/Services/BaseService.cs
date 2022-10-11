using InventoryManagement.Entities;
using InventoryManagement.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;

namespace InventoryManagement.Services
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<TEntity> _entities;

        public BaseService(DataContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public async virtual Task Add(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async virtual Task AddRange(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public virtual int Count()
        {
            return _entities.Count();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _entities.Where(expression);
        }

        public async virtual Task<TEntity?> GetSingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return await _entities.SingleOrDefaultAsync(expression);
        }

        public async virtual Task<TEntity?> Get(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async virtual Task<IEnumerable<TEntity>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async virtual Task<bool> Delete(TEntity? entity)
        {
            bool result = false;

            if (entity != null)
            {
                var type = typeof(TEntity);
                if (!type.GetProperties().Any(x => x.Name == Constants.Columns.IS_DELETED))
                {
                    throw new InvalidOperationException($"{typeof(TEntity)} doesn't have any {Constants.Columns.IS_DELETED} property.");
                }

                type.GetProperty(Constants.Columns.IS_DELETED)?.SetValue(entity, true);

                Update(entity);
                result = await _context.SaveChangesAsync() > 0;
            }

            return result;
        }
    }
}
