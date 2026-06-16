using Linka.DtoLayer.CatalogDtos.ProductDtos;

namespace Linka.WebUI.Services.CatalogServices.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<UpdateProductDto> GetByIdProductAsync(string id);
        Task DecreaseProductStockAsync(
    DecreaseProductStockDto dto);
        Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync();
        Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string categoryId);

        Task<
    PagedProductResultDto<
        ResultProductWithCategoryDto>>
    GetPagedProductsWithCategoryAsync(
        string? search,
        string? categoryId,
        int page,
        int pageSize);

        Task<List<ResultProductDto>>
    GetFeaturedProductsAsync();

    }
}
