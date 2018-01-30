using System;

namespace Demo.Auditing
{
    public class Auditable : IAuditable
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ChangedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ChangedBy { get; set; }
        public string DeactivatedBy { get; set; }
    }
}