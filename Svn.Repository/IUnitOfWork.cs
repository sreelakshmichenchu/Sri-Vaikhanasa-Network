
using System;
using System.Data;
using System.Threading.Tasks;

namespace Svn.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        bool CommitTransaction();
        void RollbackTransaction();
    }
}
