using Notebook.Domain;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IAddressService
    {
        Task CreateAddressAsync(AddressType addressType, string country, string region, string city, string street, int buildingNumber);
        Task GetAddressAsync(Guid personId);
    }
}
