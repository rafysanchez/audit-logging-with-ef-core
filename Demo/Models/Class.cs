using System.Diagnostics.CodeAnalysis;
using Demo.Auditing;

namespace Demo.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Class : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}