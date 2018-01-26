using System.Collections.Generic;
using Demo.Auditing;

namespace Demo.Models
{
    public class Student : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
    }
}