using System;
using System.Collections.Generic;

#nullable disable

namespace ContactManagerWebAPI.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }

        public virtual Person Person { get; set; }
    }
}
