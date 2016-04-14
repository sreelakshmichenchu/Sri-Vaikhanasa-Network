using Svn.Model;
using System;

namespace Svn.Service.ArticleStateHandlers
{
    public interface IArticleState
    {
        Article Article { get; }
        string StateName { get; }
        ArticleStateType State { get; }

        void Draft();
        void Submit();
        void Approve();
        void Delete();
    }
}
