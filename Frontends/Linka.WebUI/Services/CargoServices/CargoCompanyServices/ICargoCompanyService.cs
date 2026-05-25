using Linka.DtoLayer.CargoDtos.CargoCompanyDtos;

namespace Linka.WebUI.Services.CargoServices.CargoCompanyServices
{
    public interface ICargoCompanyService
    {
        Task<List<ResultCargoCompanyDto>> GetAllCargoCompanyAsync();
        Task CreateCargoCompanyAsync(CreateCargoCompanyDto createCargoCompanyDto);
        Task UpdateCargoCompanyAsync(UpdateCargoCompanyDto updateCargoCompanyDto);
        Task DeleteCargoCompanyAsync(int id);
        Task<UpdateCargoCompanyDto> GetByIdCargoCompanyAsync(int id);
    }
}
