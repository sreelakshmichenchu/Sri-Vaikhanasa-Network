using Svn.Model;
using Svn.Repository;

namespace Svn.Service
{
    public class ErrorLogService : EntityService<ErrorLog>, IErrorLogService
    {
        public ErrorLogService(IUnitOfWork unitOfWork, IRepository<ErrorLog> repository) : base(unitOfWork, repository) { }
    }
}
