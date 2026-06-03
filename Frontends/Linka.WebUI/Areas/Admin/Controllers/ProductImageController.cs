using Linka.DtoLayer.CatalogDtos.ProductImageDtos;
using Linka.WebUI.Services.CatalogServices.ProductImageServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ProductImage")]
    public class ProductImageController : AdminControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(
            IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        // Product image var mı diye kontrol eden ara action
        [HttpGet]
        [Route("ProductImageOperation/{id}")]
        public async Task<IActionResult> ProductImageOperation(string id)
        {
            var existingImage =
                await _productImageService
                    .GetByProductIdProductImageAsync(id);

            if (existingImage == null)
            {
                return RedirectToAction(
                    "CreateProductImageDetail",
                    "ProductImage",
                    new { area = "Admin", id });
            }

            return RedirectToAction(
                "ProductImageDetail",
                "ProductImage",
                new { area = "Admin", id });
        }

        // Create sayfası
        [HttpGet]
        [Route("CreateProductImageDetail/{id}")]
        public IActionResult CreateProductImageDetail(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Add Product Images";
            ViewBag.v0 = "Product Image Operations";

            var model = new CreateProductImageDto
            {
                ProductId = id,
                Images = new List<string>()
            };

            return View(model);
        }

        // Yeni image kaydı oluştur
        [HttpPost]
        [Route("CreateProductImageDetail/{id}")]
        public async Task<IActionResult> CreateProductImageDetail(
            string id,
            CreateProductImageDto createProductImageDto)
        {
            createProductImageDto.ProductId = id;

            createProductImageDto.Images =
                createProductImageDto.Images?
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Take(4)
                    .ToList()
                ?? new List<string>();

            var existingImage =
                await _productImageService
                    .GetByProductIdProductImageAsync(id);

            if (existingImage != null)
            {
                return RedirectToAction(
                    "ProductImageDetail",
                    "ProductImage",
                    new { area = "Admin", id });
            }

            await _productImageService
                .CreateProductImageAsync(createProductImageDto);

            return RedirectToAction(
                "ProductListWithCategory",
                "Product",
                new { area = "Admin" });
        }

        // Update sayfası
        [HttpGet]
        [Route("ProductImageDetail/{id}")]
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product Image Page";
            ViewBag.v0 = "Product Image Operations";

            var existingImage =
                await _productImageService
                    .GetByProductIdProductImageAsync(id);

            if (existingImage == null)
            {
                return RedirectToAction(
                    "CreateProductImageDetail",
                    "ProductImage",
                    new { area = "Admin", id });
            }

            var model = new UpdateProductImageDto
            {
                ProductImageId = existingImage.ProductImageId,
                ProductId = existingImage.ProductId,
                Images = existingImage.Images ?? new List<string>()
            };

            return View(model);
        }

        // Mevcut image kaydını güncelle
        [HttpPost]
        [Route("ProductImageDetail/{id}")]
        public async Task<IActionResult> ProductImageDetail(
            string id,
            UpdateProductImageDto updateProductImageDto)
        {
            updateProductImageDto.ProductId = id;

            updateProductImageDto.Images =
                updateProductImageDto.Images?
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Take(4)
                    .ToList()
                ?? new List<string>();

            await _productImageService
                .UpdateProductImageAsync(updateProductImageDto);

            return RedirectToAction(
                "ProductListWithCategory",
                "Product",
                new { area = "Admin" });
        }
    }
}