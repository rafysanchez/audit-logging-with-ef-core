using Demo.Auditing;
using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public StudentDbContext(DbContextOptions<DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Map(new AuditablePropertiesConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}