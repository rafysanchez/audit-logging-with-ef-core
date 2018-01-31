using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Demo.Auditing;

namespace Demo.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Student : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
    }
}