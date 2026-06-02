using Linka.DtoLayer.OrderDtos
    .OrderOrderingDtos;

namespace Linka.WebUI.Services
    .OrderServices.OrderOrderingServices
{
    public interface IOrderOrderingServices
    {
        Task<List<ResultOrderingByUserIdDto>>
            GetOrderingByUserId(string id);

        Task<int>
            CreateOrderingWithDetailsAsync(
                CreateOrderingWithDetailsDto dto);

        Task<ResultOrderingDetailDto?>
    GetOrderingDetailAsync(
        int orderingId,
        string userId);

        Task<List<ResultOrderingDto>>
    GetAllOrderingAsync();

        Task UpdateOrderingStatusAsync(
            UpdateOrderingStatusDto dto);
    }
}