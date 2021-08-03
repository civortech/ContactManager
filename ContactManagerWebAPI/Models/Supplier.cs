using System;
using System.Collections.Generic;

#nullable disable

namespace ContactManagerWebAPI.Models
{
    public partial class Supplier
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Telephone { get; set; }

        public virtual Person Person { get; set; }
    }
}
