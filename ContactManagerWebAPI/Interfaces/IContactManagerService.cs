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
        Task<SupplierResponseDTO> GetSuppliersByPageAsync(int limit, int page, CancellationToken cancellationToken);
        Task<ContactResponseDTO> GetContactsByPageAsync(int limit, int page, CancellationToken cancellationToken);
        Task<ContactDTO> GetContactAsync(int id, CancellationToken cancellationToken);
        Task<ContactDTO> AddContactAsync(ContactDTO contactDTO, CancellationToken cancellationToken);
        Task<ContactDTO> UpdateContactAsync(string type, int id, ContactDTO contactDTO, CancellationToken cancellationToken);
        void DeleteContact(string type, int id);
    }
}
