using Notebook.Domain.Entities;

namespace Notebook.Repositories.Contracts.Repositories
{
    public interface IAddressRepository
    {
        void Create(Address address);
        Task<Address> GetAddressAsync(Guid contactId);
    }
}
