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

        public async Task<Contact> GetContactAsync(Guid contactId)
        {
            return await FindByCondition(c => c.Id == contactId).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(ContactParameters contactParameters)
        {
            return await GetAll().Skip((contactParameters.PageNumber - 1) * contactParameters.PageSize).Take(contactParameters.PageSize).ToListAsync();
        }
                
        public async Task<Contact> GetContactByFieldsAsync(IQueryable<Contact> query)
        {
            Expression<Func<Contact, bool>> expression = a => query.Contains(a);

            var contact = await FindByCondition(expression).FirstOrDefaultAsync();

            return contact;
        }
    }
}   
