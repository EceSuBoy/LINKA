using Linka.DtoLayer.BasketDtos;

namespace Linka.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            var values = await GetBasket() ?? new BasketTotalDto();
            values.BasketItems ??= new List<BasketItemDto>();

            var existing = values.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);
            if (existing == null)
                values.BasketItems.Add(basketItemDto);
            else
                existing.Quantity += basketItemDto.Quantity;

            await SaveBasket(values);
        }

        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BasketTotalDto> GetBasket()
        {
            var responseMessage = await _httpClient.GetAsync("baskets");

            if (!responseMessage.IsSuccessStatusCode)
                return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };

            var content = await responseMessage.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content) || !content.TrimStart().StartsWith("{"))
                return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };

            var values = System.Text.Json.JsonSerializer.Deserialize<BasketTotalDto>(content,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (values != null && values.BasketItems == null)
                values.BasketItems = new List<BasketItemDto>();

            return values ?? new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            var values = await GetBasket();
            var deletedItem=values.BasketItems.FirstOrDefault(x=>x.ProductId == productId);
            var result = values.BasketItems.Remove(deletedItem);
            await SaveBasket(values);
            return true;
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("baskets", basketTotalDto);
            var content = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Basket save failed: {responseMessage.StatusCode} - {content}");
            }
        }
    }
}
