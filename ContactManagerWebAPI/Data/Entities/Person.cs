using System;
using System.Collections.Generic;

#nullable disable

namespace ContactManagerWebAPI.Data.Entities
{
    public partial class Person
    {
        public Person()
        {
            Customers = new HashSet<Customer>();
            Suppliers = new HashSet<Supplier>();
        }

        public int Id { get; set; }
        public int NameId { get; set; }

        public virtual Name Name { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
