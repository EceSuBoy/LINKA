using Linka.Catalog.Dtos.ProductDtos;
using Linka.Catalog.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductsController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }
        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var values = await _ProductService.GetAllProductAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var values = await _ProductService.GetByIdProductAsync(id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult>
    CreateProduct(
        CreateProductDto createProductDto)
        {
            try
            {
                await _ProductService
                    .CreateProductAsync(
                        createProductDto);

                return Ok(
                    "Product successfully added.");
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _ProductService.DeleteProductAsync(id);
            return Ok("Product successfully deleted");
        }

        [HttpPut]
        public async Task<IActionResult>
    UpdateProduct(
        UpdateProductDto updateProductDto)
        {
            try
            {
                await _ProductService
                    .UpdateProductAsync(
                        updateProductDto);

                return Ok(
                    "Product successfully updated.");
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    exception.Message);
            }
        }
        [HttpGet("ProductListWithCategory")]
        public async Task<IActionResult> ProductListWithCategory()
        {
            var values = await _ProductService.GetProductsWithCategoryAsync();
            return Ok(values);

        }
        [HttpGet("ProductListWithCategoryByCategoryId/{id}")]
        public async Task<IActionResult> ProductListWithCategoryByCategoryId(string id)
        {
            var values = await _ProductService.GetProductsWithCategoryByCategoryIdAsync(id);
            return Ok(values);

        }

        [HttpGet(
    "GetPagedProductsWithCategory")]
        public async Task<IActionResult>
    GetPagedProductsWithCategory(
        [FromQuery]
        string? search,

        [FromQuery]
        string? categoryId,

        [FromQuery]
        int page = 1,

        [FromQuery]
        int pageSize = 10)
        {
            var values =
                await _ProductService
                    .GetPagedProductsWithCategoryAsync(
                        search,
                        categoryId,
                        page,
                        pageSize);

            return Ok(
                values);
        }

        [HttpGet("GetFeaturedProducts")]
        public async Task<IActionResult>
    GetFeaturedProducts()
        {
            var values =
                await _ProductService
                    .GetFeaturedProductsAsync();

            return Ok(values);
        }
    }
}
