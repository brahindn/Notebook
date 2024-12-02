using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts.Repositories;
using Notebook.Shared.RequestFeatures;
using System.Linq.Expressions;

namespace Notebook.Repositories.Implementation.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Contact> GetContactByIdAsync(Guid contactId)
        {
            return await FindByCondition(c => c.Id == contactId).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(ContactParameters contactParameters)
        {
            return await GetAll().Skip((contactParameters.PageNumber - 1) * contactParameters.PageSize).Take(contactParameters.PageSize).ToListAsync();
        }
                
        public async Task<IEnumerable<Contact>> GetContactByFieldsAsync(IQueryable<Contact> query)
        {
            var contacts = await FindByCondition(e => e == query).ToListAsync();

            return contacts;
        }

        
    }
}   
