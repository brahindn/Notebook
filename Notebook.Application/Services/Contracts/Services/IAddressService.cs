﻿using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IAddressService
    {
        Task CreateAddressAsync(AddressForCreateDTO addressDTO);
        Task UpdateAddressAsync(AddressForUpdateDTO addressDTO);
        Task DeleteAddressAsync(Address address);
        Task<Address> GetAddressAsync(Guid personId);
        Task<IEnumerable<Address>> GetAllAddressesAsync(AddressParameters addressParameters);
        Task<Address> GetAddressByFields(Guid? contactId, AddressType? addressType, string? country, string? region, string? city, string? street, int? buildingNumber);
    }
}
