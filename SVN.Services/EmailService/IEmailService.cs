using Svn.Model;
using System;

namespace Svn.Service
{
    public interface IEmailService : IEntityService<EmailTemplate>
    {
        string GetEmailTemplateByCode(string code);
    }
}
