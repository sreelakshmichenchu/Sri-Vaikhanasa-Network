using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Svn.Repository
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        private ObjectContext _objectContext;
        private DbTransaction _transaction;

        public UnitOfWork(DbContext context)
        {

            _dbContext = context;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.Unspecified);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public bool CommitTransaction()
        {
            _transaction.Commit();
            return true;
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
        }
    }
}
