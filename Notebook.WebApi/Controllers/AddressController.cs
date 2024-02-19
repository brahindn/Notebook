using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Domain;

namespace Notebook.WebApi.Controllers
{
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _loggerManager;
        
        public AddressController(IServiceManager serviceManager, ILoggerManager loggerManager)
        {
            _serviceManager = serviceManager;
            _loggerManager = loggerManager;
        }

        [HttpGet("Address")]
        public async Task<IActionResult> GetAddress(Guid personId)
        {
            try
            {
                _loggerManager.LogInfo($"Getting address for personId: {personId}");

                var address = await _serviceManager.AddressService.GetAddressAsync(personId);

                if(address == null)
                {
                    _loggerManager.LogError($"Address for personId: {personId} not found.");

                    return NotFound();
                }

                _loggerManager.LogInfo($"Address for personId: {personId} retrieved successfully.");

                return Ok(address);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetAddress error: {ex.Message}");

                return StatusCode(500, $"GetAddress error: {ex.Message}");
            }
        }

        [HttpPost("NewAddress")]
        public async Task<IActionResult> CreateAddress(AddressType addressType, string country, string region, string city, string street, int buildingNumber, Guid contactId)
        {
            try
            {
                _loggerManager.LogInfo($"Creating a new address for contactId: {contactId}");

                await _serviceManager.AddressService.CreateAddressAsync(addressType, country, region, city, street, buildingNumber, contactId);

                _loggerManager.LogInfo($"New address for {contactId} created successfully.");

                return Ok();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateAddress error: {ex.Message}");

                return StatusCode(500, $"CreateAddress error: {ex.Message}");
            }
        }
    }
}
