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

                var context = StudentDbContext.Create(connection);

                var students = new StudentRepository(context);
                var mathStudent = new Student
                {
                    var student = students.Get(1);
                    student.Name = "student";
                    student.Classes.Add(new Class { Id = 2, Name = "History" });
                    students.Update(student);
                }
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