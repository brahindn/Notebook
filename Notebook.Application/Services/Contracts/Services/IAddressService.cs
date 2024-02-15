using Notebook.Domain;
using Notebook.Domain.Entities;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IAddressService
    {
        Task CreateAddressAsync(AddressType addressType, string country, string region, string city, string street, int buildingNumber, Guid contactId);
        Task<Address> GetAddressAsync(Guid personId);
    }
}
