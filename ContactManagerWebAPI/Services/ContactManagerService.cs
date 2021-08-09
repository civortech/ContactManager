using ContactManagerWebAPI.Data;
using ContactManagerWebAPI.DTOs;
using ContactManagerWebAPI.Extensions;
using ContactManagerWebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManagerWebAPI.Services
{
    public class ContactManagerService : IContactManagerService
    {
        private readonly ContactManagerDbContext _dbContext;
        private readonly ILogger<ContactManagerService> _logger;

        public ContactManagerService(ILogger<ContactManagerService> logger, ContactManagerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<SupplierResponseDto> GetSuppliersByPageAsync(int limit, int page, CancellationToken cancellationToken)
        {
            var suppliers = await _dbContext.Suppliers.AsNoTracking().PaginateAsync(page, limit, cancellationToken);

            return new SupplierResponseDto
            {
                CurrentPage = suppliers.CurrentPage,
                TotalPages = suppliers.TotalPages,
                TotalItems = suppliers.TotalItems,
                Items = suppliers.Items.Select(p => new SupplierDto
                {
                    Id = p.Id,
                    PersonId = p.PersonId,
                    Telephone = p.Telephone
                }).ToList()
            };
        }

        public async Task<ContactResponseDto> GetContactsByPageAsync(int limit, int page, CancellationToken cancellationToken)
        {
            var contacts = await 
                (from p in _dbContext.VwPersons

                join c in _dbContext.Customers
                    on p.PersonId equals c.PersonId into j1
                from r1 in j1.DefaultIfEmpty()

                join s in _dbContext.Suppliers
                    on p.PersonId equals s.PersonId into j2
                from r2 in j2.DefaultIfEmpty()

                select new
                {
                    p.PersonId,
                    p.NameId,
                    ContactId = r1.Id + "" == "" ? r2.Id : r1.Id,
                    Type = r1.Id + "" == "" ? "Supplier" : "Contact",
                    p.First,
                    p.Last,
                    r1.Email,
                    r1.Birthday,
                    r2.Telephone
                })
                .AsNoTracking()
                .OrderBy(p => p.Last)
                .PaginateAsync(page, limit, cancellationToken);

            return new ContactResponseDto
            {
                CurrentPage = contacts.CurrentPage,
                TotalPages = contacts.TotalPages,
                TotalItems = contacts.TotalItems,
                Items = contacts.Items.Select(p => new ContactDto
                {
                    PersonId = p.PersonId,
                    NameId = p.NameId,
                    ContactId = p.ContactId,
                    Type = p.Type,
                    First = p.First,
                    Last = p.Last,
                    Email = p.Email,
                    Birthday = p.Birthday,
                    Telephone = p.Telephone
                }).ToList()
            };
        }
    }
}
