using Svn.Model;
using Svn.Repository;
using System;

namespace Svn.Service.ArticleStateHandlers
{
    public class DraftState : ArticleState
    {
        public DraftState(Article article, IRepository<Article> repository) 
            : base(article, ArticleStateType.Draft, repository) { }

        public override void Draft()
        {
            this.Article.Status = ArticleStateType.Draft.ToString();
            Repository.UpdateExcept(this.Article, Svn.Resources.Constants.InitialArticleId);
        }

        public override void Submit()
        {
            this.Article.Status = ArticleStateType.Submitted.ToString();
            Repository.UpdateExcept(this.Article, Svn.Resources.Constants.InitialArticleId);
        }

        public override void Approve()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromDraft2Approved);
        }

        public override void Delete()
        {
            //Hard Delete the record
            Repository.Delete(this.Article);
        }
    }
}
