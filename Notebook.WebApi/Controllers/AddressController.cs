using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Domain;

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
        public async Task<IActionResult> AddAddress(AddressType addressType, string country, string region, string city, string street, int buildingNumber, Guid contactId)
        {
            try
            {
                await _serviceManager.AddressService.CreateAddressAsync(addressType, country, region, city, street, buildingNumber, contactId);

                _logger.Information($"New address for contact {contactId} has been added successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(Guid addressId, AddressType newAddressType, string newCountry, string newRegion, string newCity, string newStreet, int newBuildingNumber)
        {
            try
            {
                var existAddress = await _serviceManager.AddressService.GetAddressAsync(addressId);

                if (existAddress == null)
                {
                    return NotFound();
                }

                await _serviceManager.AddressService.UpdateAddressAsync(addressId, newAddressType, newCountry, newRegion, newCity, newStreet, newBuildingNumber);

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

                _logger.Information($"Address for contact {address.PersonId} has been got");

                return Ok(address);
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
