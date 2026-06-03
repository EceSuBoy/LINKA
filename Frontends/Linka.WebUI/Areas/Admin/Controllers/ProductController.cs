using Linka.DtoLayer.CatalogDtos.CategoryDtos;
using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.CategoryServices;
using Linka.WebUI.Services.CatalogServices.ProductServices;
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

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
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

            var values = await _categoryService.GetAllCategoriesAsync();
            List<SelectListItem> categoryValues = (from c in values
                                                       select new SelectListItem
                                                       {
                                                           Text = c.CategoryName,
                                                           Value = c.CategoryId.ToString()
                                                       }).ToList();
            ViewBag.CategoryValues = categoryValues;
            return View();
        }
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult>
    CreateProduct(
        CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();

                return View(
                    createProductDto);
            }

            try
            {
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

                await LoadCategoriesAsync();

                return View(
                    createProductDto);
            }
        }
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product";
            ViewBag.v3 = "Update Product";
            ViewBag.v0 = "Product Operations";


            var values = await _categoryService.GetAllCategoriesAsync();
            List<SelectListItem> categoryValues = (from c in values
                                                   select new SelectListItem
                                                   {
                                                       Text = c.CategoryName,
                                                       Value = c.CategoryId.ToString()
                                                   }).ToList();
            ViewBag.CategoryValues = categoryValues;

            var productValues = await _productService.GetByIdProductAsync(id);
            return View(productValues);
        }
        [HttpPost]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult>
    UpdateProduct(
        string id,
        UpdateProductDto updateProductDto)
        {
            updateProductDto.ProductId =
                id;

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();

                return View(
                    updateProductDto);
            }

            try
            {
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

                await LoadCategoriesAsync();

                return View(
                    updateProductDto);
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

        private async Task LoadCategoriesAsync()
        {
            var categories =
                await _categoryService
                    .GetAllCategoriesAsync();

            ViewBag.Categories =
                categories;
        }
    }
}
