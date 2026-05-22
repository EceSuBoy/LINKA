using Linka.DtoLayer.OrderDtos.OrderAddressDtos;

namespace Linka.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressServices
    {
       // Task<List<ResultAboutDto>> GetAllAboutAsync();
        Task CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto);
        //Task UpdateAboutAsync(UpdateAboutDto updateAboutDto);
        //Task DeleteAboutAsync(string id);
        //Task<GetByIdAboutDto> GetByIdAboutAsync(string id);
    }
}
