using System;
using System.Linq;
using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Demo.Auditing
{
    internal class AuditablePropertiesConfig : IModelBuilderMap
    {
        public void Map(ModelBuilder builder)
        {
            var auditables = builder.Model
                .GetEntityTypes()
                .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType));

            foreach (var entityType in auditables)
            {
                builder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>("CreatedAt");

                builder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>("ChangedAt");

                builder.Entity(entityType.ClrType)
                    .Property<string>("CreatedBy");

                builder.Entity(entityType.ClrType)
                    .Property<string>("ChangedBy");

                builder.Entity(entityType.ClrType)
                    .Property<string>("DeactivatedBy");
            }
        }
    }
}