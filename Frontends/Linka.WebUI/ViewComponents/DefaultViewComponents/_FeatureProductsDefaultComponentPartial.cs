using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Linka.WebUI.Services.CommentServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureProductsDefaultComponentPartial
        : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public _FeatureProductsDefaultComponentPartial(
            IProductService productService,
            ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products =
    await _productService
        .GetFeaturedProductsAsync();

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

                    return new FeaturedProductViewModel
                    {
                        ProductId =
                            product.ProductId,

                        ProductName =
                            product.ProductName,

                        DiscountRate =
                            product.DiscountRate,

                        ProductPrice =
                            product.ProductPrice,

                        ProductImageUrl =
                            product.ProductImageUrl,

                        ProductDescription =
                            product.ProductDescription,

                        CategoryId =
                            product.CategoryId,

                        CommentCount =
                            statistic?.CommentCount ?? 0,

                        AverageRating =
                            statistic?.AverageRating ?? 0
                    };
                })
                .ToList();

            return View(model);
        }
    }
}