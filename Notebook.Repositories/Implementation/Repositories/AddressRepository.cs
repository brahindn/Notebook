using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts.Repositories;

namespace Notebook.Repositories.Implementation.Repositories
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Address> GetAddressAsync(Guid contactId)
        {
            return await FindByCondition(a => a.PersonId == contactId).SingleOrDefaultAsync();
        }
    }
}
