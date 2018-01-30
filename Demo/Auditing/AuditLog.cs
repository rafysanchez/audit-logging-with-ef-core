using System;

namespace Demo.Auditing
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string RootEntityName { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset ChangedAt { get; set; }
        public string State { get; set; }
        public string ChangedBy { get; set; }
        public string RootEntityId { get; set; }
    }
}