using Svn.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Svn.Service
{
    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected IRepository<TEntity> Repository { get; private set; }

        public IQueryable<TEntity> GetAll { get { return Repository.GetAll; } }

        protected EntityService(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
        }

        public virtual void Create(TEntity entity)
        {
            UnitOfWork.BeginTransaction();
            if (entity == null)
                throw new ArgumentNullException(Svn.Resources.Errors.EntityCannotBeNull);

            Repository.Insert(entity);
            UnitOfWork.CommitTransaction();
        }


        public virtual void Update(TEntity entity)
        {
            UnitOfWork.BeginTransaction();
            if (entity == null)
                throw new ArgumentNullException(Svn.Resources.Errors.EntityCannotBeNull);

            Repository.Update(entity);            
            UnitOfWork.CommitTransaction();
        }

        public virtual void Delete(TEntity entity)
        {
            UnitOfWork.BeginTransaction();
            if (entity == null)
                throw new ArgumentNullException(Svn.Resources.Errors.EntityCannotBeNull);

            Repository.Delete(entity);
            UnitOfWork.CommitTransaction();
        }

        public virtual TEntity GetById(object id)
        {
            return Repository.GetById(id);
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.FindBy(predicate);
        }
    }
}
