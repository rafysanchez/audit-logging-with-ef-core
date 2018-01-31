using System;
using System.Diagnostics.CodeAnalysis;

namespace Demo.Auditing
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IAuditable
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset ChangedAt { get; set; }
        string CreatedBy { get; set; }
        string ChangedBy { get; set; }
        string DeactivatedBy { get; set; }
    }
}