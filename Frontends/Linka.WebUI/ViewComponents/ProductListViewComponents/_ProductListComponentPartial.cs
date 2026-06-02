using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Linka.WebUI.Services.CommentServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public _ProductListComponentPartial(
            IProductService productService,
            ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
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

            var commentStatistics =
                await _commentService
                    .GetAllProductCommentStatisticsAsync();

            var statisticsByProductId =
                commentStatistics.ToDictionary(
                    x => x.ProductId,
                    x => x);

            var model =
                products.Select(product =>
                {
                    statisticsByProductId.TryGetValue(
                        product.ProductId,
                        out var statistic);

                    return new ProductListItemViewModel
                    {
                        ProductId =
                            product.ProductId,

                        ProductName =
                            product.ProductName,

                        ProductPrice =
                            product.ProductPrice,

                        DiscountRate =
                            product.DiscountRate,

                        ProductImageUrl =
                            product.ProductImageUrl,

                        ProductDescription =
                            product.ProductDescription,

                        CategoryId =
                            product.CategoryId,

                        CategoryName =
                            product.Category?.CategoryName ??
                            string.Empty,

                        CommentCount =
                            statistic?.CommentCount ?? 0,

                        AverageRating =
                            statistic?.AverageRating ?? 0
                    };
                })
                .ToList();

            /*
             * Önce filtreleme yapılır.
             */
            if (minPrice.HasValue)
            {
                model =
                    model.Where(x =>
                            x.DiscountedPrice >= minPrice.Value)
                        .ToList();
            }

            if (maxPrice.HasValue)
            {
                model =
                    model.Where(x =>
                            x.DiscountedPrice < maxPrice.Value)
                        .ToList();
            }

            /*
             * Ardından sorting uygulanır.
             */
            sort =
                string.IsNullOrWhiteSpace(sort)
                    ? "default"
                    : sort.ToLowerInvariant();

            model =
                sort switch
                {
                    "popularity" =>
                        model
                            .OrderByDescending(x =>
                                x.CommentCount)
                            .ThenBy(x =>
                                x.ProductName)
                            .ToList(),

                    "best-rating" =>
                        model
                            .OrderByDescending(x =>
                                x.AverageRating)
                            .ThenByDescending(x =>
                                x.CommentCount)
                            .ThenBy(x =>
                                x.ProductName)
                            .ToList(),

                    "price-asc" =>
                        model
                            .OrderBy(x =>
                                x.DiscountedPrice)
                            .ThenBy(x =>
                                x.ProductName)
                            .ToList(),

                    "price-desc" =>
                        model
                            .OrderByDescending(x =>
                                x.DiscountedPrice)
                            .ThenBy(x =>
                                x.ProductName)
                            .ToList(),

                    _ =>
                        model
                            .OrderBy(x =>
                                x.ProductName)
                            .ToList()
                };

            /*
             * Son olarak yalnızca seçili sayfanın ürünleri alınır.
             */
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize != 10 &&
                pageSize != 20 &&
                pageSize != 30)
            {
                pageSize = 10;
            }

            model =
                model
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return View(model);
        }
    }
}
