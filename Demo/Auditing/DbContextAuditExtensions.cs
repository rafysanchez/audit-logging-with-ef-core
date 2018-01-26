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

        private static string PrimaryKeyValue(EntityEntry changedEntity)
        {
            var primaryKey = changedEntity.Metadata.FindPrimaryKey();
            var primaryKeyValue = primaryKey.Properties[0].PropertyInfo.GetValue(changedEntity.Entity).ToString();
            return primaryKeyValue;
        }

        private static bool NonAuditProperty(IProperty p)
        {
            var auditProperties = new[] {"CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt"};
            return !auditProperties.Contains(p.Name);
        }

        private static string OldValue(EntityEntry<IAuditable> changedEntity, string name)
        {
            return changedEntity.Property(name).OriginalValue?.ToString();
        }

        private static string CurrentValue(EntityEntry<IAuditable> changedEntity, string name)
        {
            return changedEntity.Property(name).CurrentValue?.ToString();
        }
    }
}