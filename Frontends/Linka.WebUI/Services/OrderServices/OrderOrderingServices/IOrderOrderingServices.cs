using Linka.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace Linka.WebUI.Services.OrderServices.OrderOrderingServices
{
    public interface IOrderOrderingServices
    {
        Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id);
    }
}
