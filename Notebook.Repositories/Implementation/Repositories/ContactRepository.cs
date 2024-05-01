using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts.Repositories;
using Notebook.Shared.RequestFeatures;
using System.Globalization;

namespace Notebook.Repositories.Implementation.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Contact> GetContactAsync(Guid contactId)
        {
            return await FindByCondition(c => c.Id == contactId).SingleOrDefaultAsync();
        }

        public async Task<Contact> GetContactByFieldsAsync(string newFirstName, string newLastName, string newPhoneNumber)
        {
            return await FindByCondition(c =>
            c.FirstName == newFirstName &&
            c.LastName == newLastName && 
            c.PhoneNumber == newPhoneNumber).
            SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(ContactParameters contactParameters)
        {
            return await GetAll().Skip((contactParameters.PageNumber - 1) * contactParameters.PageSize).Take(contactParameters.PageSize).ToListAsync();
        }
    }
}
