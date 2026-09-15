using Linka.DtoLayer.CatalogDtos.CategoryDtos;
using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.CategoryServices;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Linka.WebUI.Services.ImageUploadServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    public class ProductController : AdminControllerBase
    {

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageUploadService _imageUploadService;

        public ProductController(IProductService productService, ICategoryService categoryService, IImageUploadService imageUploadService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageUploadService = imageUploadService;
        }

        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product List";
            ViewBag.v0 = "Product Operations";

            var values = await _productService.GetAllProductAsync();
            return View(values);
        }

        [HttpGet]
        [Route("ProductListWithCategory")]
        public async Task<IActionResult>
    ProductListWithCategory()
        {
            ViewBag.v1 =
                "Home";

            ViewBag.v2 =
                "Products";

            ViewBag.v3 =
                "Product List";

            ViewBag.v0 =
                "Product Operations";

            var categories =
                await _categoryService
                    .GetAllCategoriesAsync();

            var products =
                await _productService
                    .GetPagedProductsWithCategoryAsync(
                        search:
                            string.Empty,

                        categoryId:
                            string.Empty,

                        page:
                            1,

                        pageSize:
                            10);

            var model =
                new AdminProductListViewModel
                {
                    Categories =
                        categories,

                    Products =
                        products
                };

            return View(model);
        }

        [Route("CreateProduct")]
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product List";
            ViewBag.v0 = "Product Operations";

            await LoadCategoriesAsync();

            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult>
    CreateProduct(
        CreateProductDto createProductDto,
        IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(createProductDto.CategoryId);

                return View(
                    createProductDto);
            }

            try
            {
                if (imageFile != null &&
    imageFile.Length > 0)
                {
                    var uploadedUrl =
                        await _imageUploadService
                            .UploadAsync(imageFile);

                    if (!string.IsNullOrWhiteSpace(uploadedUrl))
                    {
                        createProductDto.ProductImageUrl =
                            uploadedUrl;
                    }
                }

                await _productService
                    .CreateProductAsync(
                        createProductDto);

                TempData["ProductSuccess"] =
                    "Product created successfully.";

                return RedirectToAction(
                    nameof(
                        ProductListWithCategory));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    exception.Message);

                await LoadCategoriesAsync(createProductDto.CategoryId);

                return View(createProductDto);
            }
        }
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        [Route("UpdateProduct/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product";
            ViewBag.v3 = "Update Product";
            ViewBag.v0 = "Product Operations";

            var productValues =
                await _productService
                    .GetByIdProductAsync(id);

            await LoadCategoriesAsync(productValues.CategoryId);

            return View(productValues);
        }

        [HttpPost]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult>
    UpdateProduct(
        string id,
        UpdateProductDto updateProductDto,
        IFormFile? imageFile)
        {
            updateProductDto.ProductId =
                id;

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(updateProductDto.CategoryId);

                return View(
                    updateProductDto);
            }

            try
            {
                if (imageFile != null &&
    imageFile.Length > 0)
                {
                    var uploadedUrl =
                        await _imageUploadService
                            .UploadAsync(imageFile);

                    if (!string.IsNullOrWhiteSpace(uploadedUrl))
                    {
                        updateProductDto.ProductImageUrl =
                            uploadedUrl;
                    }
                }

                await _productService
                    .UpdateProductAsync(
                        updateProductDto);

                TempData["ProductSuccess"] =
                    "Product updated successfully.";

                return RedirectToAction(
                    nameof(
                        ProductListWithCategory));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    exception.Message);

                await LoadCategoriesAsync(updateProductDto.CategoryId);

                return View(updateProductDto);
            }
        }

        [HttpGet]
        [Route("ProductTable")]
        public async Task<IActionResult>
    ProductTable(
        string? search,
        string? categoryId,
        int page = 1,
        int pageSize = 10)
        {
            var values =
                await _productService
                    .GetPagedProductsWithCategoryAsync(
                        search,
                        categoryId,
                        page,
                        pageSize);

            return PartialView(
                "_ProductTablePartial",
                values);
        }

        private async Task LoadCategoriesAsync(string? selectedCategoryId = null)
        {
            var categories =
                await _categoryService
                    .GetAllCategoriesAsync();

            List<SelectListItem> categoryValues =
                categories
                    .Select(c => new SelectListItem
                    {
                        Text =
                            c.CategoryName,

                        Value =
                            c.CategoryId,

                        Selected =
                            c.CategoryId == selectedCategoryId
                    })
                    .ToList();

            ViewBag.CategoryValues =
                categoryValues;
        }
    }
}
