using System.Linq;
using Svn.Service;
using System.Web.Http;

namespace Svn.WebApi.Controllers
{
    public class EmailTemplateController : ApiController
    {
        readonly IEmailService oEmailService;

        public EmailTemplateController(IEmailService service)
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
