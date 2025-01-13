using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Shared.RequestFeatures;
using Notebook.WebApi.RabbitMQ;
using Notebook.Domain.Requests;
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

        [HttpPost("add")]
        public Task<IActionResult> AddAddress([FromBody] CreateAddressRequest address)
        {
            var routingKey = "AddAddressKey";

            _messageProducer.SendMessage(address, routingKey);

            _logger.Information($"New address for contact {address.ContactId} has been added successfully");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("update")]
        public Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest address)
        {
            var routingKey = "UpdateAddressKey";

            _messageProducer.SendMessage(address, routingKey);

            _logger.Information($"Updated address: {address.Id}");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var existAddress = await _serviceManager.AddressService.GetAddressByIdAsync(addressId);

            var routingKey = "DeleteAddressKey";

            _messageProducer.SendMessage(existAddress, routingKey);

            _logger.Information($"Deleted address: {existAddress.Id}");

            return Ok();
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressById(Guid addressId)
        {

            var address = await _serviceManager.AddressService.GetAddressByIdAsync(addressId);

            if(address == null)
            {
                return NotFound();
            }

            var addressDTO = _mapper.Map<CreateAddressRequest>(address);

            _logger.Information($"Address for contact {address.ContactId} has been got");

            return Ok(addressDTO);
        }

        [HttpGet("getByFields")]
        public async Task<IActionResult> GetAddressByFields([FromBody]GetAddressRequest getAddressRequest)
        {
            if(getAddressRequest == null)
            {
                return BadRequest();
            }

            var allAddresses = await _serviceManager.AddressService.GetAddressByFieldsAsync(getAddressRequest);

            _logger.Information("Necessery addresses received");

            return Ok(allAddresses);
        }

        [HttpGet("allAddresses")]
        public async Task<IActionResult> GetAllAddresses([FromQuery] AddressParameters addressParameters)
        {
            var addresses = await _serviceManager.AddressService.GetAllAddressesAsync(addressParameters);

            _logger.Information("All addresses have been got");

            return Ok(addresses);
        }
    }
}
