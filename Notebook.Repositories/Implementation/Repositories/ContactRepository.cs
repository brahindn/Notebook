using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts.Repositories;

namespace Notebook.Repositories.Implementation.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Contact> GetContactAsync(string firstName, string lastName)
        {
            return await FindByCondition(c => c.FirstName == firstName || c.LastName == lastName).SingleOrDefaultAsync();
        }
    }
}
