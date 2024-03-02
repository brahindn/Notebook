using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.WebApi.Responses;

namespace Notebook.WebApi.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly Serilog.ILogger _logger;
        public LogsController(IServiceManager serviceManager, Serilog.ILogger logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                var logsCollection = await _serviceManager.MongoService.GetDataFromMongoDB();

                var logsColleectionDTO = logsCollection.Select(logs => new LogsResponseDTO
                {
                    Id = logs["_id"]?.AsObjectId.ToString(),
                    Level = logs["Level"]?.AsString,
                    UtcTimesTamp = logs["UtcTimestamp"]?.AsString,
                    Message = logs["MessageTemplate"]?.AsString
                }).ToList();

                _logger.Information("All logs have been got.");

                return Ok(logsColleectionDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"GetAllLogs error: {ex.Message}");
            }
        }
    }
}
