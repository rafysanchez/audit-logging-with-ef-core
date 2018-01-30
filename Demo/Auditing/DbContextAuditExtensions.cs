using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;

namespace Demo.Auditing
{
    internal static class DbContextAuditExtensions
    {
        public static void SaveChangesWithAudit(this DbContext dbContext, object rootEntity)
        {
            var userName = Environment.GetEnvironmentVariable("USERNAME");
            var now = DateTime.Now;
            var entityEntries = dbContext.ChangeTracker.Entries<IAuditable>().ToList();
            var root = dbContext.ChangeTracker.Entries().Single(entry => entry.Entity == rootEntity);


            foreach (var changedEntity in entityEntries)
            {
                if (changedEntity.State == EntityState.Unchanged) continue;

                var properties = changedEntity.Metadata.GetProperties().ToList();

                var propertyValues = properties.Where(NonAuditProperty).Select(p => new
                    {
                        p.Name,
                        OldValue = OldValue(changedEntity, p.Name),
                        CurrentValue = CurrentValue(changedEntity, p.Name)
                    })
                    .ToList();

                var newValues = propertyValues.ToDictionary(v => v.Name, v => v.CurrentValue);

                var changedValues = propertyValues.Where(arg => arg.CurrentValue != arg.OldValue).ToList();

                var oldValues = changedValues
                    .ToDictionary(v => v.Name, v => v.OldValue);
                var currentValues = changedValues
                    .ToDictionary(v => v.Name, v => v.CurrentValue);


                var auditLogs = dbContext.Set<AuditLog>();
                if (changedEntity.State == EntityState.Added)
                {
                    changedEntity.Property("CreatedBy").CurrentValue = userName;
                    changedEntity.Property("CreatedAt").CurrentValue = now;

                    var log = new AuditLog
                    {
                        EntityName = changedEntity.Entity.GetType().Name,
                        EntityId = PrimaryKeyValue(changedEntity),
                        OldValue = null,
                        NewValue = JsonConvert.SerializeObject(newValues),
                        ChangedAt = now,
                        ChangedBy = userName,
                        State = changedEntity.State.ToString("G"),
                        RootEntityName = root.Entity.GetType().Name,
                        RootEntityId = PrimaryKeyValue(root)
                    };

                    auditLogs.Add(log);
                }
                else if (changedEntity.State == EntityState.Modified)
                {
                    changedEntity.Property("UpdatedBy").CurrentValue = userName;
                    changedEntity.Property("UpdatedAt").CurrentValue = now;

                    var log = new AuditLog
                    {
                        EntityName = changedEntity.Entity.GetType().Name,
                        EntityId = PrimaryKeyValue(changedEntity),
                        OldValue = JsonConvert.SerializeObject(oldValues),
                        NewValue = JsonConvert.SerializeObject(currentValues),
                        ChangedAt = now,
                        ChangedBy = userName,
                        State = changedEntity.State.ToString("G"),
                        RootEntityName = root.Entity.GetType().Name,
                        RootEntityId = PrimaryKeyValue(root)
                    };

                    auditLogs.Add(log);
                }
            }

            dbContext.SaveChanges();
        }

        private static EntityEntry GetRootEntry(this DbContext dbContext, object rootEntity)
        {
            if (rootEntity == null) return null;
            return dbContext.ChangeTracker.Entries().Single(entry => entry.Entity == rootEntity);
        }

        private static IEnumerable<EntityEntry<IAuditable>> GetAuditableEntries(this DbContext context)
        {
            return context.ChangeTracker.Entries<IAuditable>().ToList();
        }

        private static bool IsDeleted(this EntityEntry entry)
        {
            return entry.State == EntityState.Deleted;
        }

        private static bool IsCreated(this EntityEntry entry)
        {
            return entry.State == EntityState.Added;
        }

        private static bool IsModified(this EntityEntry entry)
        {
            return entry.State == EntityState.Modified;
        }

        private static bool IsUnchanged(this EntityEntry entry)
        {
            return entry.State == EntityState.Unchanged;
        }

        private static List<AuditProperty> GetChangedValues(this IEnumerable<AuditProperty> properties)
        {
            return properties.Where(arg => arg.CurrentValue != arg.OldValue).ToList();
        }

        private static Dictionary<string, string> ToDictionary(this IEnumerable<AuditProperty> properties,
            Func<AuditProperty, string> value)
        {
            return properties.ToDictionary(v => v.Name, value);
        }

        private static List<AuditProperty> GetPropertyValues(this EntityEntry<IAuditable> entry)
        {
            var properties = entry.Metadata.GetProperties().ToList();

            return properties
                .Where(NonAuditProperty)
                .Select(property => AuditProperty.From(entry, property))
                .ToList();
        }

        private static string TypeName(this EntityEntry entry)
        {
            return entry.Entity.GetType().Name;
        }

        private static string Serialize(this Dictionary<string, string> values)
        {
            return JsonConvert.SerializeObject(values);
        }

        private static string TypeName(this IAuditable auditable)
        {
            return auditable.GetType().Name;
        }

        private static string PrimaryKeyValue(this EntityEntry entry)
        {
            var primaryKey = entry.Metadata.FindPrimaryKey();
            return primaryKey.Properties[0].PropertyInfo.GetValue(entry.Entity).ToString();
        }

        private static bool NonAuditProperty(this IProperty property)
        {
            var auditProperties = new[]
                {"CreatedBy", "CreatedAt", "ChangedBy", "ChangedAt", "DeactivatedBy", "DeactivatedAt"};
            return !auditProperties.Contains(property.Name);
        }

        public static string OldValue(this EntityEntry entry, string name)
        {
            return entry.Property(name).OriginalValue?.ToString();
        }

        public static string CurrentValue(this EntityEntry entry, string name)
        {
            return entry.Property(name).CurrentValue?.ToString();
        }
    }

    internal class AuditProperty
    {
        public string Name { get; set; }
        public string OldValue { get; set; }
        public string CurrentValue { get; set; }

        public static AuditProperty From(EntityEntry<IAuditable> entry, IProperty property)
        {
            return new AuditProperty
            {
                Name = property.Name,
                OldValue = entry.OldValue(property.Name),
                CurrentValue = entry.CurrentValue(property.Name)
            };
        }
    }
}