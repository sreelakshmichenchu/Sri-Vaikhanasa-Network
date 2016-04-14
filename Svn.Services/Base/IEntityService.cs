using System;
using System.Linq;
using System.Linq.Expressions;

namespace Svn.Service
{
    public interface IEntityService<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll { get; }
        void Create(TEntity entity);
        void Delete(TEntity entity);        
        TEntity GetById(object id);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);
    }
}
