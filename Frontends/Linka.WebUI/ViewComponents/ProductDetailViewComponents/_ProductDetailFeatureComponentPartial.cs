using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailFeatureComponentPartial
        : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductDetailFeatureComponentPartial(
            IProductService productService)
        {
            _productService =
                productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string id,
            int commentCount,
            double averageRating)
        {
            var product =
                await _productService
                    .GetByIdProductAsync(id);

            var model =
                new ProductDetailFeatureViewModel
                {
                    ProductId =
                        product.ProductId,

                    ProductName =
                        product.ProductName,

                    ProductPrice =
                        product.ProductPrice,

                    DiscountRate =
                        product.DiscountRate,

                    ProductDescription =
                        product.ProductDescription,

                    CommentCount =
                        commentCount,

                    AverageRating =
                        averageRating
                };

            return View(model);
        }
    }
}