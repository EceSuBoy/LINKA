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

        public async Task AddBasketItem(
    BasketItemDto basketItemDto)
        {
            var basket =
                await GetBasket();

            /*
             * Kullanıcı daha önce sepete hiçbir ürün eklemediyse
             * Redis içinde henüz kayıt bulunmayabilir.
             */
            basket ??=
                new BasketTotalDto
                {
                    BasketItems =
                        new List<BasketItemDto>()
                };

            basket.BasketItems ??=
                new List<BasketItemDto>();

            var existingItem =
                basket.BasketItems
                    .FirstOrDefault(x =>
                        x.ProductId ==
                        basketItemDto.ProductId);

            if (existingItem == null)
            {
                basket.BasketItems
                    .Add(basketItemDto);
            }
            else
            {
                /*
                 * Aynı ürün yeniden eklenirse ayrı satır oluşturmak
                 * yerine ürün miktarını artırıyoruz.
                 */
                existingItem.Quantity +=
                    basketItemDto.Quantity;
            }

            await SaveBasket(basket);
        }

        public async Task DeleteBasket(
    string userId)
        {
            /*
             * Basket API kullanıcıyı token üzerinden belirliyor.
             * Bu nedenle ayrıca userId göndermiyoruz.
             */
            await ClearBasket();
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

        public async Task<bool> RemoveBasketItem(
    string productId)
        {
            var basket =
                await GetBasket();

            var deletedItem =
                basket.BasketItems
                    .FirstOrDefault(
                        x => x.ProductId == productId);

            if (deletedItem == null)
            {
                return false;
            }

            basket.BasketItems
                .Remove(deletedItem);

            await SaveBasket(basket);

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

        public async Task<bool> UpdateBasketItemQuantity(
    string productId,
    int quantity)
        {
            var basket =
                await GetBasket();

            var item =
                basket.BasketItems
                    .FirstOrDefault(
                        x => x.ProductId == productId);

            if (item == null)
            {
                return false;
            }

            /*
             * Quantity 0 olursa ürünü sepetten tamamen kaldırıyoruz.
             */
            if (quantity <= 0)
            {
                basket.BasketItems.Remove(item);
            }
            else
            {
                /*
                 * Aşırı yüksek miktar gönderilmesini engelliyoruz.
                 */
                item.Quantity =
                    Math.Clamp(quantity, 1, 99);
            }

            await SaveBasket(basket);

            return true;
        }

        public async Task ClearBasket()
        {
            var responseMessage =
                await _httpClient.DeleteAsync(
                    "baskets");

            var content =
                await responseMessage.Content
                    .ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Basket could not be cleared: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }
    }
}
