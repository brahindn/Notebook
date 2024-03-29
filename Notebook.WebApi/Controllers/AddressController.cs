﻿using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.WebApi.Requests;
using Notebook.WebApi.Responses;

namespace Notebook.WebApi.Controllers
{
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly Serilog.ILogger _logger;

        public AddressController(IServiceManager serviceManager, Serilog.ILogger logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressForCreateDTO address)
        {
            if(address == null)
            {
                return BadRequest("AddressForCreate object is null");
            }

            try
            {
                await _serviceManager.AddressService.CreateAddressAsync(address.AddressType, address.Country, address.Region, address.City, address.Street, address.BuildingNumber, address.ContactId);

                _logger.Information($"New address for contact {address.ContactId} has been added successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(Guid addressId, AddressForUpdateDTO address)
        {
            if (address == null)
            {
                return BadRequest("AddressForUpdateDTO object is null");
            }

            try
            {
                var existAddress = await _serviceManager.AddressService.GetAddressAsync(addressId);

                if (existAddress == null)
                {
                    return NotFound();
                }

                await _serviceManager.AddressService.UpdateAddressAsync(addressId, address.AddressType, address.Country, address.Region, address.City, address.Street, address.BuildingNumber);

                _logger.Information($"Updated address: {addressId}");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"UpdateAddress error: {ex.Message}");
            }
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var existAddress = await _serviceManager.AddressService.GetAddressAsync(addressId);

            if (existAddress == null)
            {
                return NotFound();
            }

            try
            {
                await _serviceManager.AddressService.DeleteAddressAsync(existAddress);

                _logger.Information($"Contact deleted: {existAddress.Id}");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressById(Guid addressId)
        {
            try
            {
                var address = await _serviceManager.AddressService.GetAddressAsync(addressId);

                if(address == null)
                {
                    return NotFound();
                }

                var addressDTO = new AddressResponseDTO
                {
                    Id = address.Id,
                    AddressType = address.AddressType,
                    Country = address.Country,
                    City = address.City,
                    Region = address.Region,
                    Street = address.Street,
                    BuildingNumber = address.BuildingNumber,
                    ContactId = address.PersonId
                };

                _logger.Information($"Address for contact {address.PersonId} has been got");

                return Ok(addressDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"GetAddress error: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllAddress()
        {
            try
            {
                var addresses = _serviceManager.AddressService.GetAllAddressesAsync();

                var addressDTO = addresses.Select(a => new AddressResponseDTO
                {
                    Id = a.Id,
                    AddressType = a.AddressType,
                    Country = a.Country,
                    City = a.City,
                    Region = a.Region,
                    Street = a.Street,
                    BuildingNumber = a.BuildingNumber,
                    ContactId = a.PersonId
                }).ToList();

                _logger.Information("All addresses have been got");

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"GetAddresses error: {ex.Message}");
            }
        }
    }
}
