using Linka.Discount.Dtos;

namespace Linka.Discount.Services
{
    public interface IDiscountService
    {
        Task<List<ResultCouponDto>> GetAllCouponAsync();
        Task CreateCouponAsync(CreateCouponDto createCouponDto); 
        Task UpdateCouponAsync(CreateCouponDto createCouponDto);
        Task DeleteCouponAsync(int id);
        Task<GetByIdCouponDto> GetByIdCouponAsync(int id);
    }
}
