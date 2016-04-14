using SVN.Model;
using SVN.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVN.Service.ArticleStateHandlers
{
    public class RejectState : ArticleState
    {
        public RejectState(Article article, IArticleRepository repository) 
            : base(article, ArticleStateType.Rejected, repository) { }

        public override void Draft()
        {
            throw new InvalidOperationException("Editing is not allowed on this article as it is rejected by admin");
        }

        public override void Submit()
        {
            throw new InvalidOperationException("Editing is not allowed on this article as it is rejected by admin");
        }

        public override void Approve()
        {
            throw new InvalidOperationException("This article cannot be approved as it is rejected by admin");
        }

        public override void Reject()
        {
            throw new InvalidOperationException("This article is already in rejected status");
        }

        public override void Delete()
        {
            // Hard Delete the record
            _repository.Delete(this.Article);
        }
    }
}
