using Linka.DtoLayer.CatalogDtos.AboutDtos;
using Linka.WebUI.Services.CatalogServices.AboutServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/About")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "About";
            ViewBag.v3 = "About List";
            ViewBag.v0 = "About Operations";

            var values = await _aboutService.GetAllAboutAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateAbout")]
        public IActionResult CreateAbout()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "About";
            ViewBag.v3 = "Add New About";
            ViewBag.v0 = "About Operations";
            return View();
        }

        [HttpPost]
        [Route("CreateAbout")]
        public async Task<IActionResult> CreateAbout(CreateAboutDto createAboutDto)
        {
            await _aboutService.CreateAboutAsync(createAboutDto);
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }
        [Route("DeleteAbout/{id}")]
        public async Task<IActionResult> DeleteAbout(string id)
        {
            await _aboutService.DeleteAboutAsync(id);
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }
        [Route("UpdateAbout/{id}")]
        public async Task<IActionResult> UpdateAbout(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "About";
            ViewBag.v3 = "Update About";
            ViewBag.v0 = "About Operations";

            var values = await _aboutService.GetByIdAboutAsync(id);
            var updateCategoryDto = new UpdateAboutDto
            {
                AboutId = values.AboutId,
                Description = values.Description,
                Address = values.Address,
                Email = values.Email,
                Phone = values.Phone,       
            };
            return View(updateCategoryDto);
        }
        [HttpPost]
        [Route("UpdateAbout/{id}")]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            await _aboutService.UpdateAboutAsync(updateAboutDto);
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }
    }
}
