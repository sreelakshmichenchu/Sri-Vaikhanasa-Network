using System;
using System.Data.Entity;

namespace Svn.Model
{
    public static class SvnExtensions
    {
        /// <summary>
        /// Update an entity object's specified columns, comma separated
        /// This method assumes you already have a context open/initialized
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject"></param>
        /// <param name="properties"></param>
        public static void Update<TEntity>(this DbContext context, TEntity entityObject, params string[] properties) where TEntity : class
        {
            context.Set<TEntity>().Attach(entityObject);

            var entry = context.Entry(entityObject);

            foreach (string name in properties)
                entry.Property(name).IsModified = true;

            context.SaveChanges();
        }

        /// <summary>
        /// Update an entity object's columns except specified, comma separated
        /// This method assumes you already have a context open/initialized
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject"></param>
        /// <param name="properties"></param>
        public static void UpdateExcept<TEntity>(this DbContext context, TEntity entityObject, params string[] properties) where TEntity : class
        {
            context.Set<TEntity>().Attach(entityObject);
            
            var entry = context.Entry(entityObject);

            entry.State = System.Data.Entity.EntityState.Modified;

            foreach (string name in properties)
                entry.Property(name).IsModified = false;

            context.SaveChanges();
        }

        /// <summary>
        /// Convert string to Enum value
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Enum value of type T</returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
