using System;
using System.Collections.Generic;
using System.Text;
using Interview;

namespace InMemoryRepositoryTests
{
    public class Car : IStoreable
    {
        public Guid CarId { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public IComparable Id
        {
            get => CarId;
            set => this.Id = CarId;
        }
    }
}
