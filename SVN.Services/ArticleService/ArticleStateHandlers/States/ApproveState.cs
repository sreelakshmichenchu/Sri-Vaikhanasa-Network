using Svn.Model;
using Svn.Repository;
using System;
using System.Linq;

namespace Svn.Service.ArticleStateHandlers
{
    public class ApproveState : ArticleState
    {
        public ApproveState(Article article, IRepository<Article> repository) 
            : base(article, ArticleStateType.Approved, repository) { }

        public override void Draft()
        {
            CheckIfAlreadyDraftVersionAvailable();
            CheckIfAlreadySubmittedVersionAvailable();

            // Create a new draft record for the same article
            this.Article.Status = ArticleStateType.Draft.ToString();
            this.Article.InitialArticle = null;
            this.Article.Id = Guid.NewGuid();
            Repository.Insert(this.Article);
        }        

        public override void Submit()
        {
            CheckIfAlreadyDraftVersionAvailable();
            CheckIfAlreadySubmittedVersionAvailable();

            // Create a new submit record for the same article
            this.Article.Status = ArticleStateType.Submitted.ToString();
            this.Article.InitialArticle = null;
            this.Article.Id = Guid.NewGuid();
            Repository.Insert(this.Article);
        }

        public override void Approve()
        {
            throw new InvalidOperationException(Svn.Resources.Errors.ArticleFromApproved2Approved);
        }

        public override void Delete()
        {
            // Soft Delete the record
            this.Article.Status = ArticleStateType.Deleted.ToString();
            Repository.UpdateExcept(this.Article, Svn.Resources.Constants.InitialArticleId);
        }

        #region Helpers

        private void CheckIfAlreadyDraftVersionAvailable()
        {
            // Check for any existing Draft version of same Article. If any throw error
            var anyDraftVersionAvailable =
                Repository.FindBy(a1 => a1.Id != this.Article.Id 
                    && a1.InitialArticleId == this.Article.InitialArticleId
                    && a1.Status == ArticleStateType.Draft.ToString()).Any();

            if (anyDraftVersionAvailable)
                throw new InvalidOperationException(Svn.Resources.Errors.AlreadyDraftVersionOfSameArticleAvailable);
        }

        private void CheckIfAlreadySubmittedVersionAvailable()
        {
            // Check for any existing Submitted version of same Article. If any throw error
            var anyDraftVersionAvailable =
                Repository.FindBy(a1 => a1.Id != this.Article.Id
                    && a1.InitialArticleId == this.Article.InitialArticleId
                    && a1.Status == ArticleStateType.Submitted.ToString()).Any();

            if (anyDraftVersionAvailable)
                throw new InvalidOperationException(Svn.Resources.Errors.AlreadySubmittedVersionOfSameArticleAvailable);
        }

        #endregion
    }
}
