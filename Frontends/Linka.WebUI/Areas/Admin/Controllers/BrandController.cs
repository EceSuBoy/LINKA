using Linka.DtoLayer.CatalogDtos.BrandDtos;
using Linka.WebUI.Services.CatalogServices.BrandServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Brand")]
    public class BrandController : AdminControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Brands";
            ViewBag.v3 = "Brand List";
            ViewBag.v0 = "Brand Operations";

            var values = await _brandService.GetAllBrandAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateBrand")]
        public IActionResult CreateBrand()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Brand";
            ViewBag.v3 = "Add New Brand";
            ViewBag.v0 = "Brand Operations";
            return View();
        }

        [HttpPost]
        [Route("CreateBrand")]
        public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto)
        {
            await _brandService.CreateBrandAsync(createBrandDto);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }
        [Route("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }
        [Route("UpdateBrand/{id}")]
        public async Task<IActionResult> UpdateBrand(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Brand";
            ViewBag.v3 = "Update Brand";
            ViewBag.v0 = "Brand Operations";

            var values = await _brandService.GetByIdBrandAsync(id);
            var updateCategoryDto = new UpdateBrandDto
            {
                BrandId = values.BrandId,
                BrandName = values.BrandName,
                ImageUrl = values.ImageUrl
            };
            return View(updateCategoryDto);
        }
        [HttpPost]
        [Route("UpdateBrand/{id}")]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto)
        {
            await _brandService.UpdateBrandAsync(updateBrandDto);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }
    }
}

