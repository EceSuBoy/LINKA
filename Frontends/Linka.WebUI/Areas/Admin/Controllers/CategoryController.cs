using Linka.DtoLayer.CatalogDtos.CategoryDtos;
using Linka.WebUI.Services.CatalogServices.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICategoryService _categoryService;

        public CategoryController(IHttpClientFactory httpClientFactory, ICategoryService categoryService)
        {
            _httpClientFactory = httpClientFactory;
            _categoryService = categoryService;
        }
        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Category";
            ViewBag.v3 = "Category List";
            ViewBag.v0 = "Category Operations";

            var values = await _categoryService.GetAllCategoriesAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateCategory")]
        public IActionResult CreateCategory()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Category";
            ViewBag.v3 = "Add New Category";
            ViewBag.v0 = "Category Operations";
            return View();
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index", "Category", new { area = "Admin" });

        }
        [Route("UpdateCategory/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Category";
            ViewBag.v3 = "Update Category";
            ViewBag.v0 = "Category Operations";
            var values = await _categoryService.GetByIdCategoryAsync(id);
            var updateCategoryDto = new UpdateCategoryDto
            {
                CategoryId = values.CategoryId,
                CategoryName = values.CategoryName,
                ImageUrl=values.ImageUrl
            };
            return View(updateCategoryDto);
        }
        [HttpPost]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
    }
}
