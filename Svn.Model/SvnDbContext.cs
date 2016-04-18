namespace Svn.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using System.Data.Entity.Validation;
    using System.Text;

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
            try
            {
                PreSaveLogic();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
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
