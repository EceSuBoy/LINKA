using Linka.DtoLayer.CatalogDtos.ProductImageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/ProductImage")]
    public class ProductImageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductImageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("ProductImageDetail/{id}")]
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product Image Page";
            ViewBag.v0 = "Product Image Operations";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/ProductImages/ProductImagesByProductId?id=" + id);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(jsonData) && jsonData != "null")
                {
                    var values = JsonConvert.DeserializeObject<UpdateProductImageDto>(jsonData);

                    if (values != null)
                    {
                        values.ProductId = id;
                        values.Images ??= new List<string>();
                        return View(values);
                    }
                }
            }

            var emptyModel = new UpdateProductImageDto
            {
                ProductId = id,
                Images = new List<string>()
            };

            return View(emptyModel);
        }

        [HttpPost]
        [Route("ProductImageDetail/{id}")]
        public async Task<IActionResult> ProductImageDetail(UpdateProductImageDto updateProductImageDto)
        {
            updateProductImageDto.Images = updateProductImageDto.Images
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Take(4)
                .ToList();

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateProductImageDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:7070/api/ProductImages/", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
            }

            return View(updateProductImageDto);
        }

        [HttpGet]
        [Route("CreateProductImageDetail/{id}")]
        public async Task<IActionResult> CreateProductImageDetail(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Add Product Images";
            ViewBag.v0 = "Product Image Operations";

            var client = _httpClientFactory.CreateClient();
            var checkResponse = await client.GetAsync("https://localhost:7070/api/ProductImages/ProductImagesByProductId?id=" + id);

            if (checkResponse.IsSuccessStatusCode)
            {
                var jsonData = await checkResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(jsonData) && jsonData != "null")
                {
                    var existingImage = JsonConvert.DeserializeObject<UpdateProductImageDto>(jsonData);

                    if (existingImage?.Images != null && existingImage.Images.Count >= 4)
                    {
                        TempData["ImageError"] = "This product already has the maximum number of images.";
                        return RedirectToAction("ProductImageDetail", "ProductImage", new { area = "Admin", id });
                    }
                }
            }

            var model = new CreateProductImageDto
            {
                ProductId = id,
                Images = new List<string>()
            };

            return View(model);
        }

        [HttpPost]
        [Route("CreateProductImageDetail/{id}")]
        public async Task<IActionResult> CreateProductImageDetail(CreateProductImageDto createProductImageDto)
        {
            var client = _httpClientFactory.CreateClient();

            var checkResponse = await client.GetAsync("https://localhost:7070/api/ProductImages/ProductImagesByProductId?id=" + createProductImageDto.ProductId);

            if (checkResponse.IsSuccessStatusCode)
            {
                var existingData = await checkResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(existingData) && existingData != "null")
                {
                    var existingImage = JsonConvert.DeserializeObject<UpdateProductImageDto>(existingData);

                    if (existingImage?.Images != null && existingImage.Images.Count >= 4)
                    {
                        TempData["ImageError"] = "This product already has the maximum number of images.";
                        return RedirectToAction("ProductImageDetail", "ProductImage", new { area = "Admin", id = createProductImageDto.ProductId });
                    }
                }
            }

            createProductImageDto.Images = createProductImageDto.Images
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Take(4)
                .ToList();

            var jsonData = JsonConvert.SerializeObject(createProductImageDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7070/api/ProductImages/", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductImageDetail", "ProductImage", new { area = "Admin", id = createProductImageDto.ProductId });
            }

            return View(createProductImageDto);
        }
    }
}