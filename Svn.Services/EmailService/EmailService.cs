using Svn.Model;
using Svn.Repository;
using System.Linq;
using System.Net.Mail;

namespace Svn.Service
{
    public class EmailService : EntityService<EmailTemplate>, IEmailService
    {
        public EmailService(IUnitOfWork unitOfWork, IRepository<EmailTemplate> repository) : base(unitOfWork, repository) { }

        public string GetEmailTemplateByCode(string code)
        {
            var res = this.FindBy(r => r.Code == code && r.IsActive).ToList();
            if (res.Any())
            {
                return res.First().Template;
            }

            return string.Empty;
        }

        public static void SendEmail(string toEmail, string receiverName, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(new MailAddress(toEmail, receiverName));
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Send(mail);
        }
    }
}
