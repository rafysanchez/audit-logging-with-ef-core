using System;
using System.Data.Common;
using System.Linq;
using Demo.Auditing;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repositories
{
    internal class StudentRepository
    {
        private readonly StudentDbContext context;

        public StudentRepository(StudentDbContext context)
        {
            this.context = context;
        }

        public void Create(Student student)
        {
            context.Users.Add(student);
            context.SaveChangesWithAudit(student);
        }

        public void Update(Student student)
        {
            context.SaveChangesWithAudit(student);
        }

        public Student Get(int id)
        {
            return context.Users.Include(s => s.Classes).Single(s => s.Id == id);
        }
    }
}