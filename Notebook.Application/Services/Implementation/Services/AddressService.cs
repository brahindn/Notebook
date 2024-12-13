using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;
using Notebook.Repositories.Contracts;
using Notebook.Shared.RequestFeatures;

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

        public async Task CreateAddressAsync(CreateAddressRequest createAddressRequest)
        {
            var contact = await _repositoryManager.Contact.GetContactByIdAsync(createAddressRequest.ContactId) ?? throw new ArgumentNullException($"The address without any contact");

            if(contact == null)
            {
                throw new ArgumentNullException(nameof(createAddressRequest), "Contact not found");
            }

            var address = _mapper.Map<Address>(createAddressRequest);

            _repositoryManager.Address.Create(address);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateAddressAsync(UpdateAddressRequest updateAddressRequest)
        {
            var existAddress = await _repositoryManager.Address.GetAddressByIdAsync(updateAddressRequest.Id) ?? throw new ArgumentNullException($"That address {updateAddressRequest.Id} was not found.");

            if (!updateAddressRequest.AddressType.HasValue &&
               string.IsNullOrEmpty(updateAddressRequest.Country) &&
               string.IsNullOrEmpty(updateAddressRequest.Region) &&
               string.IsNullOrEmpty(updateAddressRequest.City) &&
               string.IsNullOrEmpty(updateAddressRequest.Street) &&
               !updateAddressRequest.BuildingNumber.HasValue)
            {
                throw new ArgumentException("At least one field must be provided.");
            }

            var updatedAddress = _mapper.Map(updateAddressRequest, existAddress);

            _repositoryManager.Address.Update(updatedAddress);
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

        public Task<Address> GetAddressByIdAsync(Guid addressId)
        {
            return _repositoryManager.Address.GetAddressByIdAsync(addressId);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync(AddressParameters addressParameters)
        {
            return await _repositoryManager.Address.GetAddressesAsync(addressParameters);
        }

        public async Task<IEnumerable<GetAddressResponse>> GetAddressByFieldsAsync(GetAddressRequest addressRequest)
        {
            var query = _repositoryManager.Address.GetAll();

            if(addressRequest.ContactId != null)
            {
                query = query.Where(a => a.ContactId == addressRequest.ContactId);
            }
            if(addressRequest.AddressType != null)
            {
                query = query.Where(a => a.AddressType == addressRequest.AddressType);
            }
            if(addressRequest.Country != null)
            {
                query = query.Where(a => a.Country == addressRequest.Country);
            }
            if(addressRequest.Region != null)
            {
                query = query.Where(a => a.Region == addressRequest.Region);
            }
            if(addressRequest.City != null)
            {
                query = query.Where(a => a.City == addressRequest.City);
            }
            if(addressRequest.Street != null)
            {
                query = query.Where(a => a.Street == addressRequest.Street);
            }
            if(addressRequest.BuildingNumber != null)
            {
                query = query.Where(a => a.BuildingNumber == addressRequest.BuildingNumber);
            }

            var addresses = await query.ToListAsync();

            return _mapper.Map<IEnumerable<GetAddressResponse>>(addresses);
        }
    }
}
