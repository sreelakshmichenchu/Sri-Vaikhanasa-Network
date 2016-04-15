using Svn.Service;
using System;
using System.Web.Http;

namespace Svn.WebApi.Controllers
{
    public class ArticleApiController : ApiController
    {
        readonly IArticleService oArticleService;

        public ArticleApiController(IArticleService service)
        {
            oArticleService = service;
        }

        public object Get()
        {
            return oArticleService.GetPublishedItems;
        }

        public object Get(string userId)
        {
            return oArticleService.GetItemsByUserID(Guid.Parse(userId));
        }
    }
}
