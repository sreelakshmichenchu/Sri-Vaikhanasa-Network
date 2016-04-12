using Microsoft.Security.Application;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Svn.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AllowSafeHtmlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string html = Convert.ToString(value, CultureInfo.InvariantCulture);
            string sanitizedHtml = Sanitizer.GetSafeHtmlFragment(html);

            if (sanitizedHtml.Equals(html, StringComparison.InvariantCulture))
            {
                this.ErrorMessage = Svn.Resources.Errors.AllowXssSafeHtmlAttributeErrorMessage;
                return false;
            }

            return true;
        }
    }
}
