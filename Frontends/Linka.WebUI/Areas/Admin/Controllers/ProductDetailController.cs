using Linka.DtoLayer.CatalogDtos.ProductDetailDtos;
using Linka.WebUI.Services.CatalogServices.ProductDetailServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ProductDetail")]
    public class ProductDetailController : AdminControllerBase
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailController(
            IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet]
        [Route("ProductDetailOperation/{id}")]
        public async Task<IActionResult> ProductDetailOperation(string id)
        {
            var existingDetail =
                await _productDetailService
                    .GetByProductIdProductDetailAsync(id);

            if (existingDetail == null)
            {
                return RedirectToAction(
                    "CreateProductDetail",
                    "ProductDetail",
                    new { area = "Admin", id });
            }

            return RedirectToAction(
                "UpdateProductDetail",
                "ProductDetail",
                new { area = "Admin", id });
        }

        [HttpGet]
        [Route("CreateProductDetail/{id}")]
        public IActionResult CreateProductDetail(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Create Product Description and Info Page";
            ViewBag.v0 = "Product Operations";

            var model = new CreateProductDetailDto
            {
                ProductId = id
            };

            return View(model);
        }

        [HttpPost]
        [Route("CreateProductDetail/{id}")]
        public async Task<IActionResult> CreateProductDetail(
            string id,
            CreateProductDetailDto createProductDetailDto)
        {
            createProductDetailDto.ProductId = id;

            await _productDetailService
                .CreateProductDetailAsync(createProductDetailDto);

            return RedirectToAction(
                "ProductListWithCategory",
                "Product",
                new { area = "Admin" });
        }

        [HttpGet]
        [Route("UpdateProductDetail/{id}")]
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product Description and Info Page";
            ViewBag.v0 = "Product Operations";

            var existingDetail =
                await _productDetailService
                    .GetByProductIdProductDetailAsync(id);

            if (existingDetail == null)
            {
                return RedirectToAction(
                    "CreateProductDetail",
                    "ProductDetail",
                    new { area = "Admin", id });
            }

            var model = new UpdateProductDetailDto
            {
                ProductDetailId = existingDetail.ProductDetailId,
                ProductId = existingDetail.ProductId,
                ProductDescription = existingDetail.ProductDescription,
                ProductInfo = existingDetail.ProductInfo
            };

            return View(model);
        }

        [HttpPost]
        [Route("UpdateProductDetail/{id}")]
        public async Task<IActionResult> UpdateProductDetail(
            string id,
            UpdateProductDetailDto updateProductDetailDto)
        {
            updateProductDetailDto.ProductId = id;

            await _productDetailService
                .UpdateProductDetailAsync(updateProductDetailDto);

            return RedirectToAction(
                "ProductListWithCategory",
                "Product",
                new { area = "Admin" });
        }
    }
}