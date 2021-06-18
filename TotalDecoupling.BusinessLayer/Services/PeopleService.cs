using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TotalDecoupling.BusinessLayer.Models;
using TotalDecoupling.DataAccessLayer;
using TotalDecoupling.Shared.Models;
using Entities = TotalDecoupling.DataAccessLayer.Entities;

namespace TotalDecoupling.BusinessLayer.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IDbContext dbContext;

        public PeopleService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OperationResult<IEnumerable<Person>>> GetAsync()
        {
            var people = await dbContext.GetData<Entities.Person>()
                .OrderBy(p => p.FirstName).ThenBy(p => p.LastName)
                .Select(p => new Person
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName
                })
                .ToListAsync();

            return people;
        }

        public async Task<OperationResult<Person>> GetAsync(Guid id)
        {
            var dbPerson = await dbContext.GetData<Entities.Person>().FirstOrDefaultAsync(p => p.Id == id);
            if (dbPerson == null)
            {
                return OperationResult.Fail(FailureReason.ItemNotFound);
            }

            var person = new Person
            {
                Id = dbPerson.Id,
                FirstName = dbPerson.FirstName,
                LastName = dbPerson.LastName
            };

            return person;
        }

        public async Task<OperationResult> SaveAsync(Person person)
        {
            var dbPerson = person.Id != Guid.Empty ? await dbContext.GetData<Entities.Person>(trackingChanges: true)
                .FirstOrDefaultAsync(p => p.Id == person.Id) : null;

            if (dbPerson == null)
            {
                var samePersonExists = await dbContext.GetData<Entities.Person>()
                    .AnyAsync(p => p.FirstName == person.FirstName && p.LastName == person.LastName
                    && p.CreateDate.AddMinutes(1) > DateTime.UtcNow);

                if (samePersonExists)
                {
                    var validationErrors = new List<ValidationError>
                    {
                        new ValidationError("FirstName", "Nome non valido"),
                        new ValidationError("LastName", "Cognome non valido")
                    };

                    return OperationResult.Fail(FailureReason.ClientError, "Non puoi creare utenti con lo stesso nome in meno di 1 minuto", validationErrors);
                }

                dbPerson = new Entities.Person
                {
                    CreateDate = DateTime.UtcNow
                };

                dbContext.Insert(dbPerson);
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;

            await dbContext.SaveAsync();
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var dbPerson = await dbContext.GetData<Entities.Person>(trackingChanges: true).FirstOrDefaultAsync(p => p.Id == id);

            if (dbPerson != null)
            {
                if (dbPerson.FirstName == "Admin")
                {
                    return OperationResult.Fail(FailureReason.Forbidden, "Non puoi cancellare l'utente admin");
                }

                dbContext.Delete(dbPerson);
                await dbContext.SaveAsync();

                return OperationResult.Ok();
            }
            else
            {
                return OperationResult.Fail(FailureReason.ItemNotFound);
            }
        }
    }
}
