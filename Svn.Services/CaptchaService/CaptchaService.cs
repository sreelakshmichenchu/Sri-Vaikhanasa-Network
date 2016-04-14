using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Service
{
    public class CaptchaService : ICaptchaService
    {
        public byte[] GetNewCaptcha(out string captchaText)
        {
            System.Drawing.FontFamily family = new System.Drawing.FontFamily("Arial");
            CaptchaImage img = new CaptchaImage(150, 40, family);
            string text = img.CreateRandomText(4) + " " + img.CreateRandomText(3);
            img.SetText(text);
            img.GenerateImage();
            captchaText = text;
            var bitmapBytes = CaptchaImage.BitmapToBytes(img.Image);
            return bitmapBytes;
        }
    }
}
