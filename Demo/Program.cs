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
                classes.Create(new Class {Id = 2, Name = "History"});

                var students = new StudentRepository(context);

                var math = classes.Get(1);
                var history = classes.Get(2);

                var jim = new Student
                {
                    Id = 1,
                    Name = "Jim Doe",
                    Classes = new List<Class> {math, history}
                };
                students.Create(jim);

                var nextOfKins = students.Get(1);
                nextOfKins.NextOfKins =
                    new List<NextOfKin> {NextOfKin.Create("John Doe"), NextOfKin.Create("Jane Doe")};
                students.Update(nextOfKins);

                var residence = students.Get(1);
                nextOfKins.Hometown = City.Create("Oslo");
                students.Update(nextOfKins);

                new StudentReport(connection, 1).Write();
                new ClassesReport(connection).Write();

                Console.Out.WriteLine("Classes: " + context.Classes.Count());
                Console.Out.WriteLine("Students: " + context.Students.Count());
            }
        }
    }
}