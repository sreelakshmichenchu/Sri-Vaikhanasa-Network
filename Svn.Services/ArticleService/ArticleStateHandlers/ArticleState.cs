

using Svn.Model;
using Svn.Repository;
namespace Svn.Service.ArticleStateHandlers
{
    public abstract class ArticleState : IArticleState
    {
        public Article Article { get; private set; }

        public string StateName { get { return State.ToString(); } }

        public ArticleStateType State { get; private set; }

        protected IRepository<Article> Repository { get; private set; }

        protected ArticleState(Article article, ArticleStateType state, IRepository<Article> repository)
        {
            this.Article = article;
            this.State = state;
            Repository = repository;
        }

        public abstract void Draft();

        public abstract void Submit();

        public abstract void Approve();

        public abstract void Delete();
    }    
}
