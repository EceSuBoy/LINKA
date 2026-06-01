using Linka.DtoLayer.CatalogDtos.ProductDetailDtos;

namespace Linka.WebUI.Services.CatalogServices.ProductDetailServices
{
    public interface IProductDetailService
    {
        Task<List<ResultProductDetailDto>> GetAllProductDetailAsync();

        Task<GetByIdProductDetailDto?> GetByIdProductDetailAsync(string id);

        Task<GetByIdProductDetailDto?> GetByProductIdProductDetailAsync(string productId);

        Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto);

        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto);

        Task DeleteProductDetailAsync(string id);
    }
}
