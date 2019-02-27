using System;
using System.Collections.Generic;
using System.Text;
using Interview;

namespace InMemoryRepositoryTests
{
    public struct Person : IStoreable
    {
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public IComparable Id
        {
            get => PersonId;
            set => this.Id = PersonId;
        }
    }
}
