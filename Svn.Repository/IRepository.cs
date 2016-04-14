
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Svn.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll { get; }
        TEntity GetById(object id);
        TEntity GetById(object id, bool isTrackable);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Update(TEntity entity, params string[] properties);
        void UpdateExcept(TEntity entity, params string[] properties);
        void Delete(object id);
        void Delete(TEntity entity);
        void Refresh(TEntity entity);
    }
}
