using Svn.Model;
using Svn.Repository;
using Svn.Service.ArticleStateHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service
{
    public class ArticleService : EntityService<Article>, IArticleService
    {
        public ArticleService(IUnitOfWork unitOfWork, IRepository<Article> repository) : base(unitOfWork, repository) { }

        public IEnumerable<Article> GetPublishedItems
        {
            get
            {
                return Repository.FindBy(w1 => w1.Status == ArticleStateType.Approved.ToString())
                    .GroupBy(a1 => a1.InitialArticleId)
                    .SelectMany(s1 => s1.OrderByDescending(o1 => o1.CreatedAt).ThenByDescending(o2 => o2.ModifiedAt).Take(1)).ToList();
            }
        }

        public IEnumerable<Article> GetMyItems(Guid myUserId)
        {
            var items = Repository.FindBy(w1 => w1.CreatedBy == myUserId)
                .GroupBy(a1 => a1.InitialArticleId)
                .SelectMany(s1 => s1.OrderByDescending(o1 => o1.CreatedAt).ThenByDescending(o2 => o2.ModifiedAt).Take(1)).ToList();

            return items;
        }

        public void SaveAsDraft(Article article)
        {
            UnitOfWork.BeginTransaction();

            if (IsArticleDoesNotExist(article))
            {
                CreateArticleForTheFirstTime(article, ArticleStateType.Draft);
            }
            else
            {
                var oldArticle = Repository.GetById(article.Id, false);
                article.InitialArticleId = oldArticle.InitialArticleId;
                IArticleState state = ArticleStateFactory.GetState(oldArticle.Status.ToEnum<ArticleStateType>(), article, (IRepository<Article>)Repository);
                state.Draft();
            }

            UnitOfWork.CommitTransaction();
        }

        

        public void Submit(Model.Article article)
        {
            UnitOfWork.BeginTransaction();

            if (IsArticleDoesNotExist(article))
            {
                CreateArticleForTheFirstTime(article, ArticleStateType.Submitted);
            }
            else
            {
                var oldArticle = Repository.GetById(article.Id, false);
                article.InitialArticleId = oldArticle.InitialArticleId;
                IArticleState state = ArticleStateFactory.GetState(oldArticle.Status.ToEnum<ArticleStateType>(), article, (IRepository<Article>)Repository);
                state.Submit();
            }

            UnitOfWork.CommitTransaction();
        }

        public void ReferBack(Model.Article article)
        {
            IArticleState state = ArticleStateFactory.GetState(article.Status.ToEnum<ArticleStateType>(), article, (IRepository<Article>)Repository);
            state.Draft();
        }

        public void Approve(Model.Article article)
        {
            IArticleState state = ArticleStateFactory.GetState(article.Status.ToEnum<ArticleStateType>(), article, (IRepository<Article>)Repository);
            state.Approve();
        }

        public override void Delete(Model.Article entity)
        {
            IArticleState state = ArticleStateFactory.GetState(entity.Status.ToEnum<ArticleStateType>(), entity, (IRepository<Article>)Repository);
            state.Delete();
        }

        public Guid GetAuthorIDByArticleID(Guid articleID)
        {
            var item = Repository.GetById(articleID);
            if (item == null)
                throw new NullReferenceException("Article not found with given ID");

            return item.CreatedBy;
        }

        #region Helpers

        private void CreateArticleForTheFirstTime(Article article, ArticleStateType stateType)
        {
            article.Status = stateType.ToString();
            Repository.Insert(article);
            Repository.Refresh(article);
            article.InitialArticleId = article.Id;
            Repository.Update(article, Svn.Resources.Constants.InitialArticleId);
        }

        private static bool IsArticleDoesNotExist(TrackableEntity article)
        {
            return article.Id == Guid.Empty;
        }

        #endregion
    }
}
