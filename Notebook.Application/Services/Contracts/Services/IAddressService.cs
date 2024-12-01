using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IAddressService
    {
        Task CreateAddressAsync(CreateAddressRequest createAddressRequest);
        Task UpdateAddressAsync(UpdateAddressRequest updateAddressRequest);
        Task DeleteAddressAsync(Address address);
        Task<Address> GetAddressByIdAsync(Guid personId);
        Task<IEnumerable<Address>> GetAllAddressesAsync(AddressParameters addressParameters);
        Task<IEnumerable<GetAddressResponse>> GetAddressByFieldsAsync(GetAddressRequest getAddressRequest);
    }
}
