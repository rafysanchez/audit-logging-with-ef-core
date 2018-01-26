using Demo.Auditing;

namespace Demo.Models
{
    public class Class : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}