using Svn.Model;
using Svn.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service.ArticleStateHandlers
{
    public sealed class ArticleStateFactory
    {
        private ArticleStateFactory() { }

        internal static IArticleState GetState(ArticleStateType articleStateType, Article article, IRepository<Article> repository)
        {
            switch (articleStateType)
            {
                case ArticleStateType.Draft: return new DraftState(article, repository);
                case ArticleStateType.Submitted: return new SubmitState(article, repository);
                case ArticleStateType.Approved: return new ApproveState(article, repository);
                case ArticleStateType.Deleted: return new DeleteState(article, repository);
                default: throw new ApplicationException("Invalid Article State: " + articleStateType);
            }
        }
    }
}
