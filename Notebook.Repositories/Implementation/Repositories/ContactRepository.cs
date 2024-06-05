using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts.Repositories;
using Notebook.Shared.RequestFeatures;

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

        public async Task<Contact> GetContactByFieldsAsync(string? newFirstName, string? newLastName, string? newPhoneNumber, string? newEmail)
        {
            var contact = await FindByCondition(c =>
            (c.FirstName == newFirstName || c.FirstName == null) ||
            (c.LastName == newLastName || c.LastName == null) ||
            (c.PhoneNumber == newPhoneNumber || c.PhoneNumber == null) ||
            (c.Email == newEmail || c.Email == null)).
            SingleOrDefaultAsync();

            return contact;         
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(ContactParameters contactParameters)
        {
            return await GetAll().Skip((contactParameters.PageNumber - 1) * contactParameters.PageSize).Take(contactParameters.PageSize).ToListAsync();
        }
    }
}   
