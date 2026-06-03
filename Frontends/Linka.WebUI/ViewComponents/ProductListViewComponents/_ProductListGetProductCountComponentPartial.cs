using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListGetProductCountComponentPartial
        : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListGetProductCountComponentPartial(
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
            int pageSize,
            bool discountedOnly)
        {
            var products =
                string.IsNullOrWhiteSpace(id)
                    ? await _productService
                        .GetProductsWithCategoryAsync()
                    : await _productService
                        .GetProductsWithCategoryByCategoryIdAsync(id);

            if (discountedOnly)
            {
                products =
                    products
                        .Where(x =>
                            x.DiscountRate > 0 &&
                            x.DiscountRate <= 100)
                        .ToList();
            }

            var totalProductCount =
                products.Count(product =>
                    IsProductInRange(
                        GetSalePrice(product),
                        minPrice,
                        maxPrice));

            var model =
                new ProductListToolbarViewModel
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

                    PageSize =
                        pageSize,

                    TotalProductCount =
                        totalProductCount,

                    DiscountedOnly =
    discountedOnly,
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