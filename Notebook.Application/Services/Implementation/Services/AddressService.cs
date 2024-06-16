using AutoMapper;
using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Repositories.Contracts;
using Notebook.Shared.RequestFeatures;
using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;

namespace Notebook.Application.Services.Implementation.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public AddressService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task CreateAddressAsync(AddressForCreateDTO addressDTO)
        {
            
            var contact = await _repositoryManager.Contact.GetContactAsync(addressDTO.ContactId) ?? throw new ArgumentException("Person cannot be null or empty");
            var address = _mapper.Map<Address>(addressDTO);

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

        public async Task<Address> GetAddressByFields(Guid? contactId, AddressType? addressType, string? country, string? region, string? city, string? street, int? buildingNumber)
        {
            var query = _repositoryManager.Address.GetAll();

            if(contactId != null)
            {
                query = query.Where(a => a.PersonId == contactId);
            }
            if(addressType != null)
            {
                query = query.Where(a => a.AddressType == addressType);
            }
            if(country != null)
            {
                query = query.Where(a => a.Country == country);
            }
            if(region != null)
            {
                query = query.Where(a => a.Region == region);
            }
            if(city != null)
            {
                query = query.Where(a => a.City == city);
            }
            if(street != null)
            {
                query = query.Where(a => a.City == city);
            }
            if(buildingNumber != null)
            {
                query = query.Where(a => a.BuildingNumber == buildingNumber);
            }

            return await _repositoryManager.Address.GetAddressByFieldsAsync(query);
        }
    }
}
