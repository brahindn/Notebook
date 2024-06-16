using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Shared.RequestFeatures;
using Notebook.WebApi.RabbitMQ;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;
using AutoMapper;

namespace Notebook.WebApi.Controllers
{
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly Serilog.ILogger _logger;
        private readonly MessageProducer _messageProducer;
        private readonly IMapper _mapper;

        public AddressController(IServiceManager serviceManager, Serilog.ILogger logger, MessageProducer messageProducer, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _messageProducer = messageProducer;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressForCreateDTO address)
        {
            try
            {
                var routingKey = "AddKey";

                _messageProducer.SendMessage(address, routingKey);

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
        public async Task<IActionResult> UpdateAddress([FromBody] AddressForUpdateDTO address)
        {
            try
            {
                var routingKey = "UpdateKey";

                _messageProducer.SendMessage(address, routingKey);

                _logger.Information($"Updated address: {address.Id}");

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

            try
            {
                var routingKey = "DeleteKey";

                _messageProducer.SendMessage(existAddress, routingKey);

                _logger.Information($"Deleted address: {existAddress.Id}");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"DeleteAddress error: {ex.Message}");
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

                var addressDTO = new AddressResponseDTO()
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
        public async Task<IActionResult> GetAllAddresses([FromQuery] AddressParameters addressParameters)
        {
            try
            {
                var addresses = await _serviceManager.AddressService.GetAllAddressesAsync(addressParameters);

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
