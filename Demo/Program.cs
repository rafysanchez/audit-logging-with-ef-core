using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Models;
using Demo.Reporting;
using Demo.Repositories;
using Microsoft.Data.Sqlite;

namespace Demo
{
    internal class Program
    {
        public static void Main()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var context = StudentDbContext.Create(connection);

                var classes = new ClassRepository(context);
                classes.Create(new Class {Id = 1, Name = "Math"});

                var students = new StudentRepository(context);
                var math = classes.Get(1);
                var mathStudent = new Student
                {
                    Id = 1,
                    Name = "math student",
                    Classes = new List<Class> {math}
                };
                students.Create(mathStudent);

                var historyStudent = students.Get(1);
                historyStudent.Name = "history student";
                historyStudent.Classes.Add(new Class {Id = 2, Name = "History"});
                students.Update(historyStudent);
                new StudentReport(connection, 1).Write();
                new ClassesReport(connection).Write();
            }
        }
    }
}