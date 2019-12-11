using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmearAdmin.Data;

namespace SmearAdmin.Persistence
{
    public class Repositories<TEntity> : IRepositories<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repositories(DbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity).State = EntityState.Modified;
            _entities.Update(entity);
        }
        public virtual void Update(TEntity entity, TEntity entityUpdate)
        {
            _context.Entry(entity).CurrentValues.SetValues(entityUpdate);
            _entities.Update(entity).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Unchanged;
            foreach (var prop in properties)
            {
                entry.Property(prop).IsModified = true;
            }
        }

        //public virtual void UpdateRange(IEnumerable<TEntity> entities, IEnumerable<TEntity> entitiesUpdate)
        //{
        //    _context.Entry(entities).CurrentValues.SetValues(entitiesUpdate);
        //    _entities.UpdateRange(entities);
        //    //_entities.UpdateRange(entities).State = EntityState.Modified;
        //}

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


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public virtual TEntity Get(int id)
        {
            return _entities.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }
    }
}
