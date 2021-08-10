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
        private static ContactDTO ItemToDTO(ContactDTO contactDto) =>
        new ContactDTO
        {
            PersonId = contactDto.PersonId,
            NameId = contactDto.NameId,
            ContactId = contactDto.ContactId,
            Type = contactDto.Type,
            First = contactDto.First,
            Last = contactDto.Last,
            Email = contactDto.Email,
            Birthday = contactDto.Birthday,
            Telephone = contactDto.Telephone
        };

        private readonly IContactManagerService _service;
        private readonly ILogger<ContactManagerController> _logger;

        public ContactManagerController(ILogger<ContactManagerController> logger, IContactManagerService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("Contacts")]
        [ProducesResponseType(typeof(ContactResponseDTO), Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), Status400BadRequest)]
        public async Task<IActionResult> GetContactsByPageAsync([FromQuery] UrlQueryParams urlQueryParameters, CancellationToken cancellationToken)
        {
            var data = await _service.GetContactsByPageAsync(urlQueryParameters.Limit, urlQueryParameters.Page, cancellationToken);
            return Ok(data);
        }

        [HttpGet("Contacts/{id}")]
        public async Task<ActionResult<ContactDTO>> GetContact(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"HttpGet id: {id}");
            var contact = await _service.GetContactAsync(id, cancellationToken);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpPost("InsertContact")]
        public async Task<ActionResult<ContactDTO>> PostContact(ContactDTO contactDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"HttpPost contactDTO.Last: {contactDTO.Last}");
            var contact = await _service.AddContactAsync(contactDTO, cancellationToken);
            
            return CreatedAtAction(nameof(GetContact), new { id = contact.PersonId }, ItemToDTO(contact));
        }
 

        [HttpPut("UpdateContact/{type}/{id}")]
        public async Task<IActionResult> Put(string type, int id, ContactDTO contactDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"HttpPut type: {type} id: {id} First: {contactDTO.PersonId}");

            //if (id != contactDTO.PersonId)
            //{
            //    return BadRequest();
            //}

            var contact = await _service.UpdateContactAsync(type, id, contactDTO, cancellationToken);
            if (contact == null)
            {
                return NotFound();
            }


            return NoContent();
        }

        [HttpDelete("DeleteContact/{type}/{id}")]
        public void Delete(string type, int id)
        {
            _logger.LogInformation($"HttpDelete type: {type} id: {id}");
            _service.DeleteContact(type, id);
        }

    }
}
