using Linka.DtoLayer.CargoDtos.CargoCustomerDtos;

namespace Linka.WebUI.Services.CargoServices.CargoCustomerServices
{
    public interface ICargoCustomerService
    {
        Task<GetCargoCustomerByIdDto> GetByIdCargoCustomerInfoAsync(string id);
    }
}
