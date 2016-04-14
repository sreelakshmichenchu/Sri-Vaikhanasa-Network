using Svn.Model;
using Svn.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service.ArticleStateHandlers
{
    public class DeleteState : ArticleState
    {
        public DeleteState(Article article, IRepository<Article> repository) 
            : base(article, ArticleStateType.Deleted, repository) { }

        public override void Draft()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromDeleted2Draft);
        }

        public override void Submit()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromDeleted2Submitted);
        }

        public override void Approve()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromDeleted2Approved);
        }

        public override void Delete()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromDeleted2Deleted);
        }
    }
}
