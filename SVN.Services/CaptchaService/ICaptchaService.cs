using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service
{
    public interface ICaptchaService
    {
        byte[] GetNewCaptcha(out string captchaText);
    }
}
