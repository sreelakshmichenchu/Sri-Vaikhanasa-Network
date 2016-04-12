using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Model
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
