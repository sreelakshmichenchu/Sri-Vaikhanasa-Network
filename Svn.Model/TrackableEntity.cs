using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svn.Model
{
    public class TrackableEntity : ITrackableEntity
    {
        public Guid Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ModifiedAt { get; set; }

        public Guid ModifiedBy { get; set; }
    }
}
