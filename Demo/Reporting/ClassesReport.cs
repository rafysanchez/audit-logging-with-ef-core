using Microsoft.Data.Sqlite;

namespace Demo.Reporting
{
    internal class ClassesReport : AuditReport
    {
        public ClassesReport(SqliteConnection connection) : base(connection) { }

        protected override string GetTitle()
        {
            return "Changes for all classes:";
        }

        protected override string GetSql()
        {
            return "select * from auditlogs where EntityName=\'Class\'";
        }
    }
}