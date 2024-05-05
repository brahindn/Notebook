using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Implementation.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepositoryManager _repositoryManager;

        public AddressService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task CreateAddressAsync(AddressType addressType, string country, string region, string city, string street, int buildingNumber, Guid contactId)
        {
            if (!(addressType.Equals(AddressType.Personal) || addressType.Equals(AddressType.Business)) || string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(street) || contactId==Guid.Empty)
            {
                throw new ArgumentException("AddressType, Country, City, Street and PersonId cannot be null or empty");
            }

            var contact = await _repositoryManager.Contact.GetContactAsync(contactId) ?? throw new ArgumentException("Person cannot be null or empty");

            var address = new Address
            {
                Person = contact,
                AddressType = addressType,
                Country = country,
                Region = region,
                City = city,
                Street = street,
                BuildingNumber = buildingNumber
            };

            _repositoryManager.Address.Create(address);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateAddressAsync(Guid addressId, AddressType? newAddressType, string? newCountry, string? newRegion, string? newCity, string? newStreet, int? newBuildingNumber)
        {
            var existAddress = await _repositoryManager.Address.GetAddressAsync(addressId) ?? throw new ArgumentNullException($"That address {addressId} was not found.");

            existAddress.AddressType = newAddressType ?? existAddress.AddressType;
            existAddress.Country = newCountry ?? existAddress.Country;
            existAddress.Region = newRegion ?? existAddress.Region;
            existAddress.City = newCity ?? existAddress.Country;
            existAddress.Street = newStreet ?? existAddress.Street;
            existAddress.BuildingNumber = newBuildingNumber ?? existAddress.BuildingNumber;

            _repositoryManager.Address.Update(existAddress);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteAddressAsync(Address address)
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            _repositoryManager.Address.Delete(address);
            await _repositoryManager.SaveAsync();
        }

        public Task<Address> GetAddressAsync(Guid addressId)
        {
            return _repositoryManager.Address.GetAddressAsync(addressId);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync(AddressParameters addressParameters)
        {
            return await _repositoryManager.Address.GetAddressesAsync(addressParameters);
        }
    }
}
