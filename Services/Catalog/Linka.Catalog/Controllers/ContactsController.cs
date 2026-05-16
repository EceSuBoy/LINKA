using Linka.Catalog.Dtos.ContactDtos;
using Linka.Catalog.Services.ContactServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Catalog.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _ContactService;

        public ContactsController(IContactService ContactService)
        {
            _ContactService = ContactService;
        }
        [HttpGet]
        public async Task<IActionResult> ContactList()
        {
            var values = await _ContactService.GetAllContactAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(string id)
        {
            var values = await _ContactService.GetByIdContactAsync(id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto createContactDto)
        {
            await _ContactService.CreateContactAsync(createContactDto);
            return Ok("Contact successfully added.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            await _ContactService.DeleteContactAsync(id);
            return Ok("Contact successfully deleted.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContact(UpdateContactDto updateContactDto)
        {
            await _ContactService.UpdateContactAsync(updateContactDto);
            return Ok("Contact successfully updated.");
        }
    }
}

