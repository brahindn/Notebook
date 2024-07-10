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
        public Task<IActionResult> AddAddress([FromBody] AddressForCreateDTO address)
        {
            var routingKey = "AddKey";

            _messageProducer.SendMessage(address, routingKey);

            _logger.Information($"New address for contact {address.ContactId} has been added successfully");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("{addressId}")]
        public Task<IActionResult> UpdateAddress([FromBody] AddressForUpdateDTO address)
        {
            var routingKey = "UpdateKey";

            _messageProducer.SendMessage(address, routingKey);

            _logger.Information($"Updated address: {address.Id}");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var existAddress = await _serviceManager.AddressService.GetAddressAsync(addressId);

            var routingKey = "DeleteKey";

            _messageProducer.SendMessage(existAddress, routingKey);

            _logger.Information($"Deleted address: {existAddress.Id}");

            return Ok();
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressById(Guid addressId)
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

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses([FromQuery] AddressParameters addressParameters)
        {
            var addresses = await _serviceManager.AddressService.GetAllAddressesAsync(addressParameters);

            _logger.Information("All addresses have been got");

            return Ok(addresses);
        }
    }
}
