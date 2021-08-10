using ContactManagerWebAPI.Data;
using ContactManagerWebAPI.Data.Entities;
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
        private bool ContactExists(int id) => _dbContext.VwPersons.Any(e => e.PersonId == id);

        public ContactManagerService(ILogger<ContactManagerService> logger, ContactManagerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<SupplierResponseDTO> GetSuppliersByPageAsync(int limit, int page, CancellationToken cancellationToken)
        {
            var suppliers = await _dbContext.Suppliers.AsNoTracking().PaginateAsync(page, limit, cancellationToken);

            return new SupplierResponseDTO
            {
                CurrentPage = suppliers.CurrentPage,
                TotalPages = suppliers.TotalPages,
                TotalItems = suppliers.TotalItems,
                Items = suppliers.Items.Select(p => new SupplierDTO
                {
                    Id = p.Id,
                    PersonId = p.PersonId,
                    Telephone = p.Telephone
                }).ToList()
            };
        }

        public async Task<ContactResponseDTO> GetContactsByPageAsync(int limit, int page, CancellationToken cancellationToken)
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
                     Type = r1.Id + "" == "" ? "s" : "c",
                     p.First,
                     p.Last,
                     r1.Email,
                     r1.Birthday,
                     r2.Telephone
                 })
                .AsNoTracking()
                .OrderByDescending(p => p.PersonId)
                .PaginateAsync(page, limit, cancellationToken);

            return new ContactResponseDTO
            {
                CurrentPage = contacts.CurrentPage,
                TotalPages = contacts.TotalPages,
                TotalItems = contacts.TotalItems,
                Items = contacts.Items
                .Select(p => new ContactDTO
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

        public async Task<ContactDTO> GetContactAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.VwContacts
                .Where(p => p.PersonId == id)
                .Select(p => new ContactDTO
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
                }).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ContactDTO> AddContactAsync(ContactDTO contactDTO, CancellationToken cancellationToken)
        {
            var name = new Name
            {
                First = contactDTO.First,
                Last = contactDTO.Last
            };
            await _dbContext.Names.AddAsync(name, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var person = new Person
            {
                NameId = name.Id
            };
            await _dbContext.Persons.AddAsync(person, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if ("c".Equals(contactDTO.Type))
            {
                var customer = new Customer
                {
                    PersonId = person.Id,
                    Birthday = contactDTO.Birthday,
                    Email = contactDTO.Email ?? "" //TODO FIX IN THE FUTURE
                };
                await _dbContext.Customers.AddAsync(customer, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var supplier = new Supplier
                {
                    PersonId = person.Id,
                    Telephone = contactDTO.Telephone ?? "" //TODO FIX IN THE FUTURE
                };
                await _dbContext.Suppliers.AddAsync(supplier, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return contactDTO;
        }

        public async Task<ContactDTO> UpdateContactAsync(string type, int id, ContactDTO contactDTO, CancellationToken cancellationToken)
        {
            var contact = await GetContactAsync(id, cancellationToken);

            var name = new Name
            {
                Id = contact.NameId,
                First = contactDTO.First ?? contact.First,
                Last = contactDTO.Last ?? contact.Last
            };

            _dbContext.Names.Update(name);
            //await _dbContext.SaveChangesAsync(cancellationToken);

            if ("c".Equals(type))
            {
                var customer = new Customer
                {
                    Id = (int)contact.ContactId,
                    PersonId = contact.PersonId,
                    Birthday = contactDTO.Birthday ?? contact.Birthday,
                    Email = contactDTO.Email ?? contact.Email
                };
                _dbContext.Customers.Update(customer);
                //await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var supplier = new Supplier
                {
                    Id = (int)contact.ContactId,
                    PersonId = contact.PersonId,
                    Telephone = contactDTO.Telephone ?? contact.Telephone
                };
                _dbContext.Suppliers.Update(supplier);
                //await _dbContext.SaveChangesAsync(cancellationToken);
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException) when (!ContactExists(contact.PersonId))
            {
                return null;
            }

            return contact;
        }

        public void DeleteContact(string type, int id)
        {
            if ("c".Equals(type))
            {
                var person = _dbContext.Persons
                    .Where(e => e.Id == id)
                    .Include(e => e.Name)
                    .Include(e => e.Customers)
                    .First();

                _dbContext.Persons.Remove(person);

                _dbContext.SaveChanges();
            }
            else
            {
                var person = _dbContext.Persons
                    .Where(e => e.Id == id)
                    .Include(e => e.Name)
                    .Include(e => e.Suppliers)
                    .First();

                _dbContext.Persons.Remove(person);

                _dbContext.SaveChanges();
            }
        }

    }
}
