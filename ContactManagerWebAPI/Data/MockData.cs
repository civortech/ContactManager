using ContactManagerWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagerWebAPI.Data
{
    public class MockData
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = new ContactManagerDbContext(
                      serviceProvider
                      .GetRequiredService<DbContextOptions<ContactManagerDbContext>>());

            if (!context.Names.Any())
            {
                var names = new List<Name>
                {
                    new Name{ First = "Jean", Last = "Petrovic" },
                    new Name{ First = "John", Last = "Doe" },
                    new Name{ First = "Sue", Last = "Smith" },
                    new Name{ First = "Sina", Last = "Alrais" }
                };

                context.Names.AddRange(names);
                context.SaveChanges();
            }

            if (!context.Persons.Any())
            {
                var persons = new List<Person>
                {
                    new Person{ NameId = 1 },
                    new Person{ NameId = 2 },
                    new Person{ NameId = 3 },
                    new Person{ NameId = 4 },
                };

                context.Persons.AddRange(persons);
                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer{ PersonId = 1, Birthday = null, Email = "jp@email.com" },
                    new Customer{ PersonId = 2, Birthday = new DateTime(1987, 3, 10, 0, 0, 0), Email = "s@email.com" }
                };

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            if (!context.Suppliers.Any())
            {
                var suppliers = new List<Supplier>
                {
                    new Supplier{ PersonId = 3, Telephone = "1234567" },
                    new Supplier{ PersonId = 4, Telephone = "1234567890" },
                };

                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

        }
    }
}
