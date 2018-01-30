using Demo.Auditing;

namespace Demo.Models
{
    public class Class : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}