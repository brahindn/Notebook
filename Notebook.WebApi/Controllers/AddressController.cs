using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Domain;

namespace Notebook.WebApi.Controllers
{
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AddressController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressType addressType, string country, string region, string city, string street, int buildingNumber, Guid contactId)
        {
            try
            {
                await _serviceManager.AddressService.CreateAddressAsync(addressType, country, region, city, street, buildingNumber, contactId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(Guid id, AddressType newAddressType, string newCountry, string newRegion, string newCity, string newStreet, int newBuildingNumber)
        {
            var existAddress = await _serviceManager.AddressService.GetAddressAsync(id);

            if(existAddress == null)
            {
                return NotFound();
            }

            try
            {
                await _serviceManager.AddressService.UpdateAddressAsync(id, newAddressType, newCountry, newRegion, newCity, newStreet, newBuildingNumber);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"UpdateAddress error: {ex.Message}");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var existAddress = await _serviceManager.AddressService.GetAddressAsync(id) ?? throw new ArgumentException($"Address with id: {id} not found.");

            try
            {
                await _serviceManager.AddressService.DeleteAddressAsync(existAddress);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(Guid personId)
        {
            try
            {
                var address = await _serviceManager.AddressService.GetAddressAsync(personId);

                if(address == null)
                {
                    return NotFound();
                }

                return Ok(address);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"GetAddress error: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllAddress()
        {
            try
            {
                var addresses = _serviceManager.AddressService.GetAllAddressesAsync();

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"GetAddresses error: {ex.Message}");
            }
        }
    }
}
