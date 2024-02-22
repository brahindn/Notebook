using Notebook.Domain;
using Notebook.Domain.Entities;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IAddressService
    {
        Task CreateAddressAsync(AddressType addressType, string country, string region, string city, string street, int buildingNumber, Guid contactId);
        Task UpdateAddressAsync(Guid id, AddressType addressType, string country, string region, string city, string street, int buildingNumber);
        Task DeleteAddressAsync(Address address);
        Task<Address> GetAddressAsync(Guid personId);
        IQueryable<Address> GetAllAddressesAsync();
    }
}
