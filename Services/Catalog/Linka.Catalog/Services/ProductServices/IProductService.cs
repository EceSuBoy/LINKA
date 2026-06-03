using Linka.Catalog.Dtos.ProductDtos;

namespace Linka.Catalog.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<GetByIdProductDto> GetByIdProductAsync(string id);
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync();
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string categoryId);

        Task<
    PagedProductResultDto<
        ResultProductsWithCategoryDto>>
    GetPagedProductsWithCategoryAsync(
        string? search,
        string? categoryId,
        int page,
        int pageSize);

        Task<List<ResultProductDto>>
    GetFeaturedProductsAsync();


    }
}
