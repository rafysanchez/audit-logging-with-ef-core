using System;
using Demo.Auditing;

namespace Demo.Models {
    public class NextOfKin : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static NextOfKin Create(string name)
        {
            return new NextOfKin
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }
    }
}