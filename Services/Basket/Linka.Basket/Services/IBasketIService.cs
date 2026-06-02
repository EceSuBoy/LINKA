using Linka.Basket.Dtos;

namespace Linka.Basket.Services
{
    public interface IBasketIService
    {
        Task<BasketTotalDto> GetBasket(string userId);
        Task SaveBasket(BasketTotalDto basketTotalDto);
        Task DeleteBasket(string userId);

    }
}
