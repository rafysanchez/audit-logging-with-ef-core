namespace Demo.Reporting
{
    internal class AuditEntry
    {
        public string EntityName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangedAt { get; set; }
        public string ChangedBy { get; set; }
    }
}