using Microsoft.AspNetCore.Mvc;
using ContactManagerWebAPI.Interfaces;
using ContactManagerWebAPI.DTOs;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.Extensions.Logging;

namespace ContactManagerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactManagerController : ControllerBase
    {
        public record UrlQueryParams(int Limit = 20, int Page = 1);

        private readonly IContactManagerService _service;
        private readonly ILogger<ContactManagerController> _logger;

        public ContactManagerController(ILogger<ContactManagerController> logger, IContactManagerService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("Contacts")]
        [ProducesResponseType(typeof(ContactResponseDto), Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), Status400BadRequest)]
        public async Task<IActionResult> GetContactsByPageAsync([FromQuery] UrlQueryParams urlQueryParameters, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _service.GetContactsByPageAsync(urlQueryParameters.Limit, urlQueryParameters.Page, cancellationToken);

            return Ok(data);
        }

        [HttpGet("Contacts/{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"HttpGet id: {id}");
            return $"{id}";
        }

        [HttpPost("InsertContact")]
        public void Post([FromBody] object value)
        {
            _logger.LogInformation($"HttpPost value: {value}");
        }

        [HttpPut("UpdateContact/{id}")]
        public void Put(int id, [FromBody] object value)
        {
            _logger.LogInformation($"HttpPut id: {id} value: {value}");
        }

        [HttpDelete("DeleteContact/{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation($"HttpDelete id: {id}");
        }

    }
}
