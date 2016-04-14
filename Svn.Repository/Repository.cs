
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Svn.Model;

namespace Svn.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext DbContext { get; private set; }
        protected IDbSet<TEntity> DbSet { get; private set; }

        public IQueryable<TEntity> GetAll
        {
            get
            {
                return DbSet.AsQueryable<TEntity>();
            }
        }

        public Repository(DbContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual TEntity GetById(object id)
        {
            return GetById(id, true);
        }

        public virtual TEntity GetById(object id, bool isTrackable)
        {
            var entity = DbSet.Find(id);
            if (!isTrackable)
                DbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {

            return DbSet.Where(predicate).AsQueryable();
        }

        public virtual void Insert(TEntity entity)
        {
            var objEntity = entity as IEntity;
            if (objEntity != null)
            {
                objEntity.Id = Guid.NewGuid();
            }

            DbSet.Add(entity);
            DbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity, params string[] properties)
        {
            DbContext.Update(entity, properties);
            DbContext.SaveChanges();
        }

        public virtual void UpdateExcept(TEntity entity, params string[] properties)
        {
            DbContext.UpdateExcept(entity, properties);
            DbContext.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        public void Refresh(TEntity entity)
        {
            DbContext.Entry(entity).Reload();
        }
    }
}
