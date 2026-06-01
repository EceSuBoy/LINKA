using Linka.DtoLayer.CatalogDtos.ProductImageDtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Linka.WebUI.Services.CatalogServices.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultProductImageDto>>
            GetAllProductImageAsync()
        {
            var responseMessage =
                await _httpClient.GetAsync("productimages");

            responseMessage.EnsureSuccessStatusCode();

            var jsonData =
                await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert
                       .DeserializeObject<List<ResultProductImageDto>>(
                           jsonData)
                   ?? new List<ResultProductImageDto>();
        }

        public async Task<GetByIdProductImageDto?>
            GetByIdProductImageAsync(string id)
        {
            var responseMessage =
                await _httpClient.GetAsync("productimages/" + id);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound ||
                responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content
                .ReadFromJsonAsync<GetByIdProductImageDto>();
        }

        public async Task<GetByIdProductImageDto?>
            GetByProductIdProductImageAsync(string productId)
        {
            var responseMessage = await _httpClient.GetAsync(
                "productimages/ProductImagesByProductId/" + productId);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound ||
                responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var jsonData =
                await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Product image request failed: " +
                    $"{responseMessage.StatusCode} - {jsonData}");
            }

            if (string.IsNullOrWhiteSpace(jsonData) ||
                jsonData.Trim() == "null")
            {
                return null;
            }

            var values =
                JsonConvert.DeserializeObject<GetByIdProductImageDto>(
                    jsonData);

            if (values == null ||
                string.IsNullOrWhiteSpace(values.ProductImageId))
            {
                return null;
            }

            values.Images ??= new List<string>();

            return values;
        }

        public async Task CreateProductImageAsync(
            CreateProductImageDto createProductImageDto)
        {
            var responseMessage =
                await _httpClient.PostAsJsonAsync(
                    "productimages",
                    createProductImageDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Product image creation failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }

        public async Task UpdateProductImageAsync(
            UpdateProductImageDto updateProductImageDto)
        {
            var responseMessage =
                await _httpClient.PutAsJsonAsync(
                    "productimages",
                    updateProductImageDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Product image update failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }

        public async Task DeleteProductImageAsync(string id)
        {
            var responseMessage =
                await _httpClient.DeleteAsync("productimages/" + id);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Product image deletion failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }
    }
}