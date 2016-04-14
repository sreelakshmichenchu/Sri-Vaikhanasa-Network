using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Model
{
    [Table("ErrorLog")]
    public class ErrorLog : TrackableEntity
    {
        public string ErrorMessage { get; set; }
        public string InnerExceptions { get; set; }
        public string StackTrace { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string SessionID { get; set; }
    }
}
