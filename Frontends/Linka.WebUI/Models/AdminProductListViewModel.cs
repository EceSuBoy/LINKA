using Linka.DtoLayer.CatalogDtos.CategoryDtos;
using Linka.DtoLayer.CatalogDtos.ProductDtos;

namespace Linka.WebUI.Models
{
    public class AdminProductListViewModel
    {
        public List<ResultCategoryDto> Categories { get; set; } =
            new List<ResultCategoryDto>();

        public PagedProductResultDto<
            ResultProductWithCategoryDto> Products
        { get; set; } =
                new PagedProductResultDto<
                    ResultProductWithCategoryDto>();
    }
}