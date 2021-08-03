using System;
using System.Collections.Generic;

#nullable disable

namespace ContactManagerWebAPI.Models
{
    public partial class VwPerson
    {
        public int PersonId { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
}
