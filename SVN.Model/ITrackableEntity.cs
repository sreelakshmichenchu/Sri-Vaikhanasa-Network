using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Model
{
    public interface ITrackableEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
        Guid CreatedBy { get; set; }
        DateTime ModifiedAt { get; set; }
        Guid ModifiedBy { get; set; }
    }
}
