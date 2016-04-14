namespace Svn.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity;

    public class SvnDbContext : DbContext
    {
        public SvnDbContext()
            : base("name=" + Svn.Resources.Constants.DefaultConnection)
        {
            
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<ErrorLog> ApplicationErrors { get; set; }

        public override int SaveChanges()
        {
            PreSaveLogic();
            return base.SaveChanges();
        }

        private void PreSaveLogic()
        {
            var entities = ChangeTracker.Entries()
                .Where(item => item.Entity is ITrackableEntity && (item.State == EntityState.Added || item.State == EntityState.Modified));

            Guid currentUserId = (IsUserAuthenticated())
                                    ? Guid.Parse(HttpContext.Current.User.Identity.GetUserId())
                                    : Guid.Empty;

            var now = DateTime.UtcNow;

            foreach (var entry in entities)
            {
                if (entry.State == EntityState.Added)
                {
                    ((ITrackableEntity)entry.Entity).CreatedAt = now;
                    ((ITrackableEntity)entry.Entity).CreatedBy = currentUserId;
                }
                else
                {
                    entry.Property(Svn.Resources.Constants.CreatedAt).IsModified = false;
                    entry.Property(Svn.Resources.Constants.CreatedBy).IsModified = false;
                }

                ((ITrackableEntity)entry.Entity).ModifiedAt = now;
                ((ITrackableEntity)entry.Entity).ModifiedBy = currentUserId;
            }
        }

        private static bool IsUserAuthenticated()
        {
            return HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity != null &&
                !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name);
        }
    }
}
