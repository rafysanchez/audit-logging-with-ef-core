using System.Collections.Generic;
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

                using (var students = new StudentRepository(connection))
                {
                    var student = new Student
                    {
                        Id = 1,
                        Name = "name",
                        Classes = new List<Class> { new Class { Id = 1, Name = "Math" } }
                    };
                    students.Add(student);
                    students.SaveChanges(student);
                }

                using (var students = new StudentRepository(connection))
                {
                    var student = students.Get(1);
                    student.Name = "student";
                    student.Classes.Add(new Class { Id = 2, Name = "History" });
                    students.SaveChanges(student);
                }

                new StudentReport(connection, 1).Write();
                new ClassesReport(connection).Write();
            }
        }
    }
}