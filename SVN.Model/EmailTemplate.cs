namespace Svn.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("EmailTemplates")]
    public class EmailTemplate : TrackableEntity
    {
        [Required]
        [StringLength(200)]
        public string Code { get; set; }
        [Required]
        [StringLength(1000)]
        public string ModelObject { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Template { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
