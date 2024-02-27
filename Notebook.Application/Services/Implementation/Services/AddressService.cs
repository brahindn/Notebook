using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts;

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

        public async Task UpdateAddressAsync(Guid addressId, AddressType newAddressType, string newCountry, string newRegion, string newCity, string newStreet, int newBuildingNumber)
        {
            var existAddress = await _repositoryManager.Address.GetAddressAsync(addressId) ?? throw new ArgumentException(nameof(addressId));

            existAddress.AddressType = newAddressType;
            existAddress.Country = newCountry;
            existAddress.Region = newRegion;
            existAddress.City = newCity;
            existAddress.Street = newStreet;
            existAddress.BuildingNumber = newBuildingNumber;

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

        public IQueryable<Address> GetAllAddressesAsync()
        {
            var allAddresses = _repositoryManager.Address.GetAll();

            return allAddresses;
        }
    }
}
