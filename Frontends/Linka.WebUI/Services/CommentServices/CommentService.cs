using Linka.DtoLayer.CommentDtos;
using Newtonsoft.Json;

namespace Linka.WebUI.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCommentDto>> CommentListByProductId(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Comments/CommentListByProductId/" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
            return values;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            var responseMessage =
        await _httpClient.PostAsJsonAsync(
            "Comments",
            createCommentDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Comment creation failed: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _httpClient.DeleteAsync("Comments/" + id);
        }

        public async Task<int> GetActiveCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("Comments/GetActiveCommentCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Comments");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
            return values;
        }
        //dikkat
        public async Task<UpdateCommentDto> GetByIdCommentAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Comments/" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateCommentDto>();
            return values;
        }

        public async Task<int> GetPassiveCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("Comments/GetPassiveCommentCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }

        public async Task<int> GetTotalCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("Comments/GetTotalCommentCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCommentDto>("Comments", updateCommentDto);
        }

        public async Task<List<ProductCommentStatisticDto>>
    GetAllProductCommentStatisticsAsync()
        {
            var responseMessage =
                await _httpClient.GetAsync(
                    "Comments/GetAllProductCommentStatistics");

            var jsonData =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Product comment statistics could not be loaded: " +
                    $"{responseMessage.StatusCode} - {jsonData}");
            }

            var values =
                JsonConvert.DeserializeObject<
                    List<ProductCommentStatisticDto>>(
                        jsonData);

            return values ??
                new List<ProductCommentStatisticDto>();
        }

        public async Task<ProductCommentStatisticDto>
    GetProductCommentStatisticsAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException(
                    "Product ID cannot be empty.");
            }

            var responseMessage =
                await _httpClient.GetAsync(
                    "Comments/GetProductCommentStatistics/" +
                    Uri.EscapeDataString(productId));

            var jsonData =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Product comment statistics could not be loaded: " +
                    $"{responseMessage.StatusCode} - {jsonData}");
            }

            var value =
                JsonConvert.DeserializeObject<
                    ProductCommentStatisticDto>(jsonData);

            return value ??
                new ProductCommentStatisticDto
                {
                    ProductId = productId,
                    CommentCount = 0,
                    AverageRating = 0
                };
        }

    }
}
