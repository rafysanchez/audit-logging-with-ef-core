using Demo.Auditing;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public StudentDbContext(DbContextOptions options) : base(options) { }
    }
}