using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmearAdmin.Persistence
{
    public interface IRepositories<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void Update(TEntity entity, TEntity entityUpdate);
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        //void UpdateRange(IEnumerable<TEntity> entities, IEnumerable<TEntity> entitiesUpdate);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int Count();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
    }
}
