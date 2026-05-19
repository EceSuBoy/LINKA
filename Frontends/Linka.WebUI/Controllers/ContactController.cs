using Linka.DtoLayer.CatalogDtos.ContactDtos;
using Linka.WebUI.Services.CatalogServices.ContactServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            @ViewBag.directory1 = "Home Page";
            @ViewBag.directory3 = "Contacts";
            @ViewBag.directory3 = "Send Message";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateContactDto createContactDto)
        {
            createContactDto.IsRead = "False";
            createContactDto.SendDate = DateTime.Now;
            await _contactService.CreateContactAsync(createContactDto);
            return RedirectToAction("Index", "Default", new { area = "Admin" });
        }
    }
}
