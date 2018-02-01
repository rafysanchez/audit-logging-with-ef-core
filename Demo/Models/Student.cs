using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Demo.Auditing;

namespace Demo.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Student : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
        public City Hometown { get; set; }
        public List<NextOfKin> NextOfKins { get; set; }
    }
}