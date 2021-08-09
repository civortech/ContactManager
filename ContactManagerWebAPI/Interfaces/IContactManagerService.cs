using ContactManagerWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManagerWebAPI.Interfaces
{
    public interface IContactManagerService
    {
        Task<SupplierResponseDto> GetSuppliersByPageAsync(int limit, int page, CancellationToken cancellationToken);
        Task<ContactResponseDto> GetContactsByPageAsync(int limit, int page, CancellationToken cancellationToken);
    }
}
