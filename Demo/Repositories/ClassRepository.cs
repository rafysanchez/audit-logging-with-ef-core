using System;
using Demo.Auditing;
using Demo.Models;

namespace Demo.Repositories {
    internal class ClassRepository
    {
        private readonly StudentDbContext context;

        public ClassRepository(StudentDbContext context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        public void Create(Class @class)
        {
            context.Classes.Add(@class);
            context.SaveChangesWithAudit(@class);
        }

        //public void Update(Class @class)
        //{
        //    context.SaveChangesWithAudit(@class);
        //}

        public Class Get(int id)
        {
            return context.Classes.Find(id);
        }
    }
}