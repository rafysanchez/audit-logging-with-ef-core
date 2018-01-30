using System;

namespace Demo.Auditing
{
    public interface IAuditable
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset ChangedAt { get; set; }
        string CreatedBy { get; set; }
        string ChangedBy { get; set; }
        string DeactivatedBy { get; set; }
    }
}