using Linka.DtoLayer.CargoDtos.CargoCompanyDtos;
using Linka.WebUI.Services.CargoServices.CargoCompanyServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Cargo")]
    public class CargoController : Controller
    {
        private readonly ICargoCompanyService _cargoCompanyService;

        public CargoController(ICargoCompanyService cargoCompanyService)
        {
            _cargoCompanyService = cargoCompanyService;
        }
        [Route("CargoCompanyList")]
        public async Task<IActionResult> CargoCompanyList()
        {
            var values = await _cargoCompanyService.GetAllCargoCompanyAsync();
            return View(values);
        }
        [Route("CreateCargoCompany")]
        [HttpGet]
        public IActionResult CreateCargoCompany()
        {
            return View();
        }
        [Route("CreateCargoCompany")]
        [HttpPost]
        public async Task<IActionResult> CreateCargoCompany(CreateCargoCompanyDto createCargoCompanyDto)
        {
            await _cargoCompanyService.CreateCargoCompanyAsync(createCargoCompanyDto);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Admin" });
        }

        [Route("DeleteCargoCompany/{id}")]
        public async Task<IActionResult> DeleteCargoCompany(int id)
        {
            await _cargoCompanyService.DeleteCargoCompanyAsync(id);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Admin" });
        }

        [Route("UpdateCargoCompany/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateCargoCompany(int id)
        {
            var values = await _cargoCompanyService.GetByIdCargoCompanyAsync(id);
            return View(values);

        }
        [Route("UpdateCargoCompany/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCargoCompany(UpdateCargoCompanyDto updateCargoCompany)
        {
            await _cargoCompanyService.UpdateCargoCompanyAsync(updateCargoCompany);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Admin" });
        }
    }
}
