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
            /*
             * Ürünleri Catalog mikroservisinden getiriyoruz.
             */
            var products =
                await _productService
                    .GetAllProductAsync();

            /*
             * Bütün ürün yorum istatistiklerini Comment
             * mikroservisinden tek HTTP isteğiyle alıyoruz.
             */
            var commentStatistics =
                await _commentService
                    .GetAllProductCommentStatisticsAsync();

            /*
             * Her ürün için tekrar tekrar liste içinde arama
             * yapmamak için ProductId üzerinden dictionary oluşturuyoruz.
             */
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

                        ProductPrice =
                            product.ProductPrice,

                        ProductImageUrl =
                            product.ProductImageUrl,

                        ProductDescription =
                            product.ProductDescription,

                        CategoryId =
                            product.CategoryId,

                        /*
                         * Ürün hakkında hiç yorum yoksa
                         * varsayılan olarak 0 kullanıyoruz.
                         */
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