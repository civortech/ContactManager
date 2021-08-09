using System;
using System.Collections.Generic;

#nullable disable

namespace ContactManagerWebAPI.Data.Entities
{
    public partial class VwContact
    {
        public int PersonId { get; set; }
        public int NameId { get; set; }
        public int? ContactId { get; set; }
        public string Type { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Telephone { get; set; }
    }
}
