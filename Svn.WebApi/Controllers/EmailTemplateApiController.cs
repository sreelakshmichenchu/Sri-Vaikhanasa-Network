using System.Linq;
using Svn.Service;
using System.Web.Http;

namespace Svn.WebApi.Controllers
{
    public class EmailTemplateApiController : ApiController
    {
        readonly IEmailService oEmailService;

        public EmailTemplateApiController(IEmailService service)
        {
            oEmailService = service;
        }

        public object Get()
        {
            return oEmailService.GetAll.Select(s => s.Code);
        }

        public object Get(string code)
        {
            return oEmailService.GetEmailTemplateByCode(code);
        }
    }
}
