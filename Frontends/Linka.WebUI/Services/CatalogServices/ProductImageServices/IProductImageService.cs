using Linka.DtoLayer.CatalogDtos.ProductImageDtos;

namespace Linka.WebUI.Services.CatalogServices.ProductImageServices
{
    public interface IProductImageService
    {
        Task<List<ResultProductImageDto>> GetAllProductImageAsync();

        Task<GetByIdProductImageDto?> GetByIdProductImageAsync(string id);

        Task<GetByIdProductImageDto?> GetByProductIdProductImageAsync(
            string productId);

        Task CreateProductImageAsync(
            CreateProductImageDto createProductImageDto);

        Task UpdateProductImageAsync(
            UpdateProductImageDto updateProductImageDto);

        Task DeleteProductImageAsync(string id);
    }
}
