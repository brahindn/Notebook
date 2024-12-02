using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Repositories.Contracts.Repositories
{
    public interface IAddressRepository
    {
        void Create(Address address);
        void Update(Address address);
        void Delete(Address address);
        Task<Address> GetAddressByIdAsync(Guid id);
        Task<IEnumerable<Address>> GetAddressByFieldsAsync(IQueryable<Address> addresses);
        IQueryable<Address> GetAll();
        Task<IEnumerable<Address>> GetAddressesAsync(AddressParameters addressParameters);

    }
}
