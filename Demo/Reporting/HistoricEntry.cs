using System;

namespace Demo.Reporting
{
    internal class HistoricEntry
    {
        public string User { get; set; }
        public string Changed { get; set; }
        public string Property { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime At { get; set; }

        public static HistoricEntry Create(AuditEntry log, string property, string oldValue, string newValue)
        {
            return new HistoricEntry
            {
                User = log.ChangedBy,
                Changed = log.EntityName,
                Property = property,
                From = oldValue,
                To = newValue,
                At = Convert.ToDateTime(log.ChangedAt)
            };
        }
    }
}