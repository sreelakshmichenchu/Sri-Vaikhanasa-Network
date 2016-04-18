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
        [HttpGet]
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

        [Route("Save")]
        [HttpPost]
        public void SaveAsDraft(Article article)
        {
            oArticleService.SaveAsDraft(article);
        }

        [Route("Submit")]
        [HttpPost]
        public void Submit(Article article)
        {
            oArticleService.Submit(article);
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Article article)
        {
            oArticleService.Delete(article);
        }

        [Route("Approve")]
        [HttpPost]
        public void Approve(Article article)
        {
            oArticleService.Approve(article);
        }

        [Route("ReferBack")]
        [HttpPost]
        public void ReferBack(Article article)
        {
            oArticleService.ReferBack(article);
        }
    }
}
