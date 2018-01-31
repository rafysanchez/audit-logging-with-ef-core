using System.Diagnostics.CodeAnalysis;
using Demo.Auditing;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public StudentDbContext(DbContextOptions options) : base(options) { }
    }
}