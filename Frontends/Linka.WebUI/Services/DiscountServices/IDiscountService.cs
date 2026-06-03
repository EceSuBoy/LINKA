using Linka.DtoLayer.DiscountDtos;

namespace Linka.WebUI.Services.DiscountServices
{
    public interface IDiscountService
    {
        Task<List<ResultDiscountCouponDto>>
            GetAllDiscountCouponsAsync();

        Task<GetByIdDiscountCouponDto?>
            GetByIdDiscountCouponAsync(
                int id);

        Task CreateDiscountCouponAsync(
            CreateDiscountCouponDto dto);

        Task UpdateDiscountCouponAsync(
            UpdateDiscountCouponDto dto);

        Task DeleteDiscountCouponAsync(
            int id);

        Task<int> GetDiscountCouponCountRate(
            string code);
    }
}