using Linka.Catalog.Dtos.ProductDtos;
using Linka.Catalog.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Catalog.Controllers
{
    [AllowAnonymous]
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
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await _ProductService.CreateProductAsync(createProductDto);
            return Ok("Product successfully added");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _ProductService.DeleteProductAsync(id);
            return Ok("Product successfully deleted");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _ProductService.UpdateProductAsync(updateProductDto);
            return Ok("Product successfully updated");
        }
    }
}
