using Svn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service
{
    public interface IArticleService : IEntityService<Article>
    {
        IEnumerable<Article> GetPublishedItems { get; }
        IEnumerable<Article> GetItemsByUserID(Guid userId);
        void SaveAsDraft(Article article);
        void Submit(Article article);
        new void Delete(Article entity);
        void Approve(Article article);
        void ReferBack(Article article);

        Guid GetAuthorIDByArticleID(Guid articleID);
    }
}
