using Microsoft.Data.Sqlite;

namespace Demo.Reporting
{
    internal class StudentReport : AuditReport
    {
        private readonly int id;

        public StudentReport(SqliteConnection connection, int id) : base(connection)
        {
            this.id = id;
        }

        protected override string GetTitle()
        {
            return $"Changes for student no {id}:";
        }

        protected override string GetSql()
        {
            return $"select * from auditlogs where RootEntityName=\'Student\' and RootEntityId=\'{id}\'";
        }
    }
}