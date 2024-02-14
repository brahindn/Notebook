using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [HttpGet("{personId}")]
        public async Task<IActionResult> GetAddress(Guid personId)
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

        [HttpPost]
        public async Task<IActionResult> CreateAddress(AddressType addressType, string country, string region, string city, string street, int buildingNumber)
        {
            try
            {
                await _serviceManager.AddressService.CreateAddressAsync(addressType, country, region, city, street, buildingNumber);

                return CreatedAtRoute("CreateContact", street, buildingNumber);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"CreateAddress error: {ex.Message}");
            }
        }
    }
}
