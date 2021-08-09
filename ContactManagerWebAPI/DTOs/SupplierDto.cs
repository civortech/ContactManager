using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagerWebAPI.DTOs
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Telephone { get; set; }
    }
}
