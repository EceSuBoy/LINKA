using Linka.Basket.Dtos;
using Linka.Basket.Settings;
using System.Text.Json;

namespace Linka.Basket.Services
{
    public class BasketService : IBasketIService
    {
        private readonly RedisService _redisService;

        public BasketService(
            RedisService redisService)
        {
            _redisService =
                redisService;
        }

        public async Task DeleteBasket(
            string userId)
        {
            await _redisService
                .GetDb()
                .KeyDeleteAsync(userId);
        }

        public async Task<BasketTotalDto> GetBasket(
            string userId)
        {
            var basketJson =
                await _redisService
                    .GetDb()
                    .StringGetAsync(userId);

            /*
             * Kullanıcı ilk defa sepete ürün ekliyorsa
             * Redis içinde henüz basket kaydı bulunmaz.
             */
            if (basketJson.IsNullOrEmpty)
            {
                return new BasketTotalDto
                {
                    UserId =
                        userId,

                    BasketItems =
                        new List<BasketItemDto>()
                };
            }

            var basket =
                JsonSerializer
                    .Deserialize<BasketTotalDto>(
                        basketJson!);

            if (basket == null)
            {
                return new BasketTotalDto
                {
                    UserId =
                        userId,

                    BasketItems =
                        new List<BasketItemDto>()
                };
            }

            basket.UserId =
                userId;

            basket.BasketItems ??=
                new List<BasketItemDto>();

            return basket;
        }

        public async Task SaveBasket(
            BasketTotalDto basketTotalDto)
        {
            if (string.IsNullOrWhiteSpace(
                    basketTotalDto.UserId))
            {
                throw new ArgumentException(
                    "User ID cannot be empty.");
            }

            basketTotalDto.BasketItems ??=
                new List<BasketItemDto>();

            await _redisService
                .GetDb()
                .StringSetAsync(
                    basketTotalDto.UserId,
                    JsonSerializer.Serialize(
                        basketTotalDto));
        }
    }
}