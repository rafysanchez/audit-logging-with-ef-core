using System;
using Demo.Auditing;

namespace Demo.Models {
    public class City : IAuditable
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public static City Create(string name)
        {
            return new City
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }
    }
}