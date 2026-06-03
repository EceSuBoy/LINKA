using Linka.DtoLayer.CatalogDtos.FeatureDtos;
using Linka.WebUI.Services.CatalogServices.FeatureServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Feature")]
    public class FeatureController : AdminControllerBase
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Features";
            ViewBag.v3 = "Features List";
            ViewBag.v0 = "Feature Operations";

            var values = await _featureService.GetAllFeatureAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateFeature")]
        public IActionResult CreateFeature()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Features";
            ViewBag.v3 = "Add New Feature";
            ViewBag.v0 = "Feature Operations";
            return View();
        }

        [HttpPost]
        [Route("CreateFeature")]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto createFeatureDto)
        {
            await _featureService.CreateFeatureAsync(createFeatureDto);
            return RedirectToAction("Index", "Feature", new { area = "Admin" });
        }
        [Route("DeleteFeature/{id}")]
        public async Task<IActionResult> DeleteFeature(string id)
        {
            await _featureService.DeleteFeatureAsync(id);
            return RedirectToAction("Index", "Feature", new { area = "Admin" });
        }
        [Route("UpdateFeature/{id}")]
        public async Task<IActionResult> UpdateFeature(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Features";
            ViewBag.v3 = "Update Feature";
            ViewBag.v0 = "Feature Operations";

            var values = await _featureService.GetByIdFeatureAsync(id);
            var updateCategoryDto = new UpdateFeatureDto
            {
                FeatureId = values.FeatureId,
                FeatureName = values.FeatureName,
                Icon = values.Icon
            };
            return View(updateCategoryDto);
        }
        [HttpPost]
        [Route("UpdateFeature/{id}")]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            await _featureService.UpdateFeatureAsync(updateFeatureDto);
            return RedirectToAction("Index", "Feature", new { area = "Admin" });
        }
    }
}

