using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;
using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace Demo.Reporting
{
    internal abstract class AuditReport
    {
        protected abstract string GetTitle();
        protected abstract string GetSql();

        protected AuditReport(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void Write()
        {
            Console.Out.WriteLine(GetTitle());
            var entries = GetHistoricEntries();
            ConsoleTable.From(entries).Write();
            Console.Out.WriteLine();
        }

        private IEnumerable<HistoricEntry> GetHistoricEntries()
        {
            var logs = GetEntries();

            return 
                from log in logs
                let newValues = NewValues(log)
                let oldValues = OldValues(log)
                from property in InterestingProperties(newValues)
                let oldValue = oldValues?[property]
                let newValue = newValues[property]
                select HistoricEntry.Create(log, property, oldValue, newValue);
        }

        private IEnumerable<AuditEntry> GetEntries()
        {
            var sql = GetSql();
            return connection.Query<AuditEntry>(sql);
        }

        private static IEnumerable<string> InterestingProperties(Dictionary<string, string> newValues)
        {
            return newValues.Keys.Where(key => !key.EndsWith("Id"));
        }

        private static Dictionary<string, string> OldValues(AuditEntry log)
        {
            return log.OldValue == null
                ? null
                : DeserializeProperties(log.OldValue);
        }

        private static Dictionary<string, string> NewValues(AuditEntry log)
        {
            return DeserializeProperties(log.NewValue);
        }

        private static Dictionary<string, string> DeserializeProperties(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        private readonly SqliteConnection connection;
    }
}