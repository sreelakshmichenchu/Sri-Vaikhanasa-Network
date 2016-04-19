using System.Linq;
using Svn.Service;
using System.Web.Http;
using Svn.Model;

namespace Svn.WebApi.Controllers
{
    [RoutePrefix("api/EmailTemplateApi")]
    [Authorize]
    public class EmailTemplateApiController : ApiController
    {
        readonly IEmailService oEmailService;

        public EmailTemplateApiController(IEmailService service)
        {
            oEmailService = service;
        }

        [Route("")]
        [HttpGet]
        public object Get()
        {
            return oEmailService.GetAll.Select(s => s.Code);
        }

        [Route("Email/GET")]
        [HttpGet]
        public object Get(string templateName)
        {
            return oEmailService.GetEmailTemplateByCode(templateName);
        }

        [Route("Save")]
        [HttpPost]
        public void Save(EmailTemplate eTemplate)
        {
            oEmailService.Create(eTemplate);
        }

        [Route("Update")]
        [HttpPost]
        public void Update(EmailTemplate eTemplate)
        {
            oEmailService.Update(eTemplate);
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(EmailTemplate eTemplate)
        {
            oEmailService.Delete(eTemplate);
        }
    }
}
