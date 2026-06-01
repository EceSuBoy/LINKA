using Linka.DtoLayer.CatalogDtos.ProductDetailDtos;
using Newtonsoft.Json;
using System.Net;

namespace Linka.WebUI.Services.CatalogServices.ProductDetailServices
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly HttpClient _httpClient;

        public ProductDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultProductDetailDto>> GetAllProductDetailAsync()
        {
            var responseMessage =
                await _httpClient.GetAsync("productdetails");

            responseMessage.EnsureSuccessStatusCode();

            var jsonData =
                await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert
                       .DeserializeObject<List<ResultProductDetailDto>>(jsonData)
                   ?? new List<ResultProductDetailDto>();
        }

        public async Task<GetByIdProductDetailDto?> GetByIdProductDetailAsync(
            string id)
        {
            var responseMessage =
                await _httpClient.GetAsync("productdetails/" + id);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound ||
                responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content
                .ReadFromJsonAsync<GetByIdProductDetailDto>();
        }

        public async Task<GetByIdProductDetailDto?> GetByProductIdProductDetailAsync(
            string productId)
        {
            var responseMessage = await _httpClient.GetAsync(
                "productdetails/GetProductDetailByProductId/" + productId);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound ||
                responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var content =
                await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Product detail request failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }

            if (string.IsNullOrWhiteSpace(content) ||
                content.Trim() == "null")
            {
                return null;
            }

            return JsonConvert
                .DeserializeObject<GetByIdProductDetailDto>(content);
        }

        public async Task CreateProductDetailAsync(
            CreateProductDetailDto createProductDetailDto)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(
                "productdetails",
                createProductDetailDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Product detail creation failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }

        public async Task UpdateProductDetailAsync(
            UpdateProductDetailDto updateProductDetailDto)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync(
                "productdetails",
                updateProductDetailDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Product detail update failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }

        public async Task DeleteProductDetailAsync(string id)
        {
            var responseMessage =
                await _httpClient.DeleteAsync("productdetails/" + id);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Product detail deletion failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }
    }
}
