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

        public Task<Address> GetAddressAsync(Guid personId)
        {
            return _repositoryManager.Address.GetAddressAsync(personId);
        }
    }
}
