using Linka.Basket.Dtos;
using Linka.Basket.Settings;
using System.Text.Json;

namespace Linka.Basket.Services
{
    public class BasketService : IBasketIService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task DeleteBasket(string userId)
        {
            var status= await _redisService.GetDb().KeyDeleteAsync(userId);

        }

        public async Task<BasketTotalDto> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId)!;
            return (JsonSerializer.Deserialize<BasketTotalDto>(existBasket!))!;

        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            await _redisService.GetDb().StringSetAsync(basketTotalDto.UserId, JsonSerializer.Serialize(basketTotalDto));
        }

    }
}
