using Linka.DtoLayer.OrderDtos
    .OrderOrderingDtos;
using Newtonsoft.Json;

namespace Linka.WebUI.Services
    .OrderServices.OrderOrderingServices
{
    public class OrderOrderingServices
        : IOrderOrderingServices
    {
        private readonly HttpClient
            _httpClient;

        public OrderOrderingServices(
            HttpClient httpClient)
        {
            _httpClient =
                httpClient;
        }

        public async Task<List<ResultOrderingByUserIdDto>>
            GetOrderingByUserId(string id)
        {
            var responseMessage =
                await _httpClient.GetAsync(
                    $"orderings/GetOrderingByUserId/{id}");

            var jsonData =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage
                .IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Orders could not be loaded: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{jsonData}");
            }

            var values =
                JsonConvert.DeserializeObject<
                    List<ResultOrderingByUserIdDto>>(
                        jsonData);

            return values ??
                new List<ResultOrderingByUserIdDto>();
        }

        public async Task<int>
            CreateOrderingWithDetailsAsync(
                CreateOrderingWithDetailsDto dto)
        {
            var responseMessage =
                await _httpClient.PostAsJsonAsync(
                    "orderings/CreateOrderingWithDetails",
                    dto);

            var content =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage
                .IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Order could not be created: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{content}");
            }

            if (!int.TryParse(
                    content,
                    out var orderingId))
            {
                throw new Exception(
                    "Order API returned an invalid order ID.");
            }

            return orderingId;
        }

        public async Task<ResultOrderingDetailDto?>
    GetOrderingDetailAsync(
        int orderingId,
        string userId)
        {
            if (orderingId <= 0)
            {
                throw new ArgumentException(
                    "Order ID must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID cannot be empty.");
            }

            var responseMessage =
                await _httpClient.GetAsync(
                    $"orderings/GetOrderingDetail/" +
                    $"{orderingId}/" +
                    $"{Uri.EscapeDataString(userId)}");

            var jsonData =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (responseMessage.StatusCode ==
                System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Order detail could not be loaded: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{jsonData}");
            }

            return JsonConvert
                .DeserializeObject<
                    ResultOrderingDetailDto>(
                        jsonData);
        }

        public async Task<List<ResultOrderingDto>>
    GetAllOrderingAsync()
        {
            var responseMessage =
                await _httpClient.GetAsync(
                    "orderings/GetAllOrdering");

            var content =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Orders could not be loaded: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{content}");
            }

            var values =
                JsonConvert.DeserializeObject<
                    List<ResultOrderingDto>>(
                        content);

            return values ??
                new List<ResultOrderingDto>();
        }

        public async Task UpdateOrderingStatusAsync(
            UpdateOrderingStatusDto dto)
        {
            var responseMessage =
                await _httpClient.PutAsJsonAsync(
                    "orderings/UpdateOrderingStatus",
                    dto);

            var content =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Order status could not be updated: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{content}");
            }
        }
    }
}