﻿using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts.Repositories;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Repositories.Implementation.Repositories
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Address> GetAddressAsync(Guid addressId)
        {
            return await FindByCondition(a => a.Id == addressId).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync(AddressParameters addressParameters)
        {
            return await GetAll().Skip((addressParameters.PageNumber - 1) * addressParameters.PageSize).Take(addressParameters.PageSize).ToListAsync();
        }
    }
}
