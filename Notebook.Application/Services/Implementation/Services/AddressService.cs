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

        public async Task CreateAddressAsync(AddressType addressType, string country, string region, string city, string street, int buildingNumber)
        {
            if(!addressType.Equals("Personal") || !addressType.Equals("Business") || string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(street))
            {
                return;
            }

            var address = new Address
            {
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

        public Task GetAddressAsync(Guid personId)
        {
            return _repositoryManager.Address.GetAddressAsync(personId);
        }
    }
}
