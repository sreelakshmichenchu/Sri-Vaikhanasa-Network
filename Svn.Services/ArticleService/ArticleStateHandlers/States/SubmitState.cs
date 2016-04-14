using Svn.Model;
using Svn.Repository;
using System;

namespace Svn.Service.ArticleStateHandlers
{
    public class SubmitState : ArticleState
    {
        public SubmitState(Article article, IRepository<Article> repository) 
            : base(article, ArticleStateType.Submitted, repository) { }

        public override void Draft()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromSubmitted2Draft);
        }

        public override void Submit()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromSubmitted2Submitted);
        }

        public override void Approve()
        {
            // TODO: Override the same record and trigger email about approval
            this.Article.Status = ArticleStateType.Approved.ToString();
            Repository.UpdateExcept(this.Article, Svn.Resources.Constants.InitialArticleId);
        }

        public override void Delete()
        {
            // Soft Delete the record
            this.Article.Status = ArticleStateType.Deleted.ToString();
            Repository.UpdateExcept(this.Article, Svn.Resources.Constants.InitialArticleId);
        }
    }
}
