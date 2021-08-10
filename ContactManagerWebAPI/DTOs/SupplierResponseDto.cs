using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagerWebAPI.DTOs
{
    public class SupplierResponseDTO
    {
        public int CurrentPage { get; init; }

        public int TotalItems { get; init; }

        public int TotalPages { get; init; }

        public List<SupplierDTO> Items { get; init; }
    }
}
