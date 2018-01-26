using System;
using System.Linq;
using Demo.Auditing;
using Demo.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repositories
{
    internal class StudentRepository : IDisposable
    {
        private readonly StudentDbContext context;

        public StudentRepository(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseSqlite(connection)
                .Options;

            context = new StudentDbContext(options);
            context.Database.EnsureCreated();
        }

        public void Add(Student student)
        {
            context.Users.Add(student);
        }

        public Student Get(int id)
        {
            return context.Users.Include(s => s.Classes).Single(s => s.Id == id);
        }

        public void SaveChanges(Student student)
        {
            context.SaveChangesWithAudit(student);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}