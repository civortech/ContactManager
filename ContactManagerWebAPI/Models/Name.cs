using System;
using System.Collections.Generic;

#nullable disable

namespace ContactManagerWebAPI.Models
{
    public partial class Name
    {
        public Name()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
