using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListPaginationComponentPartial
        : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListPaginationComponentPartial(
            IProductService productService)
        {
            _productService =
                productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string? id,
            decimal? minPrice,
            decimal? maxPrice,
            string? sort,
            int page,
            int pageSize)
        {
            var products =
                string.IsNullOrWhiteSpace(id)
                    ? await _productService
                        .GetProductsWithCategoryAsync()
                    : await _productService
                        .GetProductsWithCategoryByCategoryIdAsync(id);

            var totalProductCount =
                products.Count(product =>
                    IsProductInRange(
                        GetSalePrice(product),
                        minPrice,
                        maxPrice));

            if (pageSize != 10 &&
                pageSize != 20 &&
                pageSize != 30)
            {
                pageSize = 10;
            }

            var totalPages =
                (int)Math.Ceiling(
                    totalProductCount /
                    (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }

            if (totalPages > 0 &&
                page > totalPages)
            {
                page = totalPages;
            }

            var model =
                new ProductListPaginationViewModel
                {
                    CategoryId =
                        id,

                    MinPrice =
                        minPrice,

                    MaxPrice =
                        maxPrice,

                    Sort =
                        string.IsNullOrWhiteSpace(sort)
                            ? "default"
                            : sort,

                    CurrentPage =
                        page,

                    PageSize =
                        pageSize,

                    TotalProductCount =
                        totalProductCount,

                    TotalPages =
                        totalPages
                };

            return View(model);
        }

        private static decimal GetSalePrice(
            ResultProductWithCategoryDto product)
        {
            var hasDiscount =
                product.DiscountRate > 0 &&
                product.DiscountRate <= 100;

            return hasDiscount
                ? product.ProductPrice -
                  (product.ProductPrice *
                   product.DiscountRate / 100m)
                : product.ProductPrice;
        }

        private static bool IsProductInRange(
            decimal salePrice,
            decimal? minPrice,
            decimal? maxPrice)
        {
            if (minPrice.HasValue &&
                salePrice < minPrice.Value)
            {
                return false;
            }

            if (maxPrice.HasValue &&
                salePrice >= maxPrice.Value)
            {
                return false;
            }

            return true;
        }
    }
}