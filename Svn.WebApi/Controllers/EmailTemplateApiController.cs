using System.Linq;
using Svn.Service;
using System.Web.Http;

namespace Svn.WebApi.Controllers
{
    [Authorize]
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

        public object Get(string templateName)
        {
            return oEmailService.GetEmailTemplateByCode(templateName);
        }
    }
}
