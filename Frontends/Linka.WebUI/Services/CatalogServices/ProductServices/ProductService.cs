using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Newtonsoft.Json;

namespace Linka.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateProductAsync(
    CreateProductDto createProductDto)
        {
            var responseMessage =
                await _httpClient
                    .PostAsJsonAsync(
                        "products",
                        createProductDto);

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    content);
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync("products/" + id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var responseMessage = await _httpClient.GetAsync("products");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
            return values;
        }

        public async Task<UpdateProductDto> GetByIdProductAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("products/" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateProductDto>();
            return values;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var responseMessage = await _httpClient.GetAsync("products/productlistwithcategory");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            return values;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string categoryId)
        {
            var responseMessage = await _httpClient.GetAsync("products/ProductListWithCategoryByCategoryId/" + categoryId);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            return values;
        }

        public async Task DecreaseProductStockAsync(
    DecreaseProductStockDto dto)
        {
            var responseMessage =
                await _httpClient
                    .PutAsJsonAsync(
                        "products/DecreaseProductStock",
                        dto);

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    content);
            }
        }

        public async Task UpdateProductAsync(
    UpdateProductDto updateProductDto)
        {
            var responseMessage =
                await _httpClient
                    .PutAsJsonAsync(
                        "products",
                        updateProductDto);

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    content);
            }
        }


        public async Task<
    PagedProductResultDto<
        ResultProductWithCategoryDto>>
    GetPagedProductsWithCategoryAsync(
        string? search,
        string? categoryId,
        int page,
        int pageSize)
        {
            var encodedSearch =
                Uri.EscapeDataString(
                    search ??
                    string.Empty);

            var encodedCategoryId =
                Uri.EscapeDataString(
                    categoryId ??
                    string.Empty);

            var requestUrl =
                "products/GetPagedProductsWithCategory" +
                $"?search={encodedSearch}" +
                $"&categoryId={encodedCategoryId}" +
                $"&page={page}" +
                $"&pageSize={pageSize}";

            var responseMessage =
                await _httpClient
                    .GetAsync(
                        requestUrl);

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    "Products could not be loaded: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{content}");
            }

            var values =
                System.Text.Json
                    .JsonSerializer
                    .Deserialize<
                        PagedProductResultDto<
                            ResultProductWithCategoryDto>>(
                                content,
                                new System.Text.Json
                                    .JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive =
                                            true
                                });

            return values ??
                new PagedProductResultDto<
                    ResultProductWithCategoryDto>();
        }

        public async Task<List<ResultProductDto>>
    GetFeaturedProductsAsync()
        {
            var responseMessage =
                await _httpClient
                    .GetAsync(
                        "products/GetFeaturedProducts");

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    "Featured products could not be loaded: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{content}");
            }

            return System.Text.Json
                    .JsonSerializer
                    .Deserialize<
                        List<ResultProductDto>>(
                            content,
                            new System.Text.Json
                                .JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive =
                                        true
                            })
                ?? new List<ResultProductDto>();
        }

    }
}
