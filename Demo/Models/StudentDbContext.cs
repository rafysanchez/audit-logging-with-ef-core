using Demo.Auditing;
using Demo.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        private StudentDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Map(new AuditablePropertiesConfig());
        }

        public static StudentDbContext Create(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new StudentDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}