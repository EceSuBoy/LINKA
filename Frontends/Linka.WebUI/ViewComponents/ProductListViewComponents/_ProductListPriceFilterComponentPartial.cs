using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListPriceFilterComponentPartial
        : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListPriceFilterComponentPartial(
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
            List<ResultProductWithCategoryDto> products;

            if (string.IsNullOrWhiteSpace(id))
            {
                products =
                    await _productService
                        .GetProductsWithCategoryAsync();
            }
            else
            {
                products =
                    await _productService
                        .GetProductsWithCategoryByCategoryIdAsync(id);
            }

            if (discountedOnly)
            {
                products =
                    products
                        .Where(x =>
                            x.DiscountRate > 0 &&
                            x.DiscountRate <= 100)
                        .ToList();
            }



            var ranges =
                new List<ProductListPriceRangeViewModel>
                {
                    new ProductListPriceRangeViewModel
                    {
                        Label = "All Prices"
                    },

                    new ProductListPriceRangeViewModel
                    {
                        Label = "0 ₺ - 1.000 ₺",
                        MinPrice = 0,
                        MaxPrice = 1000
                    },

                    new ProductListPriceRangeViewModel
                    {
                        Label = "1.000 ₺ - 10.000 ₺",
                        MinPrice = 1000,
                        MaxPrice = 10000
                    },

                    new ProductListPriceRangeViewModel
                    {
                        Label = "10.000 ₺ - 50.000 ₺",
                        MinPrice = 10000,
                        MaxPrice = 50000
                    },

                    new ProductListPriceRangeViewModel
                    {
                        Label = "50.000 ₺ - 100.000 ₺",
                        MinPrice = 50000,
                        MaxPrice = 100000
                    },

                    new ProductListPriceRangeViewModel
                    {
                        Label = "100.000 ₺ and above",
                        MinPrice = 100000
                    }
                };

            foreach (var range in ranges)
            {
                range.ProductCount =
                    products.Count(product =>
                        IsProductInRange(
                            GetSalePrice(product),
                            range.MinPrice,
                            range.MaxPrice));
            }

            var model =
    new ProductListPriceFilterViewModel
    {
        CategoryId =
            id,

        SelectedMinPrice =
            minPrice,

        SelectedMaxPrice =
            maxPrice,

        

        Sort =
            string.IsNullOrWhiteSpace(sort)
                ? "default"
                : sort,

        PageSize =
            pageSize,

        DiscountedOnly =
    discountedOnly,

        PriceRanges =
            ranges
    };

            return View(model);
        }

        private static decimal GetSalePrice(
            ResultProductWithCategoryDto product)
        {
            var hasDiscount =
                product.DiscountRate > 0 &&
                product.DiscountRate <= 100;

            if (!hasDiscount)
            {
                return product.ProductPrice;
            }

            return product.ProductPrice -
                   (product.ProductPrice *
                    product.DiscountRate / 100m);
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