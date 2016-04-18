using Svn.Model;
using Svn.Service;
using System;
using System.Web.Http;

namespace Svn.WebApi.Controllers
{
    [RoutePrefix("api/ArticleApi")]
    public class ArticleApiController : ApiController
    {
        readonly IArticleService oArticleService;

        public ArticleApiController(IArticleService service)
        {
            oArticleService = service;
        }

        [Route("")]
        [HttpGet]
        public object Get()
        {
            return oArticleService.GetPublishedItems;
        }

        [Route("User/{userId}")]
        [HttpGets]
        public object GetByUserID(string userId)
        {
            return oArticleService.GetItemsByUserID(Guid.Parse(userId));
        }

        [Route("Article/{articleId}")]
        [HttpGet]
        public object GetByArticeID(string articleId)
        {
            return oArticleService.GetById(Guid.Parse(articleId));
        }

        [Route("Article/SaveAsDraft")]
        [HttpPost]
        public void SaveAsDraft(Article article)
        {
            oArticleService.SaveAsDraft(article);
        }

        [Route("Article/Submit")]
        [HttpPost]
        public void Submit(Article article)
        {
            oArticleService.Submit(article);
        }

        [Route("Article/Delete")]
        [HttpDelete]
        public void Delete(Article article)
        {
            oArticleService.Delete(article);
        }
    }
}
